using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithChildren : UnityEngine.UI.Button 
{
    [Header("Custom Stuff")]
    [SerializeField] Graphic[] _graphics = new Graphic[0];

    protected override void Awake()
    {
        base.Awake();
       _graphics = GetComponentsInChildren<Graphic>();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        Color color = Color.white;

        if (state == Selectable.SelectionState.Normal)
            color = colors.normalColor;

        if (state == Selectable.SelectionState.Highlighted)
            color = colors.highlightedColor;

        if (state == Selectable.SelectionState.Pressed)
            color = colors.pressedColor;

        if (state == Selectable.SelectionState.Disabled)
            color = colors.disabledColor;

        if (base.gameObject.activeInHierarchy)
            ColorTween(color * colors.colorMultiplier, instant);

    }

    private void ColorTween(Color targetColor, bool instant)
    {
        base.image.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);

        foreach (Graphic g in _graphics)
            g.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);
        
    }
}
