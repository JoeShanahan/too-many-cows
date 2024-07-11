using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public bool mouseDown = false;
	public bool keyboardDown = false;
	public RectTransform btnBody;

	public Image btnImage;

	public Color32 normalColor;
	public Color32 downColor;

	public Vector2 upPosition;
	public Transform[] toHide = new Transform[0];

	public float holdTime = 0;
	public bool isUndoButton;

	private Button _button;

	private void Start()
	{
		_button = GetComponent<Button>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnStateChanged();
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		OnStateChanged();
	}

	public void OnStateChanged()
	{
		mouseDown = Input.GetMouseButton(0);

		bool isReallyDown = mouseDown || keyboardDown;

		foreach(var obj in toHide)
			obj.gameObject.SetActive(!isReallyDown);
	}

	public void KeyboardKeyPressed(bool alsoPressButton)
	{
		if (keyboardDown)
			return;

		keyboardDown = true;
		OnStateChanged();
		holdTime += 0.25f;

		if (alsoPressButton)
			_button.onClick.Invoke();
	}

	public void KeyboardKeyReleased(bool alsoPressButton)
	{
		if (keyboardDown == false)
			return;

		keyboardDown = false;
		OnStateChanged();

		if (alsoPressButton)
			_button.onClick.Invoke();
	}

	void Update()
	{
		if(mouseDown || keyboardDown)
		{
			btnBody.anchoredPosition = Vector2.up * 4;
			btnImage.color = downColor;
			holdTime += Time.deltaTime;
		}
		else
		{
			btnBody.anchoredPosition = upPosition;
			btnImage.color = normalColor;
			holdTime = 0;			
		}
	}
}

