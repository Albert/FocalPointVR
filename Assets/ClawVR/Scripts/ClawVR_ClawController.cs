using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_ClawController : MonoBehaviour {
    private GameObject[] arms;
	private Vector3 pincerDifference = new Vector3(0, 0, -0.15f);

	public bool isClosed { get; set; }
    private bool laserMode;
	private bool samePointsAsLastFrame = true;

    public Sprite TelescopeSprite;
    public Sprite LaserSprite;

    void Start () {
        arms = new GameObject[2];
        arms[0] = transform.Find("arm 1").gameObject;
        arms[1] = transform.Find("arm 2").gameObject;
    }

    void Update () {
        if (laserMode) {
            Ray laserRay = new Ray(transform.parent.position, transform.parent.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(laserRay, out hit)) {
                transform.position = hit.point - transform.TransformVector(pincerDifference);
            } else {
                transform.localPosition = Vector3.zero;
            }
        }
    }

    public void CloseClaw() {
		if (!isClosed) {
			Vector3[] points = { Vector3.zero, Vector3.forward, Vector3.up };
			foreach (Vector3 point in points) {
		        GameObject focalPoint = new GameObject("ClawFocalPoint");
				focalPoint.transform.localPosition = pincerDifference + point;
				focalPoint.transform.SetParent (transform, false);
			}
	        samePointsAsLastFrame = false;

            arms[0].transform.localRotation = Quaternion.Euler(0, -43.0f, 0);
            arms[1].transform.localRotation = Quaternion.Euler(0, +43.0f, 0);
            isClosed = true;
        }
    }

    public void OpenClaw() {
		// TODO: consider making it lerp out
		if (isClosed) {
	        foreach(Transform child in transform) {
	            if (child.name == "ClawFocalPoint") {
	                Destroy(child.gameObject);
	            }
	        }
	        samePointsAsLastFrame = false;

            arms[0].transform.localRotation = Quaternion.identity;
            arms[1].transform.localRotation = Quaternion.identity;
            isClosed = false;
        }
    }

    public void DeployLaser() {
        laserMode = true;
    }

    public void TelescopeRelatively(float amount) {
        laserMode = false;
        transform.localPosition = transform.localPosition + new Vector3(0, 0, amount);
        if (transform.localPosition.z < 0) {
            transform.localPosition = Vector3.zero;
        }
    }

    public void TelescopeAbsolutely(float amount) {
        laserMode = false;
        transform.localPosition = new Vector3(0, 0, amount);
        if (transform.localPosition.z < 0) {
            transform.localPosition = Vector3.zero;
        }
    }

    public float GetScopeDistance() {
        return transform.localPosition.z;
    }

	public bool CheckIfPointsHaveUpdated() {
		bool returnValue = !samePointsAsLastFrame;
		samePointsAsLastFrame = true;
		return returnValue;
	}
}