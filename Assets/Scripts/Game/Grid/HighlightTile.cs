using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTile : MonoBehaviour
{
 
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
 

	void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
 
    void Update()
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetFloat("_Alpha", Mathf.PingPong(Time.time, 1.0f));
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
}
