using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRAdapterDemo2 : MonoBehaviour {
    public bool enablePinnedMode;
    public GameObject handPrefab;
    private GameObject[] controllers = new GameObject[2];
    private FocalPointVR_InteractionManager ixdManager;
    public GameObject pathSpritePrefab;

    void Start() {
        controllers[0] = transform.parent.FindChild("Controller (left)").gameObject;
        controllers[1] = transform.parent.FindChild("Controller (right)").gameObject;
        ixdManager = GetComponent<FocalPointVR_InteractionManager>();
        foreach (GameObject controller in controllers) {
            FocalPointVR_SteamVRControllerAdapterDemo2 adapter = controller.AddComponent<FocalPointVR_SteamVRControllerAdapterDemo2>();
            adapter.ixdManager = ixdManager;
            GameObject newHand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            newHand.transform.parent = controller.transform;
            GameObject pathSpriteContainer = Instantiate(pathSpritePrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
            pathSpriteContainer.transform.parent = controller.transform;
            // TODO: there's got to be a better way to do this...
            adapter.pathSpriteComponents = new GameObject[] {
                controller.transform.Find("Path Sprite(Clone)/LaserMode1").gameObject,
                controller.transform.Find("Path Sprite(Clone)/LaserMode2").gameObject,
                controller.transform.Find("Path Sprite(Clone)/Telescope1").gameObject,
                controller.transform.Find("Path Sprite(Clone)/Telescope2").gameObject,
                controller.transform.Find("Path Sprite(Clone)/Selection1").gameObject,
                controller.transform.Find("Path Sprite(Clone)/Selection2").gameObject
            };
        }
    }

    void Update() {
        foreach (GameObject controller in controllers) {
            FocalPointVR_SteamVRControllerAdapterDemo2 adapter = controller.GetComponent<FocalPointVR_SteamVRControllerAdapterDemo2>();
            adapter.syndromeMode = enablePinnedMode;
        }
    }
}
