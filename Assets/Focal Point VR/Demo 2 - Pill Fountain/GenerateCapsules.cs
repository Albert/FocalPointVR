using UnityEngine;
using System.Collections;

public class GenerateCapsules : MonoBehaviour {
    public GameObject prefab;
    public float delay = 0.1f;

    private float lastGeneration = 0;

    void Update () {
        if (Time.time > lastGeneration + delay) {
            Quaternion startRotation = Quaternion.Euler (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
            GameObject newObj = Instantiate(prefab, Vector3.zero, startRotation) as GameObject;
            newObj.transform.SetParent(transform, false);
            lastGeneration = Time.time;
        }
    }
}
