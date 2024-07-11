using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Tutorials
{
	public class TutorialManager : MonoBehaviour
	{
		public GameObject dayTimer;
		public GameObject rewindBtn;

		public static bool timeLimit = true;
		public static bool canRewind = true;
		public static bool isTutorial = false;

		int _unlockRewindLevel = 3;
		int _unlockTimeLevel = 5;

		static TutorialManager _instance;

		// Use this for initialization
		void Start ()
		{
			var currentLevel = SaveData.GetCurrentLevel();
			TutorialManager.timeLimit = currentLevel >= _unlockTimeLevel;
			TutorialManager.canRewind = currentLevel >= _unlockRewindLevel;
			TutorialManager.isTutorial = currentLevel <= 5;

			rewindBtn.SetActive(TutorialManager.canRewind);
			dayTimer.SetActive(TutorialManager.timeLimit);
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
	}
}