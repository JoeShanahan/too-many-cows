using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSqueeze : MonoBehaviour
{
    public float speed = 1f;
    [Range(0f, 1f)]
    public float factor = 0.1f;
    public float phase = 0f;

    Vector2 startSize;
    RectTransform rt;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startSize = rt.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        phase += Time.deltaTime * speed;
        var vertScale = startSize.y + (factor * Mathf.Sin(phase));
        var horzScale = startSize.x + (-factor * Mathf.Sin(phase));
        rt.localScale = new Vector2(horzScale, vertScale);
    }
}
