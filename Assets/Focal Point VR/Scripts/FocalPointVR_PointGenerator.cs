using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FocalPointVR_PointGenerator : MonoBehaviour {
    private Vector3 pincerDifference = new Vector3(0, 0, 0.09f);

    public bool isClosed { get; set; }
    public bool samePointsAsLastFrame { get; set; }

    private GameObject[] handSprites = new GameObject[3];

    public void Start() {
        samePointsAsLastFrame = true;
        handSprites[0] = transform.Find("open").gameObject;
        handSprites[1] = transform.Find("closed").gameObject;
        handSprites[2] = transform.Find("pointer").gameObject;
        if (transform.parent.gameObject.name == "Controller (left)") {
            foreach (GameObject handSprite in handSprites) {
                handSprite.transform.Rotate(Vector3.up, 180);
            }
        }
        displayHandSprite(0);
    }

    void Update() {
        if (isClosed) {
            displayHandSprite(1);
        } else {
            displayHandSprite(0);
        }
    }

    public void ClosePincer() {
        if (!isClosed) {
            addFocalPoint(Vector3.zero);
            samePointsAsLastFrame = false;
            isClosed = true;
        }
    }

    public void ClosePlate() {
        if (!isClosed) {
            addFocalPoint(Vector3.zero);
            addFocalPoint(Vector3.forward);
            addFocalPoint(Vector3.up);
            samePointsAsLastFrame = false;

            isClosed = true;
        }
    }

    public void OpenPincer() {
        if (isClosed) {
            removeAllFocalPoints();
            samePointsAsLastFrame = false;
            isClosed = false;
        }
    }

    public void OpenPlate() {
        OpenPincer();
    }

    public void addFocalPoint(Vector3 localLocation) {
        GameObject focalPoint = new GameObject("FocalPoint");
        focalPoint.transform.localPosition = pincerDifference + localLocation;
        focalPoint.transform.SetParent(transform, false);
    }

    public void removeAllFocalPoints() {
        foreach (Transform child in transform) {
            if (child.name == "FocalPoint") {
                Destroy(child.gameObject);
            }
        }
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
}
