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
		translator ();
		//scaler ();
	}

	void translator() {
		if (controllerMover.clickIsDown) {
			sameAsLastFrame = false;
			GameObject point1 = Instantiate(focalPointTemplate, Vector3.up * 2.0f, Quaternion.identity) as GameObject;
			point1.transform.SetParent (transform, false);
			focalPoints.Add (point1);
		} else if (controllerMover.clickIsUp) {
			sameAsLastFrame = false;
			foreach (GameObject focalPoint in focalPoints) {
				Destroy (focalPoint);
			}
			focalPoints.Clear ();
		}
	}

	void scaler() {
		if (controllerMover.clickIsDown) {
			sameAsLastFrame = false;
			Vector3 position1 = new Vector3 (Mathf.Sin (Time.time), 0.0f, 0.0f);
			GameObject point1 = Instantiate (focalPointTemplate, position1, Quaternion.identity) as GameObject;
			point1.transform.SetParent (transform, false);
			focalPoints.Add (point1);

			Vector3 position2 = new Vector3 (-2.0f, 0.0f, 0.0f);
			GameObject point2 = Instantiate (focalPointTemplate, position2, Quaternion.identity) as GameObject;
			point2.transform.SetParent (transform, false);
			focalPoints.Add (point2);

		} else if (controllerMover.clickIsUp) {
			sameAsLastFrame = false;
			foreach (GameObject focalPoint in focalPoints) {
				Destroy (focalPoint);
			}
			focalPoints.Clear ();
		} else if (controllerMover.clickIsActive) {
			Vector3 position1 = new Vector3 (Mathf.Sin (Time.time), 0.0f, 0.0f);
			focalPoints [0].transform.localPosition = position1;
			Vector3 position2 = new Vector3 (-2.0f, 0.0f, 0.0f);
			focalPoints [1].transform.localPosition = position2;
		}
	}

	// TODO make this smarter
	public bool isChangedThisFrame() {
		return !sameAsLastFrame;
	}
}