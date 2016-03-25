using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_HandController : MonoBehaviour {
    private Vector3 pincerDifference = new Vector3(0, 0, 0.09f);

    public bool isClosed { get; set; }
    public bool samePointsAsLastFrame { get; set; }

    public GameObject pathSpritePrefab;
    private GameObject pathSpriteContainer;
    private GameObject[] pathSpriteComponents;
    public ClawVR_InteractionManager ixdManager { get; set; }
    public GameObject hoveredSubject { get; set; }

    private GameObject[] handSprites = new GameObject[3];

    public void Start() {
        samePointsAsLastFrame = true;
        handSprites[0] = transform.Find("open").gameObject;
        handSprites[1] = transform.Find("closed").gameObject;
        handSprites[2] = transform.Find("pointer").gameObject;
        if (transform.parent.gameObject.name == "Controller (left)") {
            foreach(GameObject handSprite in handSprites) {
                handSprite.transform.Rotate(Vector3.up, 180);
            }
        }
        displayHandSprite(0);
        pathSpriteContainer = Instantiate(pathSpritePrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
        pathSpriteContainer.transform.parent = transform.parent;
        // TODO: there's got to be a better way to do this...
        pathSpriteComponents = new GameObject[] {
            transform.parent.Find("Path Sprite(Clone)/Laser1").gameObject,
            transform.parent.Find("Path Sprite(Clone)/Laser2").gameObject,
            transform.parent.Find("Path Sprite(Clone)/Telescope1").gameObject,
            transform.parent.Find("Path Sprite(Clone)/Telescope2").gameObject
        };
    }

    void Update() {
        reassignHoveredObject();
        showCorrectSprites();
        if (ixdManager.laserMode) {
            Ray laserRay = new Ray(transform.parent.position, transform.parent.transform.forward);
            bool otherHandIsClosed = (otherHandController() != null && otherHandController().isClosed);
            if (isClosed) {
                if (!otherHandIsClosed) {
                    setLaserCollider(laserRay);
                }
                positionClawOnLaserCollider(laserRay);
            } else {
                if (otherHandIsClosed) {
                    positionClawOnLaserCollider(laserRay);
                } else {
                    setLaserCollider(laserRay);
                }
                pathSpriteContainer.transform.localScale = new Vector3(99999, 1.0f, 1.0f);
            }
        }
    }

    void reassignHoveredObject() {
        Ray selectionRay = new Ray(transform.parent.position, transform.parent.transform.forward);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(selectionRay);
        float closestDistance = 999999999.9f;
        hoveredSubject = null;
        foreach (RaycastHit hit in hits) {
            // TODO: there has to be a better way...
            if (hit.distance < closestDistance) {
                ClawVR_ManipulationHandler manipHandler = hit.collider.gameObject.GetComponent<ClawVR_ManipulationHandler>();
                if ((manipHandler != null && manipHandler.isSelectable) || (ixdManager.canSelectAnyObject && manipHandler == null)) {
                    if (hit.collider.gameObject.name != "Laser Collider") {
                        closestDistance = hit.distance;
                        hoveredSubject = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    void showCorrectSprites() {
        if (ixdManager.laserMode) {
            displayHandSprite(2);
            if (isClosed) {
                pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 6.0f, 6.0f);
            } else {
                pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 1.0f, 1.0f);
            }
        } else {
            if (isClosed) {
                displayHandSprite(1);
            } else {
                displayHandSprite(0);
            }
        }
    }

    private void positionClawOnLaserCollider(Ray laserRay) {
        float distance;
        if (ixdManager.laserPlane.Raycast(laserRay, out distance)) {
            transform.position = laserRay.GetPoint(distance) - transform.TransformVector(pincerDifference);
        }
    }

    private void setLaserCollider(Ray laserRay) {
        RaycastHit hit;
        if (ixdManager.subject != null) {
            Collider s = ixdManager.subject.GetComponent<Collider>();
            if (s.Raycast(laserRay, out hit, 99999)) {
                transform.position = hit.point - transform.TransformVector(pincerDifference);
                Plane p = new Plane();
                p.SetNormalAndPosition(hit.normal, hit.point);
                ixdManager.laserPlane = p;
            } else {
                transform.localPosition = Vector3.zero;
            }
        } else {
            transform.localPosition = Vector3.zero;
        }
    }

    public void CloseClaw() {
        if (!isClosed) {
            if (ixdManager.laserMode) {
                if (transform.localPosition != Vector3.zero) {
                    GameObject focalPoint = new GameObject("ClawFocalPoint");
                    focalPoint.transform.localPosition = pincerDifference;
                    focalPoint.transform.SetParent(transform, false);
                }
            } else {
                Vector3[] points = { Vector3.zero, Vector3.forward, Vector3.up };
                foreach (Vector3 point in points) {
                    GameObject focalPoint = new GameObject("ClawFocalPoint");
                    focalPoint.transform.localPosition = pincerDifference + point;
                    focalPoint.transform.SetParent(transform, false);
                }
            }
            samePointsAsLastFrame = false;
            pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 6.0f, 6.0f);

            isClosed = true;
        }
    }

    public void OpenClaw() {
        if (isClosed) {
            otherHandController().samePointsAsLastFrame = false;
            foreach (Transform child in transform) {
                if (child.name == "ClawFocalPoint") {
                    Destroy(child.gameObject);
                }
            }
            samePointsAsLastFrame = false;
            pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 1, 1);

            isClosed = false;
        }
    }

    public void DeployLaser() {
        ixdManager.laserMode = true;
        pathSpriteContainer.transform.localScale = new Vector3(9999.9f, 1, 1);
        pathSpriteComponents[0].SetActive(true);
        pathSpriteComponents[1].SetActive(true);
        pathSpriteComponents[2].SetActive(false);
        pathSpriteComponents[3].SetActive(false);
    }

    public void DeployTelescope() {
        ixdManager.laserMode = false;
        if (isClosed) {
            pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 6.0f, 6.0f);
        } else {
            pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 1, 1);
        }
        pathSpriteComponents[0].SetActive(false);
        pathSpriteComponents[1].SetActive(false);
        pathSpriteComponents[2].SetActive(true);
        pathSpriteComponents[3].SetActive(true);
    }

    public void TelescopeRelatively(float amount) {
        transform.localPosition = transform.localPosition + new Vector3(0, 0, amount);
        if (transform.localPosition.z < 0) {
            transform.localPosition = Vector3.zero;
        }
        DeployTelescope();
    }

    public void TelescopeAbsolutely(float amount) {
        transform.localPosition = new Vector3(0, 0, amount);
        if (transform.localPosition.z < 0) {
            transform.localPosition = Vector3.zero;
        }
        DeployTelescope();
    }

    public float GetScopeDistance() {
        return transform.localPosition.z;
    }

    public bool CheckIfPointsHaveUpdated() {
        bool returnValue = !samePointsAsLastFrame;
        samePointsAsLastFrame = true;
        return returnValue;
    }

    void displayHandSprite(int index) {
        for (int i = 0; i < handSprites.Length; i++) {
            handSprites[i].SetActive(index == i);
        }
    }

    public ClawVR_HandController otherHandController() {
        foreach (ClawVR_HandController controller in ixdManager.clawControllers) {
            if (this.GetInstanceID() != controller.GetInstanceID()) {
                return controller;
            }
        }
        return null;
    }
}