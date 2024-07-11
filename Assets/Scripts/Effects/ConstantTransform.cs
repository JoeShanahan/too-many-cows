using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyCows.Effects
{
	public class ConstantTransform : MonoBehaviour
	{
		public Vector3 rotateSpeed;
		public Vector3 translateSpeed;

		void Update ()
		{
			transform.Rotate(rotateSpeed * Time.deltaTime);
			transform.position += translateSpeed * Time.deltaTime;
		}
	}
}