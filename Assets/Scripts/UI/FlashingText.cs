using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingText : MonoBehaviour
{
    public float flashSpeed = 3f;
    public bool isVisible;

    bool _isActivated = false;
    Text _text;
    
    float _phase = 0f;
    float _lowerBound = 0.3f;
    float _visibility = 0f;
    CanvasGroup _canvasGrp;
    

	// Use this for initialization
	void Start ()
    {
        _text = GetComponent<Text>();
        _canvasGrp = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        _HandleVisibility();
    
        if(isVisible)
            _phase += Time.deltaTime * flashSpeed;

        if (_isActivated)
            DoActive();
        else
            DoInactive();
	}

    void DoActive()
    {
        var alpha = 1f;

        if (_phase < 0.3f)
            alpha = 1f;
        else if (_phase < 0.6f)
            alpha = _lowerBound;
        else if (_phase < 0.9f)
            alpha = 1f;
        else if (_phase < 1.2f)
            alpha = _lowerBound;
        else if (_phase < 1.5f)
            alpha = 1f;
        else if (_phase < 1.8f)
            alpha = _lowerBound;
        else if (_phase < 2.1f)
            alpha = 1f;
        
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
    }

    void DoInactive()
    {
        var alpha = ((Mathf.Sin(_phase) + 1) / 2);
        alpha = (alpha * (1 - _lowerBound)) + _lowerBound;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
    }

    public void Activate()
    {
        _isActivated = true;
        _phase = 0f;
    }

    void _HandleVisibility()
    {
        if(isVisible)
            _visibility += Time.deltaTime;
        else
            _visibility -= Time.deltaTime;

        _visibility = Mathf.Clamp(_visibility, 0, 1);

        _canvasGrp.alpha = _visibility;
    }
}
