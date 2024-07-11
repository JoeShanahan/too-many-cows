using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TooManyCows.Audio;

public class SceneLoader : MonoBehaviour
{
	public string sceneToLoad;
	public SkinnedMeshRenderer peepholeMesh;
	public bool startsOpen = false;
	public bool showAds = false;
	public TutorialScreen tutorialScreen;

	float _loadPercent = 1f;
	bool _currentlyLoading = false;
	bool _finishedTransition = false;
	
	// Use this for initialization
	void Start ()
	{	
		if(startsOpen)
			_loadPercent = 0;
		else
			AudioManager.PlaySound(SoundEffect.TransitionIn);
	}
	
	// Update is called once per frame
	void Update ()
	{
		SetCurrentPercent();
		SetTransitionShapeKey();
		HandleLoading();
		HandleFinishTransition();
	}
	
	void HandleFinishTransition()
	{
		if(_loadPercent != 0)
			return;

		if(_finishedTransition == false)
		{	
			_finishedTransition = true;
			var currentLevel = SaveData.GetCurrentLevel();
			
			if(tutorialScreen && currentLevel < 6)
			{
				var tutorialStates = SaveData.TutorialStates;

				if(tutorialStates[currentLevel-1] == false)
					tutorialScreen.BtnPressOpenTutorial();
			}
		}
	}

	void HandleLoading()
	{
		if(!_currentlyLoading)
			return;

		if(_loadPercent != 1)
			return;

		PlayerTracker.LevelRestartedOrQuit();
		PlayerTracker.SaveCurrentLevel();
		SceneManager.LoadScene(sceneToLoad);
	}

	void SetCurrentPercent()
	{
		if(_currentlyLoading)
			_loadPercent += Time.deltaTime * 1.5f;
		else
			_loadPercent -= Time.deltaTime * 1.5f;

		_loadPercent = Mathf.Clamp(_loadPercent, 0, 1);
	}

	void SetTransitionShapeKey()
	{

		peepholeMesh.SetBlendShapeWeight(0, SmoothFunctions.EaseOut(_loadPercent*1.2f) * 100);
	}

	public void LoadScene(string sceneName)
	{
		if(_currentlyLoading)
			return;
			
		sceneToLoad = sceneName;
		_currentlyLoading = true;
		AudioManager.PlaySound(SoundEffect.TransitionOut);
	}

	public void LoadLevel(int levelIdx)
	{	
		SaveData.SetCurrentLevel(levelIdx);
		LoadScene("GameScene");
	}

	public void SetPeepholeSize()
	{
		var size = Camera.main.orthographicSize * (4f/3f);
		peepholeMesh.transform.localScale = Vector3.one * size;
	}
}
