using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {
	public float timeToLive = 15.0f;
	private float initializationTime;
    private ClawVR_ManipulationHandler manipHandler;

	// Use this for initialization
	void Start () {
		initializationTime = Time.time;
        manipHandler = GetComponent<ClawVR_ManipulationHandler>();
    }

    // Update is called once per frame
    void Update () {
        if (manipHandler.isCaptured) {
            initializationTime = Time.time;
        }
		if (Time.time > initializationTime + timeToLive) {
			Destroy (this.gameObject);
		}
	}
}
