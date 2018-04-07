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
		runner.AddInput (graph ["input_1"] [0], new float[,]{{ 	head.rotation.x, head.rotation.y, head.rotation.z, head.rotation.w, head.position.x * scalingFactor, head.position.y * scalingFactor, head.position.z *scalingFactor,
				hand_left.rotation.x, hand_left.rotation.y, hand_left.rotation.z, hand_left.rotation.w, hand_left.position.x* scalingFactor, hand_left.position.y* scalingFactor, hand_left.position.z* scalingFactor,
				hand_right.rotation.x, hand_right.rotation.y, hand_right.rotation.z, hand_right.rotation.w, hand_right.position.x* scalingFactor, hand_right.position.y* scalingFactor, hand_right.position.z* scalingFactor}});
		runner.Fetch (graph["dense_2/BiasAdd"][0]);
		float[,] output_tensor = runner.Run () [0].GetValue () as float[,];
		float x = output_tensor [0, 0];
		float y = output_tensor [0, 1];
		float z = output_tensor [0, 2];
		float w = Mathf.Sqrt(1 - x * x - y * y - z * z);
		bodyTransform.rotation = new Quaternion(x, y, z, w);
		// position the body such that the base of the neck is right under the head
		bodyTransform.position = bodyTransform.position + head.position - neck.position + positionOffset;
		Debug.Log (output_tensor [0,0] + " " + output_tensor [0,1] + " " +output_tensor [0,2] + " " +output_tensor [0,3] + " " +output_tensor [0,4] + " " +output_tensor [0,5] + " " +output_tensor [0,6]);
	}
}
