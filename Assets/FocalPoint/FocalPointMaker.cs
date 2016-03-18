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
//		startTranslator ();
//		startScaler ();
		startTwoPointRotator();
//		startThreePointRotator();
	}

	void Update () {
		sameAsLastFrame = true;
//		runScaler ();
		if (controllerMover.clickIsDown) {
			setPointActivationTo (true);
		} else if (controllerMover.clickIsUp) {
			setPointActivationTo (false);
		}
	}

	void startTranslator() {
		addPoint (Vector3.up * 2.0f);
	}

	void startScaler() {
		addPoint (new Vector3 (Mathf.Sin (Time.time), 0.0f, 0.0f));
		addPoint (new Vector3 (-2.0f, 0.0f, 0.0f));
	}

	void runScaler() {
		focalPoints [0].transform.localPosition = new Vector3 (Mathf.Sin (Time.time), 0.0f, 0.0f);
		focalPoints [1].transform.localPosition = new Vector3 (-2.0f, 0.0f, 0.0f);
	}

	void startTwoPointRotator() {
		addPoint (new Vector3 (0.0f, 3.0f, 0.0f));
		addPoint (new Vector3 (-2.0f, 0.0f, 0.0f));
	}

	void startThreePointRotator() {
		addPoint (new Vector3 (-2.0f, 0.0f, 0.0f));
		addPoint (new Vector3 (-2.0f, -3.0f, 0.0f));
		addPoint (new Vector3 (0.0f, -3.0f, 0.0f));
	}

	void addPoint(Vector3 pointLocation) {
		GameObject newPoint = Instantiate(focalPointTemplate, pointLocation, Quaternion.identity) as GameObject;
		newPoint.transform.SetParent (transform, false);
		focalPoints.Add (newPoint);
	}

	void setPointActivationTo(bool state) {
		sameAsLastFrame = false;
		foreach (GameObject focalPoint in focalPoints) {
			FocalPointRenderer renderer = focalPoint.GetComponent<FocalPointRenderer> ();
			renderer.isActive = state;
		}
	}

	void destroyAllPoints() {
		sameAsLastFrame = false;
		foreach (GameObject focalPoint in focalPoints) {
			Destroy (focalPoint);
		}
		focalPoints.Clear ();
	}

	public bool isChangedThisFrame() {
		return !sameAsLastFrame;
	}
}