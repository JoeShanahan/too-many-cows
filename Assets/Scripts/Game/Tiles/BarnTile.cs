using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnTile : DynamicTile
{
	public bool startBarn;
	SkinnedMeshRenderer mesh;
	float barnOpenPercent = 0;
	public bool open = false;

	void Start()
	{
		mesh = GetComponent<SkinnedMeshRenderer>();
	}
	
	void Update ()
	{
		if(open)
			barnOpenPercent += Time.deltaTime * 2;
		else
			barnOpenPercent -= Time.deltaTime * 2;

		barnOpenPercent = Mathf.Clamp(barnOpenPercent, 0, 1);

		mesh.SetBlendShapeWeight(0, barnOpenPercent*100);
	}

	public override void UpdateVisualState()
	{	
	
		if(startBarn)
			open = cowsLeft < totalCows;
	}
}
