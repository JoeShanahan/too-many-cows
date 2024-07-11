using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTicker : MonoBehaviour
{
	public RectTransform[] btns;
	public GameObject tickPrefab;

	// Use this for initialization
	void Start ()
	{
		var states = SaveData.GetLevelStates();
		if(states.Count == 0)
			return;

		var cap = Mathf.Min(states.Count-1, btns.Length);

		for(int i=0; i<cap; i++)
		{
			if(states[i+1])
				CreateTick(btns[i]);
		}
	}
	
	void CreateTick(RectTransform rt)
	{
		Instantiate(tickPrefab, rt);
	}
}
