using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For values between [0 and 1]
public static class SmoothFunctions
{

    public static float Overgrow(float inpVal, float scale=0.1f)
    {
        inpVal = Mathf.Clamp(inpVal, 0, 1);
        if (inpVal < 0.6f)
            return Mathf.Sin(inpVal * 3.1415f * 0.8333f) * (1f + (scale * 2));

        return 1f + scale + (Mathf.Sin((inpVal * 2.5f * 3.1415f) + 3.1415f) * scale);
    }

    public static float EaseOut(float inpVal)
    {
        inpVal = Mathf.Clamp(inpVal, 0, 1);
        return Mathf.Sin(inpVal * 0.5f * 3.1415f);
    }

    public static float EaseIn(float inpVal)
    {
        inpVal = Mathf.Clamp(inpVal, 0, 1);
        return Mathf.Sin((inpVal - 1) * 0.5f * 3.1415f) + 1;
    }
}
