using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FocalPointManager : MonoBehaviour {
	public GameObject subject;
	private Transform previousSubjectParent;
	private FocalPointMaker[] makers;
	private List<GameObject> focalPoints;
	private bool anyPointChangedThisFrame = false;

	void Start () {
		makers = FindObjectsOfType(typeof(FocalPointMaker)) as FocalPointMaker[];
		focalPoints = new List<GameObject> ();
	}
	
	void Update () {
	}

	void LateUpdate() {
		updatePointsIfNecessary ();

		// translation
		if (focalPoints.Count > 0) {
			Vector3 averagePoint = new Vector3();
			foreach (GameObject point in focalPoints) {
				averagePoint += point.transform.position;
			}
			averagePoint /= focalPoints.Count;
			transform.position = averagePoint;
		}

		// scale
		if (focalPoints.Count > 1) {
			float averageDistance = 0;
			foreach (GameObject pointA in focalPoints) {
				foreach (GameObject pointB in focalPoints) {
					if (pointA != pointB) {
						averageDistance += (pointA.transform.position - pointB.transform.position).magnitude;
					}
				}
			}
			averageDistance /= focalPoints.Count;
			transform.localScale = new Vector3 (averageDistance, averageDistance, averageDistance);
		}

		// rotation
		if (focalPoints.Count == 2) {
			Quaternion currentRotation = Quaternion.LookRotation (focalPoints [0].transform.position - focalPoints [1].transform.position);
			transform.rotation = currentRotation;
		} else if (focalPoints.Count == 3) {
			// TODO This is a guess... needs to be tested out properly
			Plane myPlane = new Plane (focalPoints [0].transform.position, focalPoints [1].transform.position, focalPoints [2].transform.position);
			transform.LookAt (myPlane.normal);
		} else if (focalPoints.Count > 3) {
			// TODO I have no idea how to solve for this
		}

		if (anyPointChangedThisFrame) {
			if (subject.transform.parent != transform) {
				previousSubjectParent = subject.transform.parent;
				subject.transform.SetParent (transform);
			}
			if (focalPoints.Count == 0) {
				subject.transform.SetParent (previousSubjectParent);
			}
		}
	}

	void updatePointsIfNecessary() {
		anyPointChangedThisFrame = false;
		foreach (FocalPointMaker maker in makers) {
			if (maker.isChangedThisFrame ()) {
				anyPointChangedThisFrame = true;
				updatePoints ();
				break;
			}
		}
	}

	void updatePoints() {
		focalPoints.Clear ();
		foreach (FocalPointMaker maker in makers) {
			FocalPointRenderer[] renderers = maker.transform.GetComponentsInChildren<FocalPointRenderer> ();
			foreach (FocalPointRenderer renderer in renderers) {
				focalPoints.Add (renderer.gameObject);  // TODO is there a way to do this faster w/ addRange?
			}
		}
	}
}
