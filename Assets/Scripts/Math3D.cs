using UnityEngine;
using System;

public class Math3D {
	public static TransformInfo PivotY(Transform transform, Vector3 pivotPoint, float angle) {
		double angleRad = Math.PI * angle / 180f;
		double s = Math.Sin (angleRad);
		double c = Math.Cos (angleRad);
		float x = transform.position.x - pivotPoint.x;
		float z = transform.position.z - pivotPoint.z;

		float newX = (float)((x * c) - (z * s));
		float newZ = (float)((x * s) + (z * c));

		Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
		Quaternion newQuaternion = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - angle, transform.eulerAngles.z));
		return new TransformInfo (newPosition, newQuaternion);
	}

	public static Quaternion RightToLeftHand(float x, float y, float z, float w) {
		Quaternion newRotation = new Quaternion (x, y, z, w);
		Vector3 rotation = newRotation.eulerAngles;
		newRotation.eulerAngles = new Vector3 (rotation.x, -rotation.y, rotation.z);
		return newRotation;
	}
}
