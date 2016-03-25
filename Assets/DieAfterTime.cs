using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {
	public float timeToLive = 15.0f;
	private float initializationTime;

	// Use this for initialization
	void Start () {
		initializationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > initializationTime + timeToLive) {
			Destroy (this.gameObject);
		}
	}
}
