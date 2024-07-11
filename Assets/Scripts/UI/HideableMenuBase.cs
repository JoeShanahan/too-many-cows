using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TooManyCows.UI.Game
{
	public class HideableMenuBase : MonoBehaviour
	{
		public RectTransform[] screenContent;
		public Vector2[] activePositions;
		public Vector2[] inactivePositions;
		public PuzzleHandler puzzle;
		public bool active;
		public GameObject defaultSelection;

		protected float _activePercent = 0f;
		protected CanvasGroup _canvasGrp;

		public void SetMenuActive(bool isActive)
		{
			if (active == isActive)
				return;

			active = isActive;

			if (active && defaultSelection != null)
			{
				EventSystem.current.SetSelectedGameObject(defaultSelection);	
			}
		}

		// Use this for initialization
		void Start ()
		{
			_canvasGrp = GetComponent<CanvasGroup>();
			_SnapToStartPos();
		}
		
		// Update is called once per frame
		protected virtual void Update ()
		{
			HandleActiveTimer();
			HandleElementsPosition();
			HandleCanvasGroup();
			KeyboardInput();
		}

		protected virtual void KeyboardInput()
		{

		}

		void _SnapToStartPos()
		{
			for(int i=0; i<screenContent.Length; i++)
				screenContent[i].anchoredPosition = inactivePositions[i];
		}

		public bool IsFullyHidden()
		{
			return !active && _canvasGrp.alpha == 0;
		}

		void HandleActiveTimer()
		{
			if(active)
				_activePercent += Time.deltaTime * 3;
			else
				_activePercent -= Time.deltaTime * 3;

			_activePercent = Mathf.Clamp(_activePercent, 0, 1);
		}

		void HandleCanvasGroup()
		{
			_canvasGrp.blocksRaycasts = active;
			_canvasGrp.interactable = active;
			_canvasGrp.alpha = Mathf.Clamp(_activePercent, 0, 1);
		}

		void HandleElementsPosition()
		{
			for(int i=0; i<screenContent.Length; i++)
			{
				var cont = screenContent[i];

				if(active)
					cont.anchoredPosition = Vector2.Lerp(cont.anchoredPosition, activePositions[i], Time.deltaTime * 9);
				else
					cont.anchoredPosition = Vector2.Lerp(cont.anchoredPosition, inactivePositions[i],  Time.deltaTime * 9);
			}
		}

		public virtual void DecideIfShouldBeActive()
		{
			
		}
	}
}