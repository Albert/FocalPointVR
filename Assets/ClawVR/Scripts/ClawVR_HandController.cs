using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClawVR_HandController : ClawVR_GrasperController {
	private GameObject[] handSprites = new GameObject[3];

	public override void Start () {
		handSprites [0] = transform.Find ("open-hand").gameObject;
		handSprites [1] = transform.Find ("closed-hand").gameObject;
		handSprites [2] = transform.Find ("pointer").gameObject;
		pincerDifference = new Vector3 (0, 0, -0.09f);
		base.Start ();
	}

	public override void CloseClaw() {
		if (!isClosed) {
			displayHandSprite (1);
		}
		base.CloseClaw ();
	}

	public override void OpenClaw() {
		if (isClosed) {
			if (laserMode) {
				displayHandSprite (2);
			} else {
				displayHandSprite (0);
			}
		}
		base.OpenClaw ();
	}

	public override void DeployTelescope() {
		if (isClosed) {
			displayHandSprite (1);
		} else {
			displayHandSprite (0);
		}
		base.DeployTelescope ();
	}

	public override void DeployLaser() {
		displayHandSprite (2);
		base.DeployLaser ();
	}

	void displayHandSprite(int index) {
		for (int i = 0; i < handSprites.Length; i++) {
			handSprites [i].SetActive (index == i);
		}
	}
}