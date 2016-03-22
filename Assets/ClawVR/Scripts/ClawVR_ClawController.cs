using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_ClawController : ClawVR_GrasperController {
    private GameObject[] arms;

    public override void Start () {
		base.Start ();
        arms = new GameObject[2];
        arms[0] = transform.Find("arm 1").gameObject;
        arms[1] = transform.Find("arm 2").gameObject;
		pincerDifference = new Vector3 (0, 0, -0.20f);
    }

    public override void CloseClaw() {
		if (!isClosed) {
			arms[0].transform.localRotation = Quaternion.Euler(0, -43.0f, 0);
			arms[1].transform.localRotation = Quaternion.Euler(0, +43.0f, 0);
		}
		base.CloseClaw ();
    }

	public override void OpenClaw() {
		// TODO: consider making it lerp out
		if (isClosed) {
            arms[0].transform.localRotation = Quaternion.identity;
            arms[1].transform.localRotation = Quaternion.identity;
        }
		base.OpenClaw ();
    }
}