using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
	GUIStyle style = new GUIStyle();
	public Texture2D bg;

	// Use this for initialization
	void Start ()
	{
		style.normal.textColor = Color.white;
		style.normal.background = bg;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
