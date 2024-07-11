using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TrackingData
{
	public string[] levelStats = new string[0];
	
	public static TrackingData CreateFromJSON(string jsonString)
    {
		if(jsonString.Length == 0)
			return new TrackingData();

        return JsonUtility.FromJson<TrackingData>(jsonString);
    }

    public string ObjToJson()
    {
        return JsonUtility.ToJson(this);
    }

	public void DictionaryToStrings(Dictionary<int, int[]> inpDict)
	{
		var tmpList = new List<string>();

		foreach(var i in inpDict.Values)
		{
			var levelString = string.Format("{0},{1},{2},{3},{4},{5}", i[0], i[1], i[2], i[3], i[4], i[5]);
			tmpList.Add(levelString);
		}

		levelStats = tmpList.ToArray();
	}

	public Dictionary<int, int[]> GetLevelStats()
	{
		/* levelIdx,moveCount,rewindCount,totalSeconds,solved */
		var tmpList = new Dictionary<int, int[]>();

		foreach(var intString in levelStats)
		{
			var splitString = intString.Split(',');
			if(splitString.Length != 6)
			{
				Debug.LogWarning("LevelStats data is wrong length! (" + splitString.Length.ToString() + ")");
				continue;
			}

			var intList = new int[6];
			var failed = false;

			for(int i=0; i<6; i++)
			{
				if(!int.TryParse(splitString[i], out intList[i]))
					failed = true;
			}

			if(failed)
			{
				Debug.LogWarning("LevelStats data is wrong format! (" + intString + ")");
				continue;
			}

			tmpList[intList[0]] = intList;
		}

		return tmpList;
	}
}
