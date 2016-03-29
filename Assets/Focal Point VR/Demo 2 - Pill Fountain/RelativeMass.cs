using UnityEngine;
using System.Collections;

public class RelativeMass : MonoBehaviour {
    private Collider col;
    private Rigidbody rbody;
    private FocalPointVR_ManipulationHandler manipHandler;

    void Start () {
        col = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        manipHandler = GetComponent<FocalPointVR_ManipulationHandler>();
    }

    void Update () {
        if (manipHandler.isCaptured) {
            rbody.mass = col.bounds.extents.magnitude;
        }
    }
}
