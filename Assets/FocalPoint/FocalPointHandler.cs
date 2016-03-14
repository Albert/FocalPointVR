using UnityEngine;
using System.Collections;

public class FocalPointHandler : MonoBehaviour {
	public bool lockTranslation = false;
	public bool lockRotation = false;
	public bool lockScale = false;
	public bool translationInertia = false;
	public bool rotationInertia = false;
	public bool scaleInertia = false;
	private Rigidbody rBody;

	// pretty sure this doesn't need to do anything except save user preferences...
	void Start () {
		rBody = GetComponent<Rigidbody> ();
	}

	void Update () {
	}
}