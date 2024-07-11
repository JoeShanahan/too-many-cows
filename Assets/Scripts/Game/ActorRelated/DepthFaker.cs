using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the shadow away from the color to make it
// look like it's moving up and down
public class DepthFaker : MonoBehaviour
{
	Vector3 startPos;
	Vector3 shadowStartPos;

	public float currentHeight = 0f;
	public bool bouncing = false;
	float offsetMul = 0.03f;
	float phase = 0f;
	Vector3 diagonal = new Vector3(1, -1, 0).normalized;
	
	float percentPhase = 1.0f;

	void Start()
	{
		startPos = transform.GetChild(0).localPosition;
		shadowStartPos = transform.GetChild(1).localPosition;
	}

	// Update is called once per frame
	void Update ()
	{
		HandlePercentPhase();
		DoBounce();
	}

	void HandlePercentPhase()
	{	
		phase += Time.deltaTime * 2.2f;

		if(bouncing)
			percentPhase -= Time.deltaTime * 10;
		else
			percentPhase += Time.deltaTime * 10;
		
		percentPhase = Mathf.Clamp(percentPhase, 0, 1);

		if(percentPhase == 0)
			phase = -1.57f;
	}

	void DoBounce()
	{
		var currentPhase = (Mathf.Sin(phase) + 1) / 2;
		var percentBounce = 1 - percentPhase;

		var positionColorPhase = currentPhase * offsetMul;
		var positionShadowPhase = currentPhase * offsetMul;

		var positionColorBounce = currentHeight * 0.1f;
		var positionShadowBounce = currentHeight * offsetMul;

		var positionColor = (positionColorPhase * percentPhase) + (positionColorBounce * percentBounce);
		var positionShadow = (positionShadowPhase * percentPhase) + (positionShadowBounce * percentBounce);

		transform.GetChild(0).localPosition = startPos + (Vector3.up * positionColor);
		transform.GetChild(1).localPosition = shadowStartPos + (diagonal * positionShadow);
		
	}
}
