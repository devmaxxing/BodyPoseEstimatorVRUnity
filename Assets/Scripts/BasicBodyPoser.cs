using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBodyPoser : BodyPoser {

	[SerializeField]
	private float bodyOffset = 0.2f;

	public override void AdjustPose() {
		bodyTransform.position = new Vector3(head.position.x, head.position.y - bodyOffset, head.position.z);
		Quaternion newRotation = Quaternion.Euler (new Vector3 (0, head.rotation.eulerAngles.y));
		bodyTransform.rotation = newRotation;
	}
}
