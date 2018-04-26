using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public class BodyRotationPoser : BodyPoser{

	[SerializeField]
	private Transform neck;

	[SerializeField]
	private Vector3 positionOffset = new Vector3(0f, -0.1f, 0f);

	[SerializeField]
	private Transform pivoted_left;

	[SerializeField]
	private Transform pivoted_right;

	[SerializeField]
	private Transform pivoted_head;

	public override void AdjustPose() {
		TFSession.Runner runner = session.GetRunner ();

		//Assume model is trained with body x and z positions and y rotation normalized to that of the head
		//Assume model is trained with right hand coordinate system

		//Pivot the head and hands in the y-axis around the head such that the head's y rotation is 0
		Vector3 rotationPoint = head.position;
		float headRotation = head.rotation.eulerAngles.y;
		float rotationAmount = headRotation;

		TransformInfo pivotedHead = Math3D.PivotY(head, rotationPoint, rotationAmount);
		TransformInfo pivotedHandLeft = Math3D.PivotY(hand_left, rotationPoint, rotationAmount);
		TransformInfo pivotedHandRight = Math3D.PivotY(hand_right, rotationPoint, rotationAmount);

		//pivoted_head.position = pivotedHead.position;
		//pivoted_head.rotation = pivotedHead.rotation;

		//pivoted_left.position = pivotedHandLeft.position;
		//pivoted_left.rotation = pivotedHandLeft.rotation;

		//pivoted_right.position = pivotedHandRight.position;
		//pivoted_right.rotation = pivotedHandRight.rotation;

		//Convert rotations to right hand coordinate system
		Quaternion convertedHeadRotation = Math3D.RightToLeftHand (pivotedHead.rotation.x, pivotedHead.rotation.y, pivotedHead.rotation.z, pivotedHead.rotation.w);
		Quaternion convertedLeftHandRotation = Math3D.RightToLeftHand (pivotedHead.rotation.x, pivotedHead.rotation.y, pivotedHead.rotation.z, pivotedHead.rotation.w);
		Quaternion convertedRightHandRotation = Math3D.RightToLeftHand (pivotedHead.rotation.x, pivotedHead.rotation.y, pivotedHead.rotation.z, pivotedHead.rotation.w);

		float[,] inputs = new float[,] { { 
				convertedHeadRotation.x,
				convertedHeadRotation.y,
				convertedHeadRotation.z,
				convertedHeadRotation.w, 
				0,
				pivotedHead.position.y * scalingFactor,
				0,
				convertedLeftHandRotation.x,
				convertedLeftHandRotation.y,
				convertedLeftHandRotation.z,
				convertedLeftHandRotation.w, 
				pivotedHandLeft.position.x * scalingFactor,
				pivotedHandLeft.position.y * scalingFactor,
				pivotedHandLeft.position.z * scalingFactor,
				convertedRightHandRotation.x,
				convertedRightHandRotation.y,
				convertedRightHandRotation.z,
				convertedRightHandRotation.w, 
				pivotedHandRight.position.x * scalingFactor,
				pivotedHandRight.position.y * scalingFactor,
				pivotedHandRight.position.z * scalingFactor
			}
		};

		runner.AddInput (graph [inputLayer] [0], inputs);
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
	}
}