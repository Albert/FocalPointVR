using UnityEngine;
using System.Collections;

public class Demo1RestartWhenDropped : MonoBehaviour {
    public GameObject wallPrefab;
    private GameObject toDestroy;
    private Vector3 initialWallLocation;
    private Vector3 initialLocation;
    private Rigidbody rbody;

    void Start () {
        toDestroy = GameObject.Find("wall");
        initialWallLocation = toDestroy.transform.position;
        initialLocation = transform.position;
        rbody = GetComponent<Rigidbody>();
    }

    void Update () {
        if (transform.position.y < -50.0f) {
            transform.position = initialLocation;
            rbody.velocity = Vector3.zero;
            Destroy(toDestroy.gameObject);
            toDestroy = Instantiate(wallPrefab, initialWallLocation, Quaternion.identity) as GameObject;
        }
    }
}
