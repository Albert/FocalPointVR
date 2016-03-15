using UnityEngine;
using System.Collections;

public class FocalPointHandler : MonoBehaviour {
	public bool lockTranslation = false;
	public bool lockRotation = false;
	public bool lockScale = false;
	public bool translationInertia = false;
	public bool rotationInertia = false;
	public bool scaleInertia = false;
	public float decayTime = 0.5f;

	private Rigidbody rBody;
	private bool wasKinematic;
	private bool isActivated;
	private bool lastIsActivated;
	private Vector3 lastPosition;
	private Vector3 penultimatePosition;
	private Vector3 lastLookAt;
	private Vector3 penultimateLookAt;
	private Vector3 lastScale;
	private Vector3 penultimateScale;
	private float timeOfRelease = 0.0f;

	void Start () {
		rBody = GetComponent<Rigidbody> ();
		if (rBody != null) {
			wasKinematic = rBody.isKinematic;
		}
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
				lastScale = transform.lossyScale;
			}
		}
		// TODO: clean up all these if statements
		if (lastIsActivated != isActivated) {
			if (rBody != null && !wasKinematic) {
				rBody.isKinematic = isActivated;
				if (!isActivated) {
					if (translationInertia) {
						rBody.velocity = (lastPosition - penultimatePosition) / Time.deltaTime;
					}
					if (rotationInertia) {
//						http://answers.unity3d.com/questions/48836/determining-the-torque-needed-to-rotate-an-object.html
						Vector3 x = Vector3.Cross (penultimateLookAt, lastLookAt);
						float theta = Mathf.Asin (x.magnitude);
						Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
						Quaternion q = transform.rotation * rBody.inertiaTensorRotation;
						Vector3 T = q * Vector3.Scale (rBody.inertiaTensor, (Quaternion.Inverse (q) * w));
						rBody.AddTorque (T, ForceMode.Impulse);
					}
				}
			} else {
				if (!isActivated) {
					timeOfRelease = Time.time;
				}
			}
		}

		if (!isActivated && decayTime > (Time.time - timeOfRelease)) {
			float lerpT = (decayTime - (Time.time - timeOfRelease)) / decayTime;
			if (translationInertia) {
				Vector3 full = lastPosition - penultimatePosition;
				Vector3 lerpPartial = Vector3.Lerp (Vector3.zero, full, lerpT);
				transform.position += lerpPartial;
			}
			if (rotationInertia) {
				Quaternion full = Quaternion.FromToRotation (transform.InverseTransformDirection (penultimateLookAt), transform.InverseTransformDirection (lastLookAt));
				Quaternion lerpPartial = Quaternion.Slerp (Quaternion.identity, full, (decayTime - (Time.time - timeOfRelease)) / decayTime);
				transform.rotation *= lerpPartial;
			}
			if (scaleInertia) {
				Vector3 full = (lastScale - penultimateScale);
				Vector3 lerpPartial = Vector3.Lerp (Vector3.zero, full, lerpT);
				transform.localScale += lerpPartial;
			}
		}

		lastIsActivated = isActivated;
	}

	public void setFakeActive(bool state) {
		isActivated = state;
	}
}