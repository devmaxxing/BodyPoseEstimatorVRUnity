using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public class Body : MonoBehaviour {
	[SerializeField]
	private string model;

	[SerializeField]
	private float scalingFactor;

	[SerializeField]
	private Transform bodyTransform;

	[SerializeField]
	private Transform head;
	[SerializeField]
	private Transform hand_left;
	[SerializeField]
	private Transform hand_right;

	private TFGraph graph;
	private TFSession session;

	public float[,] output_tensor;

	// Use this for initialization
	void Start () {
		bodyTransform = GetComponent<Transform> ();
		TextAsset graphModel = Resources.Load (model) as TextAsset;
		graph = new TFGraph ();
		graph.Import (graphModel.bytes);
		session = new TFSession (graph);

	}
	
	// Update is called once per frame
	void Update () {
		TFSession.Runner runner = session.GetRunner ();
		runner.AddInput (graph ["input_1"] [0], new float[,]{{ 	head.rotation.x, head.rotation.y, head.rotation.z, head.rotation.w, head.position.x * scalingFactor, head.position.y * scalingFactor, head.position.z *scalingFactor,
				hand_left.rotation.x, hand_left.rotation.y, hand_left.rotation.z, hand_left.rotation.w, hand_left.position.x* scalingFactor, hand_left.position.y* scalingFactor, hand_left.position.z* scalingFactor,
				hand_right.rotation.x, hand_right.rotation.y, hand_right.rotation.z, hand_right.rotation.w, hand_right.position.x* scalingFactor, hand_right.position.y* scalingFactor, hand_right.position.z* scalingFactor}});
		runner.Fetch (graph["dense_2/BiasAdd"][0]);
		output_tensor = runner.Run () [0].GetValue () as float[,];
		bodyTransform.rotation = new Quaternion(output_tensor [0,0], output_tensor [0,1], output_tensor [0,2], output_tensor [0,3]);
		bodyTransform.position = new Vector3(output_tensor [0,4]/ scalingFactor, output_tensor [0,5]/ scalingFactor, output_tensor [0,6]/ scalingFactor);
		Debug.Log (output_tensor [0,0] + " " + output_tensor [0,1] + " " +output_tensor [0,2] + " " +output_tensor [0,3] + " " +output_tensor [0,4] + " " +output_tensor [0,5] + " " +output_tensor [0,6]);
	}
}
