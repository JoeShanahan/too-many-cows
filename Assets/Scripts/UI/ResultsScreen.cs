using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
	public Text leftText;
	public Text rightText;

	// Use this for initialization
	void Start ()
	{
		var trackingString = SaveData.TrackingDataString;
		leftText.text = rightText.text = "";
		


		if(trackingString.Length == 0)
			return;
			
		var trackingData = TrackingData.CreateFromJSON(trackingString);
		
		for(int i=0; i<48; i++)
		{
			if(i >= trackingData.levelStats.Length)
				break;

			if(i<24)
				leftText.text += trackingData.levelStats[i].Replace(",", " , ") + "\n";
			else
				rightText.text += trackingData.levelStats[i].Replace(",", " , ") + "\n";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
