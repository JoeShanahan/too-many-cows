using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorVibrate : MonoBehaviour {

	Vector3 startPos;
	float vibeSpeed = 32f;
	float vibeAmount = 0.004f;
	public float multiplier = 1;
	public Vector3 vibeDirection = Vector3.up;

	// Use this for initialization
	void Start () {
		startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		var phase = Mathf.Sin(Time.time*vibeSpeed);
		transform.localPosition = startPos + vibeDirection * phase * vibeAmount * multiplier;
	}
}
