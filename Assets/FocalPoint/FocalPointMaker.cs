using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FocalPointMaker : MonoBehaviour {
	public GameObject focalPointTemplate;
	private FakeControllerMover controllerMover;
	private List<GameObject> focalPoints;
	private bool sameAsLastFrame = true;

	void Start () {
		controllerMover = GetComponent<FakeControllerMover>();
		focalPoints = new List<GameObject> ();
	}

	void Update () {
		sameAsLastFrame = true;
//		translator ();
//		scaler ();
//		twoPointRotator();
		threePointRotator();
	}

	void addPoint(Vector3 pointLocation) {
		sameAsLastFrame = false;
		GameObject newPoint = Instantiate(focalPointTemplate, pointLocation, Quaternion.identity) as GameObject;
		newPoint.transform.SetParent (transform, false);
		focalPoints.Add (newPoint);
	}

	void removePoints() {
		sameAsLastFrame = false;
		foreach (GameObject focalPoint in focalPoints) {
			Destroy (focalPoint);
		}
		focalPoints.Clear ();
	}

	void translator() {
		if (controllerMover.clickIsDown) {
			addPoint (Vector3.up * 2.0f);
		} else if (controllerMover.clickIsUp) {
			removePoints ();
		}
	}

	void scaler() {
		Vector3 point0 = new Vector3 (Mathf.Sin (Time.time), 0.0f, 0.0f);
		Vector3 point1 = new Vector3 (-2.0f, 0.0f, 0.0f);
		if (controllerMover.clickIsDown) {
			addPoint (point0);
			addPoint (point1);
		} else if (controllerMover.clickIsUp) {
			removePoints ();
		} else if (controllerMover.clickIsActive) {
			focalPoints [0].transform.localPosition = point0;
			focalPoints [1].transform.localPosition = point1;
		}
	}

	void twoPointRotator() {
		if (controllerMover.clickIsDown) {
			addPoint (new Vector3 (0.0f, 3.0f, 0.0f));
			addPoint (new Vector3 (-2.0f, 0.0f, 0.0f));
		} else if (controllerMover.clickIsUp) {
			removePoints ();
		}
	}

	void threePointRotator() {
		if (controllerMover.clickIsDown) {
			addPoint (new Vector3 (-2.0f, 0.0f, 0.0f));
			addPoint (new Vector3 (-2.0f, -3.0f, 0.0f));
			addPoint (new Vector3 (0.0f, -3.0f, 0.0f));
		} else if (controllerMover.clickIsUp) {
			removePoints ();
		}
	}

	// TODO make this smarter
	public bool isChangedThisFrame() {
		return !sameAsLastFrame;
	}
}