using UnityEngine;
using System.Collections;

public class FocalPointRenderer : MonoBehaviour {
	public Color hoverColor;
	public Color activeColor;
	public bool isActive = false;
	public Sprite optionalLaserSprite; // used for when origin is set and there's a connecting laser beam
	private Vector3 origin;
	private Renderer meshRenderer;

	void Start () {
		meshRenderer = GetComponent<Renderer> ();
	}
	
	void Update () {
		if (isActive) {
			meshRenderer.material.color = activeColor;
		} else {
			meshRenderer.material.color = hoverColor;
		}
	}
}
