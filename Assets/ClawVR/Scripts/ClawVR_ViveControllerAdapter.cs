using UnityEngine;
using System.Collections;

public class ClawVR_ViveControllerAdapter : MonoBehaviour {
	private ClawVR_GrasperController clawController;
	private int controllerIndex;

	private Vector2 downLocation;
	private float telescopeDistanceOnTouch;
	private bool telescopeInterrupted;

	public ClawVR_InteractionManager ixdManager {get; set;}
    private GameObject hoveredSubject;

    void Start () {
        clawController = GetComponentInChildren<ClawVR_GrasperController>();
		ixdManager.registerClaw (clawController);
        // TODO: when this instanciates, left should be indexed to 14, right to 15, and make a note of it
        controllerIndex = GetComponent<SteamVR_TrackedObject>().index.GetHashCode();
    }

   void Update() {
		openCloseClaw ();
		moveClaw ();
		findSelection ();
    }
		
	void openCloseClaw() {
		if (Input.GetKeyDown(KeyCode.Space)) {
//		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			clawController.CloseClaw();
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
//		if (SteamVR_Controller.Input(controllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
			clawController.OpenClaw();
		}
	}

	void moveClaw() {
        if (SteamVR_Controller.Input(controllerIndex).GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
            downLocation = SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
			telescopeDistanceOnTouch = clawController.GetScopeDistance();
			telescopeInterrupted = false;
		}
		if (SteamVR_Controller.Input(controllerIndex).GetTouch(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
            Vector2 delta = SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) - downLocation;
			if (Mathf.Abs(delta.y) > 0.2 && !telescopeInterrupted) {
				clawController.TelescopeAbsolutely(telescopeDistanceOnTouch + delta.y * 0.6f);
			}
		}
//		For this offline code to work, the above two if blocks must also be commented out because they throw blocking
//		if (true == true) {
//			if (Input.GetKeyDown(KeyCode.UpArrow)) {
		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
            if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y > 0) {
				clawController.DeployLaser ();
				telescopeInterrupted = true;
            } else if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y < -0) {
				clawController.TelescopeAbsolutely(0.0f);
				telescopeInterrupted = true;
			}
		}
	}

	void findSelection() {
		Ray controllerRay = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(controllerRay, out hit)) {
			if (!hoveredSubject || !hoveredSubject.Equals(hit.collider.gameObject)) {
				hoveredSubject = hit.collider.gameObject;
			}
		} else {
			hoveredSubject = null;
		}
		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			ixdManager.changeSubject(hoveredSubject);
		}
	}
}