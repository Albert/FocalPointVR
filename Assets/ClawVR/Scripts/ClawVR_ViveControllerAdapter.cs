using UnityEngine;
using System.Collections;

public class ClawVR_ViveControllerAdapter : MonoBehaviour {
	private ClawVR_HandController clawController;
	private int controllerIndex;

	private Vector2 downLocation;
	private float telescopeDistanceOnTouch;
	private bool telescopeInterrupted;

	public ClawVR_InteractionManager ixdManager {get; set;}
	private GameObject hoveredSubject;

    void Start () {
        clawController = GetComponentInChildren<ClawVR_HandController>();
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
//		if (Input.GetKeyDown(KeyCode.Space)) {
		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			clawController.CloseClaw();
		}
//		if (Input.GetKeyUp(KeyCode.Space)) {
		if (SteamVR_Controller.Input(controllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
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
                foreach (ClawVR_HandController controller in ixdManager.clawControllers) {
                    controller.TelescopeAbsolutely(telescopeDistanceOnTouch + delta.y * 0.6f);
                }
//                clawController.TelescopeAbsolutely(telescopeDistanceOnTouch + delta.y * 0.6f);
			}
		}
		if (SteamVR_Controller.Input(controllerIndex).GetPressDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) {
            if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y > 0) {
//		For this offline code to work, the above two if blocks must also be commented out because they throw blocking
//		if (true == true) {
//			if (Input.GetKeyDown(KeyCode.UpArrow)) {
                foreach (ClawVR_HandController controller in ixdManager.clawControllers) {
                    Collider subjCollider = ixdManager.subject.GetComponent<Collider>();
                    float delta = (ixdManager.subject.transform.position - transform.position).magnitude - subjCollider.bounds.extents.magnitude / 2.0f - 2.0f;
                    if (delta > controller.GetScopeDistance()) {
                        controller.TelescopeAbsolutely(delta);
                    }
                }
//				clawController.DeployLaser ();
				telescopeInterrupted = true;
            } else if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y < -0) {
//			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                foreach (ClawVR_HandController controller in ixdManager.clawControllers) {
                    controller.TelescopeAbsolutely(0.0f);
                }
//				clawController.TelescopeAbsolutely(0.0f);
				telescopeInterrupted = true;
			}
		}
	}

	void findSelection() {
		if (SteamVR_Controller.Input (controllerIndex).GetHairTriggerDown()) {
			ixdManager.selectionMode = true;
		}
        if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x > 0.999f) {
			ixdManager.changeSubject(clawController.hoveredSubject);
		}
        if (SteamVR_Controller.Input(controllerIndex).GetHairTriggerUp()) {
            ixdManager.selectionMode = false;
		}
	}
}