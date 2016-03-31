using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRControllerAdapterDemo2 : MonoBehaviour {
    private FocalPointVR_PointGenerator pointGenerator;
    private int controllerIndex;

    public FocalPointVR_InteractionManager ixdManager { get; set; }
    private SteamVR_TrackedObject steamTrackedObj;

    public GameObject[] pathSpriteComponents { get; set; }
    private bool selectionMode;
    private GameObject hoveredSubject;
    public bool syndromeMode { get; set; }

    void Start() {
        pointGenerator = GetComponentInChildren<FocalPointVR_PointGenerator>();
        ixdManager.registerPointGenerator(pointGenerator);
        steamTrackedObj = GetComponent<SteamVR_TrackedObject>();
        controllerIndex = GetComponent<SteamVR_TrackedObject>().index.GetHashCode();
    }

    void Update() {
        // TODO -- this is quite inefficient -- would be interested in a better implementation
        controllerIndex = steamTrackedObj.index.GetHashCode();
        if (SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            if (syndromeMode) {
                pointGenerator.ClosePlate();
            } else {
                pointGenerator.ClosePincer();
            }
        }
        if (SteamVR_Controller.Input(controllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
            if (syndromeMode) {
                pointGenerator.OpenPlate();
            } else {
                pointGenerator.OpenPincer();
            }
        }
        if (SteamVR_Controller.Input(controllerIndex).GetHairTriggerDown()) {
            setSelectionMode(true);
        }
        if (SteamVR_Controller.Input(controllerIndex).GetHairTriggerUp()) {
            setSelectionMode(false);
        }
        if (selectionMode) {
            Ray selectionRay = new Ray(transform.position, transform.forward);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(selectionRay);
            float closestDistance = 999999999.9f;
            hoveredSubject = null;
            foreach (RaycastHit hit in hits) {
                // TODO: there has to be a better way...
                if (hit.distance < closestDistance) {
                    FocalPointVR_ManipulationHandler manipHandler = hit.collider.gameObject.GetComponent<FocalPointVR_ManipulationHandler>();
                    if (manipHandler != null && manipHandler.isSelectable) {
                        closestDistance = hit.distance;
                        hoveredSubject = hit.collider.gameObject;
                    }
                }
            }
        }
        if (SteamVR_Controller.Input(controllerIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x > 0.999f) {
            if (ixdManager.subject != hoveredSubject && hoveredSubject != null) {
                ixdManager.changeSubject(hoveredSubject);
            }
        }
    }

    void setSelectionMode(bool state) {
        displayPathSprite(state ? 4 : -99);
        selectionMode = state;
    }

    void displayPathSprite(int index) {
        for (int i = 0; i < pathSpriteComponents.Length; i++) {
            pathSpriteComponents[i].SetActive(index == i || index + 1 == i);
        }
    }
}
