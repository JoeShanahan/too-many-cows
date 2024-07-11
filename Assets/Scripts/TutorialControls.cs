using System.Collections;
using System.Collections.Generic;
using TooManyCows.Tutorials;
using UnityEngine;

[DefaultExecutionOrder(500)]
public class TutorialControls : MonoBehaviour
{
	public PuzzleHandler puzzle;
    CanvasGroup _grp;

    // Start is called before the first frame update
    void Start()
    {
        _grp = GetComponent<CanvasGroup>();

        if (TutorialManager.isTutorial == false)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        bool speedUpTextVisible = puzzle.FarmerInBarn() && puzzle.currentState == PuzzleState.Unsolved;
        bool thisVisible = !speedUpTextVisible && puzzle.currentState != PuzzleState.Solved;

        if(thisVisible)
			_grp.alpha += Time.deltaTime * 2;
		else
			_grp.alpha -= Time.deltaTime * 2;

		_grp.alpha = Mathf.Clamp(_grp.alpha, 0, 1);
    }
}
