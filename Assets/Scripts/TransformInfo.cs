using UnityEngine;

public struct TransformInfo {

	public Quaternion rotation;
	public Vector3 position;

	public TransformInfo(Vector3 position, Quaternion rotation) {
		this.position = position;
		this.rotation = rotation;
	}
}
