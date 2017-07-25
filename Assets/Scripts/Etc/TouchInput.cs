using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private BoxCollider2D leftScreen; 
	private BoxCollider2D rightScreen;
	private RoundManager roundManager;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<BoxCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<BoxCollider2D>();
		roundManager= GameObject.FindGameObjectWithTag ("RoundManager").GetComponent<RoundManager> ();
    }
	
	void Update () 
	{

		if (roundManager.isReadyForUser () && roundManager.getRoundStart()) 
		{
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
                //if(roundManager.getIsDisplay())
                //{
                //    roundManager.startTime = Time.time;
                //    roundManager.feedbackText.hideText();
                //    // same for here

                //   // roundManager.waitAndStartTrial();
                //}

                if (rightScreen.bounds.Contains(pos))
                {
                    Debug.Log ("inside right");
                    roundManager.userChoice = RoundManager.Sides.Right;
                    StartCoroutine(roundManager.displayResult());
               
                    // pause here and button will call wait and start trial inside of display result

                }

                else if (leftScreen.bounds.Contains(pos))
                {
                    Debug.Log ("inside left");
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






