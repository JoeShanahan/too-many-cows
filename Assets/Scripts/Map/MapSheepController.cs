using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

public class MapSheepController : MonoBehaviour
{
	public MapSheep[] sheepList;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{	
		// Move the sheep
		foreach(var sheep in sheepList)
		{
			sheep.IncrementPhase();
			sheep.BounceAround();
		}

		// Correct the position if outside bounds
		foreach(var sheep in sheepList)
		{
			sheep.SpinIfOutsideBounds();
			sheep.SeperateFromOtherSheep(sheepList);
		}

		// Correct the position AGAIN if outside bounds
		foreach(var sheep in sheepList)
		{
			sheep.ClampPosition();
		}
	}

}
