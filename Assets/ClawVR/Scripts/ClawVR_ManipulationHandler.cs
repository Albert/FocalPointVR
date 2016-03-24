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
    public float flickAnimationLength = 0.3f;

    public Vector3 thisFramePosition { get; set; }
    public Quaternion thisFrameRotation { get; set; }
    public Vector3 thisFrameScale { get; set; }

    public Vector3 lastFramePosition { get; set; }
    public Quaternion lastFrameRotation { get; set; }
    public Vector3 lastFrameScale { get; set; }

    private bool isPhysicsBased;
    private Rigidbody rbody;
    private bool prevKinematicState;

    private float timeOfRelease = 0;
    private Vector3 translationInertiaVelocity;
    private Quaternion rotationInertiaDelta;

    void Start () {
        rbody = GetComponent<Rigidbody>();
        isPhysicsBased = (rbody != null && !rbody.isKinematic);
    }

	void Update () {
        if ((Time.time - timeOfRelease) < flickAnimationLength) {
            float portionOfAnimationLeft = 1 - ((Time.time - timeOfRelease) / flickAnimationLength);
            if (translationInertiaOnRelease) {
                transform.position += translationInertiaVelocity * Time.deltaTime * portionOfAnimationLeft;
            }
            if (rotationInertiaOnRelease) {
                float angle = 0.0F;
                Vector3 axis = Vector3.zero;
                rotationInertiaDelta.ToAngleAxis(out angle, out axis);
                angle *= portionOfAnimationLeft;
                transform.Rotate(axis, angle);
            }
        }
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
                rbody.velocity = (transform.position - lastFramePosition) / Time.fixedDeltaTime;
            }
            if (rotationInertiaOnRelease) {
                float angle = 0.0F;
                Vector3 axis = Vector3.zero;
                Quaternion delta = Quaternion.Inverse(lastFrameRotation) * thisFrameRotation;
                delta.ToAngleAxis(out angle, out axis);
                rbody.AddRelativeTorque(axis.normalized * angle / Time.fixedDeltaTime);
            }
        } else {
            timeOfRelease = Time.time;
            if (translationInertiaOnRelease) {
                translationInertiaVelocity = (transform.position - lastFramePosition) / Time.fixedDeltaTime;
            }
            if (rotationInertiaOnRelease) {
                rotationInertiaDelta = Quaternion.Inverse(lastFrameRotation) * thisFrameRotation;
            }
        }
    }
}