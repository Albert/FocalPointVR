using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRControllerAdapterDemo3and4 : MonoBehaviour {
    private FocalPointVR_PointGenerator pointGenerator;
    private int controllerIndex;

    public FocalPointVR_InteractionManagerDemo3and4 ixdManager { get; set; }
    private SteamVR_TrackedObject steamTrackedObj;

    public GameObject[] pathSpriteComponents { get; set; }
    private GameObject hoveredSubject;
    private GameObject otherController;

    private bool laserPressed = false;
    private bool laserMode = false;
    private Plane laserPlane;
    private float laserDistance = 0;
    public float laserDistanceMax = 7;

    void Start() {
        laserPlane = new Plane(Vector3.up, Vector3.zero);
        pointGenerator = GetComponentInChildren<FocalPointVR_PointGenerator>();
        ixdManager.registerPointGenerator(pointGenerator);
        steamTrackedObj = GetComponent<SteamVR_TrackedObject>();
        controllerIndex = GetComponent<SteamVR_TrackedObject>().index.GetHashCode();
    }

    void Update() {
        // TODO -- this is quite inefficient -- would be interested in a better implementation
        controllerIndex = steamTrackedObj.index.GetHashCode();

        if (laserMode) {
            Ray laserRay = new Ray(transform.position, transform.forward);
            if (laserPlane.Raycast(laserRay, out laserDistance) && laserDistance < laserDistanceMax) {
                pointGenerator.transform.localPosition = Vector3.forward * laserDistance;
            } else {
                setLaserMode(false);
            }
        }
        if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            pointGenerator.ClosePincer();
        }
        if (SteamVR_Controller.Input(controllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
            pointGenerator.OpenPincer();
        }
        if (SteamVR_Controller.Input(controllerIndex).GetHairTriggerDown()) {
            setLaserMode(true);
        }
        if (SteamVR_Controller.Input(controllerIndex).GetHairTriggerUp()) {
            setLaserMode(false);
        }
        // workaround: SteamVR onTriggerDown / Up seems to be buggy (or I just didn't get it)
        if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x > 0.999f) {
            if (!laserPressed) {
                Ray laserRay = new Ray(transform.position, transform.forward);
                if (laserPlane.Raycast(laserRay, out laserDistance) && laserDistance < laserDistanceMax) {
                    pointGenerator.transform.localPosition = Vector3.forward * laserDistance;
                    pointGenerator.ClosePincer();
                }
            }
            laserPressed = true;
        } else {
            if (laserPressed) {
                pointGenerator.OpenPincer();
                pointGenerator.transform.localPosition = Vector3.zero;
            }
            laserPressed = false;
        }
    }

    void setLaserMode(bool state) {
        if (state) {
            displayPathSprite(0);
        } else {
            displayPathSprite(-99);
            pointGenerator.OpenPincer();
            pointGenerator.transform.localPosition = Vector3.zero;
        }
        laserMode = state;
    }

    void displayPathSprite(int index) {
        for (int i = 0; i < pathSpriteComponents.Length; i++) {
            pathSpriteComponents[i].SetActive(index == i || index + 1 == i);
        }
    }
}
