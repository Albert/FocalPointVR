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
            rbody.freezeRotation = true;
            if (Mathf.Abs(transform.localScale.x) > 5.0f) {
                transform.localScale = new Vector3 (5, 5, 5);
            }
            Destroy (toDestroy.gameObject);
            toDestroy = Instantiate (wallPrefab, initialWallLocation, Quaternion.identity) as GameObject;
        } else {
            rbody.freezeRotation = false;
        }
    }
}
