using UnityEngine;
using System.Collections;

public class FocalPointVR_SteamVRAdapterDemo3and4 : MonoBehaviour {
    public GameObject handPrefab;
    private GameObject[] controllers = new GameObject[2];
    private FocalPointVR_InteractionManagerDemo3and4 ixdManager;
    public GameObject pathSpritePrefab;
    public float laserDistanceMax = 7;
    private FocalPointVR_SteamVRControllerAdapterDemo3and4[] adapters = new FocalPointVR_SteamVRControllerAdapterDemo3and4[2];

    void Start() {
        controllers[0] = transform.parent.FindChild("Controller (left)").gameObject;
        controllers[1] = transform.parent.FindChild("Controller (right)").gameObject;
        ixdManager = GetComponent<FocalPointVR_InteractionManagerDemo3and4>();
        for (int i = 0; i < controllers.Length; i++) {
            adapters[i] = controllers[i].AddComponent<FocalPointVR_SteamVRControllerAdapterDemo3and4>();
            adapters[i].ixdManager = ixdManager;
            adapters[i].laserDistanceMax = laserDistanceMax;
            GameObject newHand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            newHand.transform.parent = controllers[i].transform;
            GameObject pathSpriteContainer = Instantiate(pathSpritePrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, -90, 0))) as GameObject;
            pathSpriteContainer.transform.parent = controllers[i].transform;
            // TODO: there's got to be a better way to do this...
            adapters[i].pathSpriteComponents = new GameObject[] {
                controllers[i].transform.Find("Path Sprite(Clone)/LaserMode1").gameObject,
                controllers[i].transform.Find("Path Sprite(Clone)/LaserMode2").gameObject,
                controllers[i].transform.Find("Path Sprite(Clone)/Telescope1").gameObject,
                controllers[i].transform.Find("Path Sprite(Clone)/Telescope2").gameObject,
                controllers[i].transform.Find("Path Sprite(Clone)/Selection1").gameObject,
                controllers[i].transform.Find("Path Sprite(Clone)/Selection2").gameObject
            };
        }
    }

    void Update() {
        foreach (FocalPointVR_SteamVRControllerAdapterDemo3and4 adapter in adapters) {
            adapter.laserDistanceMax = laserDistanceMax;
        }
    }
}
