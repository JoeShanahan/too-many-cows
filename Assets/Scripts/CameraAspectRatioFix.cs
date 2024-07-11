using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatioFix : MonoBehaviour 
{
	float _aspectRatioLimit = 1.9f;
	float _newOrthoScale = 6.3f;

	// Use this for initialization
	void Start () 
	{
		var aspRatio = Screen.height / Screen.width;
		if(aspRatio < _aspectRatioLimit)
			return;

		var cam = GetComponent<Camera>();
		cam.orthographicSize = _newOrthoScale;
			
	}
	
}
