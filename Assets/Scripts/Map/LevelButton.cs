using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooManyCows.Audio;

public class LevelButton : MonoBehaviour
{
	public Vector2 levelSpacing;
	public int levelIdx;
	public bool isCompleted;
	public Text btnText;
	public Vector2 position;
	public RectTransform shadowRect;

	RectTransform _rt;
	Button _btn;
	AdvancedButton _advBtn;
	SceneLoader _loader;
	bool _shadowDown = true;

	public Vector2 anchoredPosition => _rt.anchoredPosition;
	public AdvancedButton ButtonElement => _advBtn;

	// Use this for initialization
	void Start ()
	{
		_advBtn = GetComponent<AdvancedButton>();
	}

	void Update()
	{
		_HandleShadow();
	}

	void _HandleShadow()
	{
		if(shadowRect == null)
			return;

		if(_shadowDown == _advBtn.mouseDown)
			return;

		_shadowDown = _advBtn.mouseDown;

		if(_shadowDown)
			shadowRect.anchoredPosition = _rt.anchoredPosition + new Vector2(0, -7);
		else
			shadowRect.anchoredPosition = _rt.anchoredPosition + new Vector2(0, -10);
	}

	public void SetLevelIdx(int idx)
	{
		levelIdx = idx;
		btnText.text = idx.ToString();		
	}

	public void HookBtnPress(SceneLoader loader)
	{
		_loader = loader;
		_btn = GetComponent<Button>();		
  		_btn.onClick.AddListener(() =>{ BtnPressed(); });
	}

	void BtnPressed()
	{
		AudioManager.PlaySound(SoundEffect.LevelPressed);
		_loader.LoadLevel(levelIdx);
	}

	public void CheckIfShouldAppear(List<LevelButton> allLevels)
	{
		if(levelIdx == 1 || isCompleted)
		{
			gameObject.SetActive(true);
			return;
		}

		var shouldAppear = false;

		foreach(var lvl in allLevels)
		{
			if(lvl == this)
				continue;
			
			var dist = (lvl.position - position).magnitude;
			if(dist > 1.1f)
				continue;

			if(lvl.isCompleted)
			{
				shouldAppear = true;
				break;
			}
		}

		gameObject.SetActive(shouldAppear);
	}

	public void SetPosition(Vector2 pos)
	{
		position = pos;

		pos.x *= levelSpacing.x;
		pos.y *= levelSpacing.y;

		_rt = GetComponent<RectTransform>();
		_rt.anchoredPosition = pos;

		if(isCompleted)
			_rt.anchoredPosition += new Vector2(0, 4);
	}
}
