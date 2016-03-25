using UnityEngine;
using System.Collections;

public class GenerateCapsules : MonoBehaviour {
	public GameObject prefab;
	public float delay = 0.1f;

	private float lastGeneration = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastGeneration + delay) {
			Quaternion startRotation = Quaternion.Euler (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
			Instantiate(prefab, Vector3.zero, startRotation);
			lastGeneration = Time.time;
		}
	}
}
