using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.DataObjects;

public class MapSpawner : MonoBehaviour
{
	public TextAsset layoutText;
	public GameObject btnPrefab;
	public GameObject btnStarPrefab;

	public GameObject shadowStarPrefab;
	public GameObject shadowPrefab;

	public Vector2 spacing;
	public SceneLoader loader;
	public Transform shadowParent;
	
	LayoutData _layoutData;
	List <LevelButton> _levelBtns = new List<LevelButton>();

	Dictionary<int, LevelButton> _btnLookup = new Dictionary<int, LevelButton>();

	public LevelButton GetLastPlayedLevel()
	{
		int levelNum = SaveData.GetCurrentLevel();

		foreach (LevelButton btn in _levelBtns)
		{
			if (btn.gameObject.activeSelf == false)
				continue;
			
			if (btn.levelIdx == levelNum)
				return btn;
		}

		return _levelBtns[0];
	}

	public LevelButton GetButtonAt(int x, int y)
	{
		int coord = IntCoord(x, y);

		if (_btnLookup.ContainsKey(coord))
		{
			LevelButton result = _btnLookup[coord];
			
			if (result.gameObject.activeSelf)
				return result;
		}

		return null;
	}
	
	// Use this for initialization
	void Start ()
	{
		_InitLayoutData();
		_InitVisibility();
	}

	void _InitLayoutData()
	{
		_layoutData = LayoutData.CreateFromJSON(layoutText.text);
		var layoutInts = _layoutData.GetLevelLayouts();
		var states = SaveData.GetLevelStates();

		foreach(var intList in layoutInts)
		{
			var levelIdx = intList[0];
			
			var position = new Vector2(intList[2], intList[1]);
			GameObject newObj = null;
			GameObject shadowObj = null;
		

			if(levelIdx < states.Count && states[levelIdx])
			{
				shadowObj = Instantiate(shadowStarPrefab, shadowParent);
				newObj = Instantiate(btnStarPrefab, transform);
			}
			else
			{
				shadowObj = Instantiate(shadowPrefab, shadowParent);
				newObj = Instantiate(btnPrefab, transform);
			}

			var btn = newObj.GetComponent<LevelButton>();
			
			btn.SetLevelIdx(levelIdx);
			btn.HookBtnPress(loader);
			btn.SetPosition(position);
			btn.shadowRect = shadowObj.GetComponent<RectTransform>();

			_levelBtns.Add(btn);

			int coord = IntCoord(intList[2], intList[1]);
			_btnLookup[coord] = btn;
		}
	}

	private int IntCoord(int x, int y) => x * 100 + y;

	void _InitVisibility()
	{
		foreach(var lvl in _levelBtns)
			lvl.CheckIfShouldAppear(_levelBtns);
	}
}
