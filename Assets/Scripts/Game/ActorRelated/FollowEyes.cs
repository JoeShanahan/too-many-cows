using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEyes : MonoBehaviour
{
	public float distMultiplier = 0.14f;
	public Vector2 lookDirection = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(lookDirection.magnitude == 0)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10);
		}
		else
		{
			var lookDirV3 = new Vector3(lookDirection.x, 0, -lookDirection.y);
			var tgtPos = lookDirV3.normalized * distMultiplier;
			transform.localPosition = Vector3.Lerp(transform.localPosition, tgtPos, Time.deltaTime * 10);
		}
	}
}
