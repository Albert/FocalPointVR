using UnityEngine;
using System.Collections;

public class ClawVR_SteamVRAdapter : MonoBehaviour {
	public GameObject clawPrefab;

	void Start () {
		GameObject[] controllers = new GameObject[2];
		controllers[0] = transform.parent.FindChild("Controller (left)").gameObject;
		controllers[1] = transform.parent.FindChild("Controller (right)").gameObject;

		ClawVR_InteractionManager ixdManager = GetComponent<ClawVR_InteractionManager> ();

		foreach (GameObject controller in controllers) {
			ClawVR_ViveControllerAdapter adapter = controller.AddComponent<ClawVR_ViveControllerAdapter>();
			adapter.ixdManager = ixdManager;

			GameObject newClaw = Instantiate(clawPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			newClaw.transform.parent = controller.transform;
		}
	}
	
	void Update () {
	}
}