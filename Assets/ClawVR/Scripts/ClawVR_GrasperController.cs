using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_GrasperController : MonoBehaviour {
	protected Vector3 pincerDifference = new Vector3(0, 0, -0.20f);

	public bool isClosed { get; set; }
	protected bool laserMode;
	private bool samePointsAsLastFrame = true;

	public GameObject pathSpritePrefab;
	private GameObject pathSpriteContainer;
	private GameObject[] pathSpriteComponents;
	public ClawVR_InteractionManager ixdManager {get; set;}

	public virtual void Start () {
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

	void Update () {
		if (laserMode) {
			Ray laserRay = new Ray(transform.parent.position, transform.parent.transform.forward);
			RaycastHit hit;
			if (isClosed) {
				Collider c = ixdManager.subject.GetComponent<Collider> ();
				if (c.Raycast (laserRay, out hit, 99999)) {
					transform.position = hit.point - transform.TransformVector(pincerDifference);
				} else {
					transform.localPosition = Vector3.zero;
				}
			} else {
				if (Physics.Raycast(laserRay, out hit)) {
					transform.position = hit.point - transform.TransformVector(pincerDifference);
				} else {
					transform.localPosition = Vector3.zero;
				}
			}
		}
	}

	public virtual void CloseClaw() {
		if (!isClosed) {
			if (laserMode) {
				GameObject focalPoint = new GameObject("ClawFocalPoint");
				focalPoint.transform.localPosition = pincerDifference;
				focalPoint.transform.SetParent(transform, false);
			} else {
				Vector3[] points = { Vector3.zero, Vector3.forward, Vector3.up };
				foreach (Vector3 point in points) {
					GameObject focalPoint = new GameObject("ClawFocalPoint");
					focalPoint.transform.localPosition = pincerDifference + point;
					focalPoint.transform.SetParent (transform, false);
				}
			}
			samePointsAsLastFrame = false;
			pathSpriteContainer.transform.localScale = new Vector3 (1.0f, 6.0f, 6.0f);

			isClosed = true;
		}
	}

	public virtual void OpenClaw() {
		// TODO: consider making it lerp out
		if (isClosed) {
			foreach(Transform child in transform) {
				if (child.name == "ClawFocalPoint") {
					Destroy(child.gameObject);
				}
			}
			samePointsAsLastFrame = false;
			pathSpriteContainer.transform.localScale = Vector3.one;

			isClosed = false;
		}
	}

	public virtual void DeployLaser() {
		laserMode = true;
		pathSpriteContainer.transform.localScale = new Vector3(9999.9f, 1, 1);
		pathSpriteComponents[0].SetActive(true);
		pathSpriteComponents[1].SetActive(true);
		pathSpriteComponents[2].SetActive(false);
		pathSpriteComponents[3].SetActive(false);
	}

	public virtual void DeployTelescope() {
		laserMode = false;
		pathSpriteContainer.transform.localScale = new Vector3(transform.localPosition.z, 1, 1);
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
}