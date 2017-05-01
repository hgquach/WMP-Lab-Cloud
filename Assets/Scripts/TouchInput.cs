using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private BoxCollider2D leftScreen; 
	private BoxCollider2D rightScreen;
	private GameState gamestate;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<BoxCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<BoxCollider2D>();
		gamestate = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameState> ();
	}
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			if (rightScreen.bounds.Contains (pos)) {
				Debug.Log ("inside right");
				// gamestate.choice = 1;
			}

			if (leftScreen.bounds.Contains (pos)) {
				Debug.Log ("inside left");
				// gamestate.choice = 2;
			}
//			Debug.Log ("touch world pos.x " + pos.x);
//			Debug.Log ("touch world pos.y "+ pos.y);
//			Debug.Log ("touch world pos.z "+ pos.z);
		}
			
	}




}

