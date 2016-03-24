using UnityEngine;
using System.Collections;

public class ClawVR_ManipulationHandler : MonoBehaviour {
	public bool isSelectable = true;
	public bool lockTranslation;
	public bool lockRotation;
	public bool lockScale;
    public bool translationInertiaOnRelease;
    public bool rotationInertiaOnRelease;
    public bool scaleInertiaOnRelease;

    public Vector3 thisFramePosition { get; set; }
    public Quaternion thisFrameRotation { get; set; }
    public Vector3 thisFrameScale { get; set; }

    public Vector3 lastFramePosition { get; set; }
    public Quaternion lastFrameRotation { get; set; }
    public Vector3 lastFrameScale { get; set; }

    private bool isPhysicsBased;
    private Rigidbody rbody;
    private bool prevKinematicState;

    void Start () {
        rbody = GetComponent<Rigidbody>();
        isPhysicsBased = (rbody != null && !rbody.isKinematic);
    }

	void Update () {
    }

    public void capture() {
        if (isPhysicsBased) {
            prevKinematicState = rbody.isKinematic;
            rbody.isKinematic = true;
        }
    }

    public void release() {
        if (isPhysicsBased) {
            rbody.isKinematic = prevKinematicState;
			if (translationInertiaOnRelease) {
				rbody.velocity = (transform.position - lastFramePosition) / Time.deltaTime;
			}
			if (rotationInertiaOnRelease) {
                float angle = 0.0F;
                Vector3 axis = Vector3.zero;
                Quaternion delta = Quaternion.Inverse(lastFrameRotation) * thisFrameRotation;
                delta.ToAngleAxis(out angle, out axis);
                print(Time.deltaTime);
                rbody.AddRelativeTorque(axis.normalized * angle / Time.fixedDeltaTime);
			}
        }
    }
}