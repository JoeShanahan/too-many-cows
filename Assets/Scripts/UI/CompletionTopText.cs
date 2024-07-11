using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionTopText : MonoBehaviour
{
	public MainMenu mainMenu;
	public int totalLevels;
	public Text text;

	public bool isCompleted;
	Vector3 _glowCol1 = new Vector3(255, 255, 128);
	Vector3 _glowCol2 = new Vector3(222, 222, 55);

	public Transform particleObject;
	public CanvasGroup menuGrp;

	// Use this for initialization
	void Start ()
	{
		totalLevels = Mathf.Max(totalLevels, 1);
		var completedCount = 0;

		foreach(var state in SaveData.GetLevelStates())
			if(state)
				completedCount++;

		var percent = Mathf.Round(((float)completedCount / totalLevels) * 100);
		text.text = percent.ToString() + "% Complete";

		if(completedCount < totalLevels)
		{
			GetComponent<Button>().enabled = false;
		}
		else if(!SaveData.CongratulationsShown)
		{
			mainMenu.EnableCongratulations(true);
			SaveData.CongratulationsShown = true;
		}

		isCompleted = completedCount >= totalLevels;
		particleObject.gameObject.SetActive(isCompleted);
	}

	void Update()
	{
		if(isCompleted)
		{
			_GlowAnim();
			_HandleParticlesActive();
		}

	}

	void _GlowAnim()
	{
		var phase = (Mathf.Sin(Time.time * 2.5f) + 1) / 2;
		var vec = Vector3.Lerp(_glowCol1, _glowCol2, phase);
		text.color = new Color32((byte)vec.x, (byte)vec.y, (byte)vec.z, 255);
	}

	void _HandleParticlesActive()
	{
		if(menuGrp.alpha > 0 && particleObject.gameObject.activeSelf)
			particleObject.gameObject.SetActive(false);

		if(menuGrp.alpha == 0 && !particleObject.gameObject.activeSelf)
			particleObject.gameObject.SetActive(true);
	}
}
