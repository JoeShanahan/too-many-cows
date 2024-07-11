using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Effects
{
	public class SinBob : MonoBehaviour
	{
		public float BobAmount;
		public float BobSpeed;

		Vector3 _startPos;

		void Start()
		{
			_startPos = transform.position;
		}

		void Update ()
		{
			var sinFactor = Mathf.Sin(Time.time * BobSpeed);
			transform.position = _startPos + (Vector3.up * BobAmount * sinFactor);
		}
	}
}