using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(100)]
public class LevelSelector : MonoBehaviour
{
    public MapSpawner map;
    public RectTransform mapRect;
    public MainMenu mainMenu;

    private RectTransform _rect;
    private bool _canMove = true;

    public RectTransform _squareBox;
    public RectTransform _starBox;

    private int _currentX;
    private int _currentY;
    private LevelButton _currentButton;

    private float _menuDeadTime;

    // TODO init on current level according to playerprefs

    void Start()
    {
        _rect = GetComponent<RectTransform>();

        _currentButton = map.GetLastPlayedLevel();
        
        if (_currentButton != null)
        {
            _currentX = Mathf.RoundToInt(_currentButton.position.x);
            _currentY = Mathf.RoundToInt(_currentButton.position.y);
            SetPosition(_currentButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenu.active)
        {
            _menuDeadTime = 0.2f;
            return;
        }

        if (_menuDeadTime > 0)
        {
            _menuDeadTime -= Time.deltaTime;
            return;
        }
            
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            _currentButton?.ButtonElement?.KeyboardKeyPressed(false);
    
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            _currentButton?.ButtonElement?.KeyboardKeyReleased(true);

        if (_currentButton.ButtonElement.keyboardDown)
            return;

        if (_canMove == false)
            return;

        Vector2 dirInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (dirInput.magnitude < 0.1f)
            return;

        if (Mathf.Abs(dirInput.x) > Mathf.Abs(dirInput.y))
        {
            if (dirInput.x > 0)
                DoMovement(Vector2.right);
            else
                DoMovement(Vector2.left);
        }
        else
        {
            if (dirInput.y > 0)
                DoMovement(Vector2.up);
            else
                DoMovement(Vector2.down);
        }
    }

    private void DoMovement(Vector2 direction)
    {
        int newX = Mathf.RoundToInt(_currentX + direction.x);
        int newY = Mathf.RoundToInt(_currentY + direction.y);

        LevelButton newBtn = map.GetButtonAt(newX, newY);

        if (newBtn == null)
            return;
        
        _currentButton = newBtn;
        _currentX = newX;
        _currentY = newY;

        SetPosition(newBtn);
        StartCoroutine(DisableMovement(0.25f));
    }

    private void SetPosition(LevelButton btn)
    {
        DOTween.Kill(_starBox);
        DOTween.Kill(_squareBox);
        DOTween.Kill(_rect);
     
        Vector2 targetPos = (_currentButton.anchoredPosition * mapRect.localScale.x) + mapRect.anchoredPosition;
        _rect.DOAnchorPos(targetPos, 0.25f).SetEase(Ease.OutExpo);

        if (btn.isCompleted)
        {
            _starBox.DOScale(1, 0.2f).SetEase(Ease.OutExpo);
            _squareBox.DOScale(0, 0.2f).SetEase(Ease.OutExpo);
        }
        else
        {
            _starBox.DOScale(0, 0.2f).SetEase(Ease.OutExpo);
            _squareBox.DOScale(1, 0.2f).SetEase(Ease.OutExpo);
        }
    }

    private IEnumerator DisableMovement(float seconds)
    {
        _canMove = false;
        yield return new WaitForSeconds(seconds);
        _canMove = true;
    }
}
