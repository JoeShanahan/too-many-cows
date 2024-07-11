using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum LerpDirection {Right, Up, Down};

public class MenuLerper : MonoBehaviour
{
	public GameObject defaultSelection;
	public int positionMultiplier;
	public LerpDirection lerpDir;

	public MainMenuMode[] zeroStates;
	public MainMenuMode[] negativeStates;
	MainMenuMode _prevMenuMode = MainMenuMode.TopLevel;

	RectTransform _rt;
	Vector2 _offScreenVec;
	float _speed = 8f;

	void Start()
	{
		_rt = GetComponent<RectTransform>();

		if(lerpDir == LerpDirection.Right)
			_offScreenVec = new Vector2(1500, 0);
		else if(lerpDir == LerpDirection.Up)
			_offScreenVec = new Vector2(0, 1500);
		else if(lerpDir == LerpDirection.Down)
			_offScreenVec = new Vector2(0, -1500);
	}	


	// Update is called once per frame
	void Update ()
	{
		_rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, _offScreenVec * positionMultiplier, Time.deltaTime * _speed);
	}

	public void OnMenuEnable()
	{
		if (gameObject.activeSelf == false)
			return;

		if (defaultSelection != null)
		{
			EventSystem.current.SetSelectedGameObject(defaultSelection);	
		}
	}

	public void SetMenuMode(MainMenuMode menuMode)
	{
		if(menuMode == _prevMenuMode)
			return;

		_prevMenuMode = menuMode;

		foreach(var m in zeroStates)
		{
			if(menuMode == m)
			{
				OnMenuEnable();
				positionMultiplier = 0;
				return;
			}
		}

		foreach(var m in negativeStates)
		{
			if(menuMode == m)
			{
				positionMultiplier = -1;
				return;
			}
		}

		positionMultiplier = 1;
	}
}
