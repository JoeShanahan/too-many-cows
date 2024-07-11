using UnityEngine;
using System.Collections.Generic;

namespace TooManyCows.DataObjects
{
	[System.Serializable]
	public class LayoutData
	{
		public string[] levelLayout;
		
		public static LayoutData CreateFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<LayoutData>(jsonString);
		}

		public string ObjToJson()
		{
			return JsonUtility.ToJson(this);
		}

		public int[][] GetLevelLayouts()
		{
			var tmpList = new List<int[]>();

			foreach(var intString in levelLayout)
			{
				var splitString = intString.Split(',');
				if(splitString.Length != 3)
				{
					Debug.LogWarning("LevelLayout data is wrong length! (" + splitString.Length.ToString() + ")");
					continue;
				}

				var intList = new int[3];
				var failed = false;

				for(int i=0; i<3; i++)
				{
					if(!int.TryParse(splitString[i], out intList[i]))
						failed = true;
				}

				if(failed)
				{
					Debug.LogWarning("LevelLayout data is wrong format! (" + intString + ")");
					continue;
				}

				tmpList.Add(intList);
			}

			return tmpList.ToArray();
		}
	}
}