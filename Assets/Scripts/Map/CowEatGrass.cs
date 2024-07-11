using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowEatGrass : MonoBehaviour
{
	public float phase = 0;
	public float resetTime = 0;
	Quaternion startRot;
	float speed = 1;

	// Use this for initialization
	void Start ()
	{
		speed = Random.Range(0.8f, 1.3f);
		startRot = transform.rotation;
		resetTime = Random.Range(12f, 20f);
		phase = Random.Range(0, resetTime);
	}
	
	// Update is called once per frame
	void Update ()
	{	

		phase += Time.deltaTime * 1.5f * speed;

		if(phase >= resetTime)
		{
			phase -= resetTime;
			resetTime = Random.Range(12f, 20f);
			speed = Random.Range(0.8f, 1.3f);
		}

		var rotPercent = _GetRotPercent();
		transform.rotation = startRot;
		transform.RotateAround(transform.right, rotPercent * 0.3f);
	}

	float _GetRotPercent()
	{
		if(phase < 1)
			return phase;

		else if(phase < 4)
			return 1 + ((Mathf.Cos((phase-1) * 3.1415f * 1.333f) - 1)/5);

		else if(phase < 5)
			return 5 - phase;

		return 0;
	}
}
