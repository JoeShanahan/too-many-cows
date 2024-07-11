using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSheep : MonoBehaviour
{
	float phase = 0.0f;
	float waitTime = 5f;
	int hopTime = 2;
	public float moveSpeed = 3f;

	Transform sheepObj;

	Vector3 upDir;

	// Use this for initialization
	void Start ()
	{	

		hopTime = Random.Range(2, 6);
		waitTime = Random.Range(2f, 5f);

		phase = Random.Range(0, hopTime + waitTime);

		upDir = transform.up;
		sheepObj = transform.GetChild(0);
		transform.RotateAround(transform.up, Random.Range(0f, 7f));
	}

	public void IncrementPhase()
	{
		var resetTime = waitTime + (hopTime);
		phase += Time.deltaTime;

		if(phase > resetTime)
		{
			phase = 0;
			hopTime = Random.Range(1, 3);
			waitTime = Random.Range(2f, 4f);
		}
	}

	public void BounceAround()
	{
		if(phase >= hopTime)
			return;

		transform.position += transform.forward * moveSpeed * transform.localScale.z * Time.deltaTime;
		sheepObj.localPosition = new Vector3(0, 0.25f * Mathf.Abs(Mathf.Sin(phase * 3.1415f * 4)), 0);
	}

	public void ClampPosition()
	{
		// -1.1, 3.35
		// -4.2, 5.25
		var lpos = transform.localPosition;
		var x = Mathf.Clamp(lpos.x, -4.2f, -1.1f);
		var y = 3.4f;
		var z = Mathf.Clamp(lpos.z, 3.35f, 5.25f);

		transform.localPosition = new Vector3(x, y, z);
	}

	public void SeperateFromOtherSheep(MapSheep[] otherSheeps)
	{
		foreach(var other in otherSheeps)
		{
			if(other == this)
				continue;

			var diffVec = (transform.position - other.transform.position);
			if(diffVec.magnitude > 0.45f)
				continue;
			
			transform.position += diffVec * 0.05f;
			other.transform.position -= diffVec * 0.05f;
		}
	}

	public void SpinIfOutsideBounds()
	{
		if(phase >= hopTime)
			return;

		// -1.12, 3.34
		// -4.21, 5.26

		var lpos = transform.localPosition;
		var faceDir = transform.forward;
		var parentTrans = transform.parent;

		if(transform.localPosition.z < 3.35f)
			transform.rotation = Quaternion.LookRotation(Vector3.Reflect(faceDir, parentTrans.forward), upDir);

		if(transform.localPosition.z > 5.25f)
			transform.rotation = Quaternion.LookRotation(Vector3.Reflect(faceDir, parentTrans.forward), upDir);

		if(transform.localPosition.x < -4.2f)
			transform.rotation = Quaternion.LookRotation(Vector3.Reflect(faceDir, parentTrans.right), upDir);

		if(transform.localPosition.x > -1.1f)
			transform.rotation = Quaternion.LookRotation(Vector3.Reflect(faceDir, parentTrans.right), upDir);

		ClampPosition();
	}
}
