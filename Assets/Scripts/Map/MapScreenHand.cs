using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenHand : MonoBehaviour
{
	float fluctuation = 0.05f;
	Vector3 _basePosition;

	// Use this for initialization
	void Start ()
	{
		_basePosition = transform.localPosition;
		var lvlStates = SaveData.GetLevelStates();

		if(lvlStates.Count > 1 && lvlStates[1])
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		_MakeHandWiggle();	
	}

	void _MakeHandWiggle()
	{
		var xpos = _basePosition.x + (Mathf.Sin(Time.time * 3.5f) * fluctuation);
		transform.localPosition = new Vector3(xpos, _basePosition.y, _basePosition.z);
	}
}
