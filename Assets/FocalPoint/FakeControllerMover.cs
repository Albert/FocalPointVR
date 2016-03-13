using UnityEngine;
using System.Collections;

public class FakeControllerMover : MonoBehaviour {

	public bool clickIsActive;
	public bool clickIsDown;
	public bool clickIsUp;

	void Start () {

	}

	void Update () {
		debugRotate ();
		debugMove ();
		clickIsActive = Input.GetKey ("space");
		clickIsDown = Input.GetKeyDown ("space");
		clickIsUp = Input.GetKeyUp ("space");
	}

	void debugMove() {
		transform.position = new Vector3 (Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0.0f);
	}

	void debugRotate() {
		transform.Rotate (Vector3.right, Time.deltaTime * 50.0f);
	}
}