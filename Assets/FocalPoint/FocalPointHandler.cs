using UnityEngine;
using System.Collections;

public class FocalPointHandler : MonoBehaviour {
	public bool lockTranslation = false;
	public bool lockRotation = false;
	public bool lockScale = false;
	public bool translationInertia = false;
	public bool rotationInertia = false;
	public bool scaleInertia = false;

	private Rigidbody rBody;
	private bool isActivated;
	private bool lastIsActivated;
	private Vector3 lastPosition;
	private Vector3 penultimatePosition;
	private Vector3 lastLookAt;
	private Vector3 penultimateLookAt;
	private Vector3 lastScale;
	private Vector3 penultimateScale;

	void Start () {
		rBody = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (isActivated) {
			if (translationInertia) {
				penultimatePosition = lastPosition;
				lastPosition = transform.position;
			}
			if (rotationInertia) {
				penultimateLookAt = lastLookAt;
				lastLookAt = transform.forward;
			}
			if (scaleInertia) {
				penultimateScale = lastScale;
				lastScale = transform.localScale;
			}
		}

		if (isActivated != lastIsActivated) {
			rBody.isKinematic = isActivated; // TODO this should be sensitive to rBody.isKinematic's existence
			if (!isActivated) {
				if (translationInertia) {
					rBody.velocity = (lastPosition - penultimatePosition) / Time.deltaTime;
				}
				if (rotationInertia) {
//					http://answers.unity3d.com/questions/48836/determining-the-torque-needed-to-rotate-an-object.html
					Vector3 x = Vector3.Cross (penultimateLookAt, lastLookAt);
					float theta = Mathf.Asin (x.magnitude);
					Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
					Quaternion q = transform.rotation * rBody.inertiaTensorRotation;
					Vector3 T = q * Vector3.Scale (rBody.inertiaTensor, (Quaternion.Inverse (q) * w));
					rBody.AddTorque (T, ForceMode.Impulse);
				}
			}
		}

		lastIsActivated = isActivated;
	}

	public void setFakeActive(bool state) {
		isActivated = state;
	}
}