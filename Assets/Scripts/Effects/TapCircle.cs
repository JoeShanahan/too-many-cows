using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TooManyCows.DataObjects;

namespace TooManyCows.Effects
{
	public class TapCircle : MonoBehaviour
	{
		public float startSize = 0.4f;
		public float endSize = 1.2f;

		[Space(8)]
		public float startAlpha = 0.4f;
		public float evalTime = 0.8f;

		SpriteRenderer _renderer;
		float aliveTime = 0.0f;
		Vector3 _startSizeVect;
		Vector3 _endSizeVect;

		// Use this for initialization
		void Start ()
		{
			_renderer = GetComponent<SpriteRenderer>();
			_startSizeVect = new Vector3(startSize, startSize, startSize);
			_endSizeVect = new Vector3(endSize, endSize, endSize);
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(aliveTime >= evalTime)
				Destroy(gameObject);

			if(evalTime == 0)
				evalTime = 0.1f;

			aliveTime += Time.deltaTime;

			var evalPercent = aliveTime / evalTime;
			var alpha = Mathf.Max(0, 1 - evalPercent);
			var scale = SmoothFunctions.EaseOut(evalPercent);

			transform.localScale = Vector3.Lerp(_startSizeVect, _endSizeVect, scale);
			_renderer.color = new Color32(255, 255, 255, (byte)(255 * startAlpha * alpha));
			
		}
	}
}