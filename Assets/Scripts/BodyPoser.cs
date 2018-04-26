using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public abstract class BodyPoser : MonoBehaviour {
	[SerializeField]
	protected string model;

	[SerializeField]
	protected string outputLayer;

	[SerializeField]
	protected string inputLayer;

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

	void Start () {
		if (model != null) {
			TextAsset graphModel = Resources.Load (model) as TextAsset;
			graph = new TFGraph ();
			graph.Import (graphModel.bytes);
			session = new TFSession (graph);
		}
	}

	void Update () {
		AdjustPose ();
	}

	public abstract void AdjustPose ();
}
