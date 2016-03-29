using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRAdapter : MonoBehaviour {
    public GameObject handPrefab;
    private GameObject[] controllers = new GameObject[2];
    private FocalPointVR_InteractionManager ixdManager;

    void Start() {
        controllers[0] = transform.parent.FindChild("Controller (left)").gameObject;
        controllers[1] = transform.parent.FindChild("Controller (right)").gameObject;
        ixdManager = GetComponent<FocalPointVR_InteractionManager>();
        foreach (GameObject controller in controllers) {
            FocalPointVR_SteamVRControllerAdapter adapter = controller.AddComponent<FocalPointVR_SteamVRControllerAdapter>();
            adapter.ixdManager = ixdManager;
            GameObject newHand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            newHand.transform.parent = controller.transform;
        }
    }
}
