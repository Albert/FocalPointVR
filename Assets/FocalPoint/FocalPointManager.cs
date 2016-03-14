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
		applyTranslation ();
		applyScale ();
		applyRotation ();

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
				if (renderer.isActive) {
					focalPoints.Add (renderer.gameObject);  // TODO is there a way to do this faster w/ addRange?
				}
			}
		}
	}

	void applyTranslation() {
		if (focalPoints.Count > 0) {
			Vector3 averagePoint = new Vector3();
			foreach (GameObject point in focalPoints) {
				averagePoint += point.transform.position;
			}
			averagePoint /= focalPoints.Count;
			transform.position = averagePoint;
		}
	}

	void applyScale() {
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
	}

	void applyRotation() {
		if (focalPoints.Count == 2) {
			Quaternion currentRotation = Quaternion.LookRotation (focalPoints [0].transform.position - focalPoints [1].transform.position);
			transform.rotation = currentRotation;
		} else if (focalPoints.Count == 3) {
			// TODO Talk to a proper comp sci person about a better way to do this...
			Vector3 directionToLook = focalPoints [1].transform.position - focalPoints [0].transform.position;
			transform.LookAt (transform.position + directionToLook);
			Vector3 referenceDirection = focalPoints [2].transform.position - focalPoints [0].transform.position;
			Vector3 projectedReference = Vector3.ProjectOnPlane (referenceDirection, transform.forward);
			float levelingAngle = Vector3.Angle (transform.right, projectedReference);
			float sign = Vector3.Cross(transform.InverseTransformDirection(transform.right), projectedReference).z < 0 ? -1 : 1;
			levelingAngle *= sign;
			transform.Rotate (transform.InverseTransformDirection(transform.forward), levelingAngle);
		} else if (focalPoints.Count > 3) {
			// TODO I have no idea how to solve for this
		}
	}
}