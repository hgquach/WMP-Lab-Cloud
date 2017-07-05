using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private BoxCollider2D leftScreen; 
	private BoxCollider2D rightScreen;
	private RoundManagerTest roundManager;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<BoxCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<BoxCollider2D>();
		roundManager= GameObject.FindGameObjectWithTag ("RoundManager").GetComponent<RoundManagerTest> ();
    }
	
	void Update () 
	{

		if (roundManager.isReadyForUser () && roundManager.getRoundStart()) 
		{
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				//Debug.Log ("current session: " + roundManager.getCurrentSession());
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
                if (rightScreen.bounds.Contains(pos))
                {
                    //Debug.Log ("inside right");
                    roundManager.userChoice = RoundManagerTest.Sides.Right;
                    roundManager.displayResult();
                    roundManager.waitAndStartTrial();

                }

                if (leftScreen.bounds.Contains(pos))
                {
                    //Debug.Log ("inside left");
                    roundManager.userChoice = RoundManagerTest.Sides.Left;
                    roundManager.displayResult();
                    roundManager.waitAndStartTrial();
                }

                //if (touchedCorner(pos,this.cornerArray))
                //{
                //    Debug.Log("you touched a corner");
                //}



//			Debug.Log ("touch world pos.x " + pos.x);
//			Debug.Log ("touch world pos.y "+ pos.y);
//			Debug.Log ("touch world pos.z "+ pos.z);
			}
		}

			
	}
}






