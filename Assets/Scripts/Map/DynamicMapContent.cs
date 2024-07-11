using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMapContent : MonoBehaviour
{
	public int levelRequirement;

	// Use this for initialization
	void Start ()
	{
		var completedLevels = SaveData.GetLevelStates();

		if(completedLevels.Count > levelRequirement && completedLevels[levelRequirement])
			Destroy(gameObject);
	}
}
