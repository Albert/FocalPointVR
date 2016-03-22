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
//        if (SteamVR_Controller.Input(controllerIndex).GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
//            downLocation = SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
//			telescopeDistanceOnTouch = clawController.GetScopeDistance();
//			telescopeInterrupted = false;
//		}
//		if (SteamVR_Controller.Input(controllerIndex).GetTouch(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
//            Vector2 delta = SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) - downLocation;
//			if (Mathf.Abs(delta.y) > 0.2 && !telescopeInterrupted) {
//				clawController.TelescopeAbsolutely(telescopeDistanceOnTouch + delta.y * 0.6f);
//			}
//		}
//		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
//            if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y > 0) {
//		For this offline code to work, the above two if blocks must also be commented out because they throw blocking
		if (true == true) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				clawController.DeployLaser ();
				telescopeInterrupted = true;
//            } else if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y < -0) {
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				clawController.TelescopeAbsolutely(0.0f);
				telescopeInterrupted = true;
			}
		}
	}

	void findSelection() {
		Ray controllerRay = new Ray(transform.position, transform.forward);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (controllerRay);
		float closestDistance = 999999999.9f;
		hoveredSubject = null;
		foreach (RaycastHit hit in hits) {
			// TODO: there has to be a better way...
			if (hit.distance < closestDistance) {
				ClawVR_ManipulationHandler manipHandler = hit.collider.gameObject.GetComponent<ClawVR_ManipulationHandler> ();
				if ((manipHandler != null && manipHandler.isSelectable) || (ixdManager.canSelectAnyObject && manipHandler == null)) {
					closestDistance = hit.distance;
					hoveredSubject = hit.collider.gameObject;
				}
			}
		}

		//if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
		if (Input.GetKeyDown(KeyCode.S)) {
			ixdManager.changeSubject(hoveredSubject);
		}
	}
}