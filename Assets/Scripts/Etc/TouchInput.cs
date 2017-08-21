using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private BoxCollider2D leftScreen; 
	private BoxCollider2D rightScreen;
	private RoundManager roundManager;
    private SessionManager sessionManager;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<BoxCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<BoxCollider2D>();
		roundManager= GameObject.FindGameObjectWithTag ("RoundManager").GetComponent<RoundManager> ();
		sessionManager= GameObject.FindGameObjectWithTag ("SessionManager").GetComponent<SessionManager> ();
    }
	
	void Update () 
	{

		if (roundManager.isReadyForUser () && !sessionManager.isLevelDisplay) 
		{
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
                if (rightScreen.bounds.Contains(pos))
                {
                    roundManager.userChoice = RoundManager.Sides.Right;
                    StartCoroutine(roundManager.displayResult());
                }

                else if (leftScreen.bounds.Contains(pos))
                {
                    roundManager.userChoice = RoundManager.Sides.Left;
                    StartCoroutine(roundManager.displayResult());
                }

                if(!this.roundManager.getTrialRunning())
                {
                    this.roundManager.setTrialRunning();
                }
			}
		}

        
			
	}
}






