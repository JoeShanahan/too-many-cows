using UnityEngine;
using System.Collections.Generic;

namespace TooManyCows.DataObjects
{
    [System.Serializable]
    public class LevelData
    {
        public int[] levelSize = new int[2];
        public int moveCount;
        public int[] startPos = new int[2];
        public string[] tiles = new string[0];
        public int numCows;
        public string[] tractors = new string[0];
        public string[] fences = new string[0];
        public string[] sheep = new string[0];

        public bool isTransposed = false;
        
        public static LevelData CreateFromJSON(string jsonString)
        {
            LevelData outLevel = JsonUtility.FromJson<LevelData>(jsonString);

            if (outLevel.levelSize[1] > outLevel.levelSize[0])
                outLevel.Transpose();

            return outLevel;
        }

        public void Transpose()
        {
            levelSize = new int[] { levelSize[1], levelSize[0] };
            startPos = new int[] { startPos[1], startPos[0] };
            isTransposed = true;
        }

        public string ObjToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public TileData[] TileData
        {
            get
            {
                var takenTiles = new List<string>();
                var tileList = new List<TileData>();

                foreach (var tileStr in tiles)
                {
                    var splitData = tileStr.Split(',');
                    if (splitData.Length != 3)
                    {
                        Debug.LogWarning("Tile format is incorrect! (" + tileStr + ")");
                        continue;
                    }

                    if (isTransposed)
                        splitData = new string[] { splitData[0], splitData[2], splitData[1] };

                    int xPos = 0;
                    int yPos = 0;

                    var xSuccess = int.TryParse(splitData[1], out xPos);
                    var ySuccess = int.TryParse(splitData[2], out yPos);

                    if (!xSuccess || !ySuccess)
                    {
                        Debug.LogWarning("Tile position is incorrect! (" + tileStr + ")");
                        continue;
                    }

                    tileList.Add(new TileData(splitData[0], xPos, yPos));
                    takenTiles.Add(splitData[1] + "-" + splitData[2]);
                }

                for (int x = 0; x < levelSize[0]; x++)
                {
                    for (int y = 0; y < levelSize[1]; y++)
                    {
                        var positionString = x.ToString() + "-" + y.ToString();
                        if (takenTiles.Contains(positionString))
                            continue;

                        tileList.Add(new TileData(x, y));
                    }
                }

                return tileList.ToArray();
            }
        }

        public FenceData[] FenceData
        {
            get
            {
                var fenceList = new List<FenceData>();

                foreach (var fenceStr in fences)
                {
                    var splitData = fenceStr.Split(',');
                    if (splitData.Length != 4)
                    {
                        Debug.LogWarning("Fence format is incorrect! (" + fenceStr + ")");
                        continue;
                    }

                    if (isTransposed)
                        splitData = new string[] { splitData[1], splitData[0], splitData[3], splitData[2] };

                    var intList = new int[4];
                    var failed = false;

                    for(int i=0; i<4; i++)
                    {
                        if(!int.TryParse(splitData[i], out intList[i]))
                            failed = true;
                    }

                    if (failed)
                    {
                        Debug.LogWarning("Fence position is incorrect! (" + fenceStr + ")");
                        continue;
                    }

                    fenceList.Add(new FenceData(intList[0], intList[1], intList[2], intList[3]));
                }

                return fenceList.ToArray();            
            }
        }

        public TractorData[] TractorData
        {
            get
            {
                var tractorList = new List<TractorData>();
                
                foreach(var tractorString in tractors)
                {
                    var splitData = tractorString.Split(',');
                    if (splitData.Length != 3)
                    {
                        Debug.LogWarning("Tractor format is incorrect! (" + tractorString + ")");
                        continue;
                    }

                    if (isTransposed)
                        splitData = new string[] { splitData[1], splitData[0], splitData[2] };

                    var intList = new int[2];
                    var failed = false;

                    for(int i=0; i<2; i++)
                    {
                        if(!int.TryParse(splitData[i], out intList[i]))
                            failed = true;
                    }

                    if (failed)
                    {
                        Debug.LogWarning("Tractor position is incorrect! (" + tractorString + ")");
                        continue;
                    }

                    tractorList.Add(new TractorData(intList[0], intList[1],splitData[2], isTransposed));
                }

                return tractorList.ToArray();
            }
        }

        public SheepData[] SheepData
        {
            get
            {
                var sheepList = new List<SheepData>();
                
                foreach(var sheepString in sheep)
                {
                    var splitData = sheepString.Split(',');
                    if (splitData.Length != 4)
                    {
                        Debug.LogWarning("Sheep format is incorrect! (" + sheepString + ")");
                        continue;
                    }

                    if (isTransposed)
                        splitData = new string[] { splitData[0], splitData[2], splitData[1], splitData[3] };

                    var intList = new int[3];
                    var failed = false;

                    for(int i=0; i<3; i++)
                    {
                        if(!int.TryParse(splitData[i], out intList[i]))
                            failed = true;
                    }

                    if (failed)
                    {
                        Debug.LogWarning("Sheep position/length is incorrect! (" + sheepString + ")");
                        continue;
                    }

                    sheepList.Add(new SheepData(intList[0], intList[1], intList[2], splitData[3], isTransposed));
                }

                return sheepList.ToArray();
            }
        }
    }
}