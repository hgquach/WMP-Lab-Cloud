﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private PolygonCollider2D leftScreen; 
	private PolygonCollider2D rightScreen;
	private RoundManagerTest roundManager;
    //private BoxCollider2D[] cornerArray;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<PolygonCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<PolygonCollider2D>();
		roundManager= GameObject.FindGameObjectWithTag ("RoundManager").GetComponent<RoundManagerTest> ();
       // cornerArray = GameObject.FindGameObjectWithTag("Corner").GetComponentsInChildren<BoxCollider2D>();
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
                    roundManager.waitAndStartSession();

                }

                if (leftScreen.bounds.Contains(pos))
                {
                    //Debug.Log ("inside left");
                    roundManager.userChoice = RoundManagerTest.Sides.Left;
                    roundManager.displayResult();
                    roundManager.waitAndStartSession();
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

    private bool touchedCorner(Vector3 pos , BoxCollider2D[] cornerColliders)
    {
        Debug.Log(pos);
        foreach(BoxCollider2D corner in cornerColliders)
        {
            Debug.Log(corner.bounds.Contains(pos));

            if (corner.bounds.Contains(pos))
            {
                return true;
            }
        }

        return false;
    }




}

