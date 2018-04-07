using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public abstract class BodyPoser : MonoBehaviour {
	[SerializeField]
	protected string model;

	[SerializeField]
	protected float scalingFactor;

	[SerializeField]
	protected Transform bodyTransform;

	[SerializeField]
	protected Transform head;
	[SerializeField]
	protected Transform hand_left;
	[SerializeField]
	protected Transform hand_right;

	protected TFGraph graph;
	protected TFSession session;

	// Use this for initialization
	void Start () {
		TextAsset graphModel = Resources.Load (model) as TextAsset;
		graph = new TFGraph ();
		graph.Import (graphModel.bytes);
		session = new TFSession (graph);
	}
	
	// Update is called once per frame
	void Update () {
		AdjustPose ();
	}

	public abstract void AdjustPose ();
}
