using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public class BodyRotationPoser : BodyPoser{

	[SerializeField]
	private Transform neck;

	[SerializeField]
	private Vector3 positionOffset = new Vector3(0f, -0.1f, 0f);

	public override void AdjustPose() {
		TFSession.Runner runner = session.GetRunner ();

		//Our model has the best performance when the head's y-rotation is 0
		//so we pivot the head and hand's about the head before passing the inputs into the model
		Vector3 rotationPoint = head.position;
		float headRotation = head.rotation.eulerAngles.y;
		float rotationAmount = -1 * headRotation;

		TransformInfo pivotedHead = Math3D.PivotY(head, rotationPoint, rotationAmount);
		TransformInfo pivotedHandLeft = Math3D.PivotY(hand_left, rotationPoint, rotationAmount);
		TransformInfo pivotedHandRight = Math3D.PivotY(hand_right, rotationPoint, rotationAmount);

		runner.AddInput (graph [inputLayer] [0], new float[,]{{ 	pivotedHead.rotation.x, pivotedHead.rotation.y, pivotedHead.rotation.z, pivotedHead.rotation.w, pivotedHead.position.x * scalingFactor, pivotedHead.position.y * scalingFactor, pivotedHead.position.z *scalingFactor,
				pivotedHandLeft.rotation.x, pivotedHandLeft.rotation.y, pivotedHandLeft.rotation.z, pivotedHandLeft.rotation.w, pivotedHandLeft.position.x* scalingFactor, pivotedHandLeft.position.y* scalingFactor, pivotedHandLeft.position.z* scalingFactor,
				pivotedHandRight.rotation.x, pivotedHandRight.rotation.y, pivotedHandRight.rotation.z, pivotedHandRight.rotation.w, pivotedHandRight.position.x* scalingFactor, pivotedHandRight.position.y* scalingFactor, pivotedHandRight.position.z* scalingFactor}});
		runner.Fetch (graph[outputLayer][0]);
		float[,] output_tensor = runner.Run () [0].GetValue () as float[,];
		float x = output_tensor [0, 0];
		float y = output_tensor [0, 1];
		float z = output_tensor [0, 2];
		float w = Mathf.Sqrt(Mathf.Abs(1 - x * x - y * y - z * z));
		bodyTransform.rotation = Math3D.RightToLeftHand (x, y, z, w);
		bodyTransform.Rotate (new Vector3 (0, headRotation));
		// position the body such that the base of the neck is right under the head
		bodyTransform.position = bodyTransform.position + head.position - neck.position + positionOffset;

		Debug.Log (output_tensor [0,0] + " " + output_tensor [0,1] + " " +output_tensor [0,2]);
	}
}