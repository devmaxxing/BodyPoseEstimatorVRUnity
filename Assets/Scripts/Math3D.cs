using UnityEngine;

public class Math3D {
	public static TransformInfo PivotY(Transform transform, Vector3 pivotPoint, float angle) {
		float angleRad = Mathf.Deg2Rad * angle;
		float s = Mathf.Sin (angleRad);
		float c = Mathf.Cos (angleRad);
		float x = transform.position.x - pivotPoint.x;
		float z = transform.position.z - pivotPoint.z;
		Vector3 newPosition = new Vector3(x*c - z*s, transform.position.y, x*s + z*c);
		Quaternion newQuaternion = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z));
		return new TransformInfo (newPosition, newQuaternion);
	}

	public static Quaternion RightToLeftHand(float x, float y, float z, float w) {
		Quaternion newRotation = new Quaternion (x, y, z, w);
		Vector3 rotation = newRotation.eulerAngles;
		newRotation.eulerAngles = new Vector3 (rotation.x, -rotation.y, rotation.z);
		return newRotation;
	}
}
