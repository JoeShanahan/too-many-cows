using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CompletionScreen : MonoBehaviour
{
	public Color highlightColor;

	public Text timeTakenText;
	public Text movesMadeText;
	public Text rewindsMadeText;

	int _totalTime = 0;
	int _totalMoves = 0;
	int _totalRewinds = 0;

	// Use this for initialization
	void Start ()
	{
		var hcol = ColorUtility.ToHtmlStringRGB(highlightColor);

		_GetPlayStats();

		var hours = _totalTime / 3600;
		var minutes = (_totalTime / 60) % 60;
		var seconds = _totalTime % 60;

		var timeString = string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);

		timeTakenText.text = string.Format("Time Taken: <color=#{0}>{1}</color>", hcol, timeString);
		movesMadeText.text = string.Format("Total Moves: <color=#{0:n0}>{1}</color>", hcol, _totalMoves);
		rewindsMadeText.text = string.Format("Total Rewinds: <color=#{0:n0}>{1}</color>", hcol, _totalRewinds);
	}
	
	void _GetPlayStats()
	{
		var trackingData = TrackingData.CreateFromJSON(SaveData.TrackingDataString);
		var _trackingDict = trackingData.GetLevelStats();

		foreach(var intlist in _trackingDict.Values)
		{
			_totalTime += intlist[3];
			_totalMoves += intlist[1];
			_totalRewinds += intlist[2];
		}
	}

}
