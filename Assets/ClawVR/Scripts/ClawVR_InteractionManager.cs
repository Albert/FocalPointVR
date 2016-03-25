using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_InteractionManager : MonoBehaviour {
    public bool canSelectAnyObject = true;
	public GameObject subject { set; get; }
    private Transform subjectPreviousParent;
	public List<ClawVR_HandController> clawControllers { get; set; }
	private List<GameObject> focalPoints = new List<GameObject>();
	private bool anyPointChangedThisFrame;
	private Vector3[] oldPointsForRotation = new Vector3[2];
	private Light[] selectionHighlighters;
	private ClawVR_ManipulationHandler subjectManipHandler;
    public Plane laserPlane { get; set; }
    public bool laserMode { get; set; }
    public bool selectionMode { get; set; }

    void Start () {
		selectionMode = true;
        clawControllers = new List<ClawVR_HandController>();
        selectionHighlighters = GetComponentsInChildren<Light>();
        //this.changeSubject(GameObject.Find("Cube"));
    }

    void Update () {
    }

    void LateUpdate() {
        if (subject != null) {
            updatePointsIfNecessary();
		    if (anyPointChangedThisFrame && subject.transform.parent != transform && subjectManipHandler != null) {
                subjectManipHandler.capture();
		    }
            if (anyPointChangedThisFrame && subject.transform.parent == transform) {
                // kick out permanently for released objects, temporarily for if focal points just rearrange
                subject.transform.SetParent(subjectPreviousParent);
            }
            applyTranslation ();
		    applyScale ();
		    applyRotation ();
		    if (anyPointChangedThisFrame) {
			    if (focalPoints.Count == 0) {
                    if (subjectManipHandler != null) {
                        subjectManipHandler.release ();
				    }
			    } else {
				    subjectPreviousParent = subject.transform.parent;
				    subject.transform.SetParent (transform);
			    }
		    } else if (subjectManipHandler) {
                subjectManipHandler.lastFramePosition = subjectManipHandler.thisFramePosition;
                subjectManipHandler.lastFrameRotation = subjectManipHandler.thisFrameRotation;
                subjectManipHandler.lastFrameScale    = subjectManipHandler.thisFrameScale;

                subjectManipHandler.thisFramePosition = subject.transform.position;
                subjectManipHandler.thisFrameRotation = subject.transform.rotation;
                subjectManipHandler.thisFrameScale    = subject.transform.lossyScale;
            }
            runHighlighter();
        }
    }

    void updatePointsIfNecessary() {
		anyPointChangedThisFrame = false;
        foreach (ClawVR_HandController clawController in clawControllers) {
			if (clawController.CheckIfPointsHaveUpdated()) {
				anyPointChangedThisFrame = true;
				updatePoints ();
                break; // only need to do this foreach loop once if points are updated
            }
        }
    }

	void updatePoints() {
		focalPoints.Clear();
		int controllersThatAreClosed = 0;
		foreach (ClawVR_HandController clawController in clawControllers) {
			if (clawController.isClosed) {
				controllersThatAreClosed++;
			}
		}
		foreach (ClawVR_HandController clawController in clawControllers) {
			if (controllersThatAreClosed == 1) {
				foreach (Transform child in clawController.gameObject.transform) {
					if (child.name == "ClawFocalPoint") {
						focalPoints.Add(child.gameObject);
					}
				}
			} else if (controllersThatAreClosed > 1) {
                // only add 1st focal point for each controller
                Transform foundTransform = clawController.gameObject.transform.Find("ClawFocalPoint");
                if (foundTransform != null) {
                    focalPoints.Add(foundTransform.gameObject);
                }
            }
		}
	}

	void applyTranslation () {
		if (subjectManipHandler != null && subjectManipHandler.lockTranslation) {
			transform.position = subject.transform.position;
			return;
		}
		if (focalPoints.Count > 0) {
				Vector3 averagePoint = new Vector3();
				foreach (GameObject point in focalPoints) {
					averagePoint += point.transform.position;
				}
				averagePoint /= focalPoints.Count;
				transform.position = averagePoint;
//			}
		}
	}

	void applyScale () {
		if (subjectManipHandler != null && subjectManipHandler.lockScale) {
			return;
		}
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

	void applyRotation () {
		if (subjectManipHandler != null && subjectManipHandler.lockRotation) {
			return;
		}
		if (focalPoints.Count == 2) {
			Vector3 direction1 = oldPointsForRotation[0] - oldPointsForRotation[1];
			Vector3 direction2 = focalPoints [0].transform.position - focalPoints [1].transform.position;
			Vector3 cross = Vector3.Cross (direction1, direction2);
            float amountToRot = Vector3.Angle(direction1, direction2);
            transform.Rotate(cross.normalized, amountToRot, Space.World);

            oldPointsForRotation [0] = focalPoints [0].transform.position;
			oldPointsForRotation [1] = focalPoints [1].transform.position;
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

	void runHighlighter() {
		if (subject) {
			Collider col = subject.GetComponent<Collider> ();
			float mag = col.bounds.extents.magnitude;
            for (int i = 0; i < selectionHighlighters.Length; i++) {
                float portion = (float)i / (float)selectionHighlighters.Length;
                float animLength = 3.0f;
                float modifiedTime = Time.time + animLength * portion;
                float t = (modifiedTime % animLength);
                selectionHighlighters[i].range = Mathf.Lerp(mag / 2.0f, mag * 6.0f, t / animLength);
                selectionHighlighters[i].intensity = Mathf.Lerp(2, 0, t / animLength);
                selectionHighlighters[i].gameObject.transform.position = subject.transform.position;
            }
        }
	}

    public void changeSubject(GameObject newSubject) {
        if (subject != null) {
            subject.transform.SetParent(subjectPreviousParent);
        }
        subject = newSubject;
        if (subject == null) {
            foreach (Light l in selectionHighlighters) {
                l.gameObject.SetActive(false);
            }
            subjectManipHandler = null;
			selectionMode = true;
        } else {
            foreach (Light l in selectionHighlighters) {
                l.gameObject.SetActive(true);
            }
            subjectManipHandler = subject.GetComponent<ClawVR_ManipulationHandler>();
			if (subjectManipHandler != null && subjectManipHandler.reactsTo == ClawVR_ManipulationHandler.grabOrLaser.laser) {
				laserMode = true;
			} else {
				laserMode = false;
			}
        }
    }

    public void registerClaw(ClawVR_HandController c) {
		clawControllers.Add (c);
        c.ixdManager = this;
	}

    public void setLaserPlane(Vector3 worldNormal, Vector3 worldPoint) {
        laserPlane.SetNormalAndPosition(worldNormal.normalized, worldPoint);
    }
}