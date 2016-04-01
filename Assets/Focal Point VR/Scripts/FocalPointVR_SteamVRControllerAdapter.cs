using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRControllerAdapter : MonoBehaviour {
    private FocalPointVR_PointGenerator pointGenerator;
    private int controllerIndex;

    public FocalPointVR_InteractionManager ixdManager { get; set; }
    private SteamVR_TrackedObject steamTrackedObj;

    void Start() {
        pointGenerator = GetComponentInChildren<FocalPointVR_PointGenerator>();
        if (ixdManager == null) {
            ixdManager = GameObject.FindObjectOfType<FocalPointVR_InteractionManager> ();
        }
        ixdManager.registerPointGenerator(pointGenerator);
        steamTrackedObj = GetComponent<SteamVR_TrackedObject>();
        controllerIndex = GetComponent<SteamVR_TrackedObject>().index.GetHashCode();
    }

    void Update() {
        // TODO -- this is quite inefficient -- would be interested in a better implementation
        controllerIndex = steamTrackedObj.index.GetHashCode();
        if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            pointGenerator.ClosePincer();
        }
        if (SteamVR_Controller.Input(controllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
            pointGenerator.OpenPincer();
        }
    }
}
