using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectAllGrassText : MonoBehaviour
{
	public ActorManager actorManager;
	public TutorialScreen tutScreen;
	public Text actualText1;
	public Text actualText2;

	public string eatGrassText;
	public string getToGoalText;

	int _appliedStage = -1;
	int _currentLevel = 0;
	float _alpha = 0;
	Color32 _originalColor;
	CanvasGroup _grp;

	float _text1Alpha = 1f;
	float _text2Alpha = 0f;

	Dictionary<int, Vector2[]> _goalPositions = new Dictionary<int, Vector2[]>
	{
		{1, new Vector2[] { new Vector2(3, 0), new Vector2(3, -3) }},
		{2, new Vector2[] { new Vector2(1, -2), new Vector2(4, -2), new Vector2(4, 0), new Vector2(3, -1) }},
		{3, new Vector2[] { new Vector2(2, 0), new Vector2(2, -2), new Vector2(4, -1) }},
		{4, new Vector2[] { new Vector2(3, -1), new Vector2(2, -2), new Vector2(0, -3) }},
		{5, new Vector2[] { new Vector2(0, -3), new Vector2(2, 0), new Vector2(3, -1) }} 
	};


	// Use this for initialization
	void Start ()
	{
		_currentLevel = SaveData.GetCurrentLevel();
		if(!_goalPositions.ContainsKey(_currentLevel))
			Destroy(gameObject);

		_grp = GetComponent<CanvasGroup>();
		_originalColor = actualText1.color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		CalculateStage();
		IncrementAlpha();
		HandleTextAlpha();

		_grp.alpha = _alpha;
	}

	void CalculateStage()
	{
		// 0: nothing eaten, 1: all eaten, 2: reached goal
		var visitedAllTiles = true;
		var visitedGoal = false;

		var touchedTiles = actorManager.farmer.GetAllTouchedTiles();
		var requiredTiles = _goalPositions[_currentLevel];

		for(int i=0; i<requiredTiles.Length-1; i++)
		{
			if(!touchedTiles.Contains(requiredTiles[i]))
				visitedAllTiles = false;	
		}

		if(touchedTiles.Contains(requiredTiles[requiredTiles.Length-1]))
			visitedGoal = true;
		
		if(visitedAllTiles && visitedGoal)
			_appliedStage = 2;
		else if(visitedAllTiles)
			_appliedStage = 1;
		else
			_appliedStage = 0;
	}

	void HandleTextAlpha()
	{
		if(_appliedStage == 0)
			_text1Alpha += Time.deltaTime * 4;
		else
			_text1Alpha -= Time.deltaTime * 4;
		
		if(_appliedStage == 1)
			_text2Alpha += Time.deltaTime * 4;
		else
			_text2Alpha -= Time.deltaTime * 4;

		_text1Alpha = Mathf.Clamp(_text1Alpha, -1, 1);
		_text2Alpha = Mathf.Clamp(_text2Alpha, -1, 1);

		if(_alpha < 0.01f)
			return;

		var firstAlpha = (byte)(Mathf.Clamp(_text1Alpha, 0, 1) * 255 * ((Mathf.Sin(Time.time * 3f) / 10f) + 0.9f));
		var secondAlpha = (byte)(Mathf.Clamp(_text2Alpha, 0, 1) * 255 * ((Mathf.Sin(Time.time * 3f) / 10f) + 0.9f));
		
		actualText1.color = new Color32(_originalColor.r, _originalColor.g, _originalColor.b, firstAlpha);
		actualText2.color = new Color32(_originalColor.r, _originalColor.g, _originalColor.b, secondAlpha);
	}

	void IncrementAlpha()
	{
		var active = !tutScreen.onScreen && (_appliedStage == 0 || _appliedStage == 1);
		
		if(active)
			_alpha += Time.deltaTime * 3.5f;
		else
			_alpha -= Time.deltaTime * 3.5f;

		_alpha = Mathf.Clamp(_alpha, 0, 1);
	}
}
