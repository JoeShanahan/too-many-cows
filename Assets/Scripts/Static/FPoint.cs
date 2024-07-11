using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Floating point convenience functions
public class FPoint
{
	public static bool isEqual(float a, float b, float tolerance=0.01f)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }

	public static bool isEqual(Vector2 a, Vector2 b, float tolerance=0.01f)
    {	
        return ((a-b).magnitude < tolerance);
    }

	public static float Lerp(float start, float end, float percent)
	{
		var diff = end - start;
		return start + (diff *percent);
	}
}
