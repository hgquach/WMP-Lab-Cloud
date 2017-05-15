using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Use this for initialization
	private BoxCollider2D leftScreen; 
	private BoxCollider2D rightScreen;
	private SessionManager sessionmanager;
	void Awake()
	{
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen").GetComponent<BoxCollider2D>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen").GetComponent<BoxCollider2D>();
		sessionmanager= GameObject.FindGameObjectWithTag ("SessionManager").GetComponent<SessionManager> ();
	}
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Debug.Log ("current session: " + sessionmanager.getCurrentSession());
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			if (rightScreen.bounds.Contains (pos)) {
				Debug.Log ("inside right");
				sessionmanager.userChoice= SessionManager.Sides.Right;
				sessionmanager.displayResult ();
				StartCoroutine(sessionmanager.waitAndStartSession ());

			}

			if (leftScreen.bounds.Contains (pos)) {
				Debug.Log ("inside left");
				sessionmanager.userChoice= SessionManager.Sides.Left;
				sessionmanager.displayResult ();
				StartCoroutine(sessionmanager.waitAndStartSession ());
			}

//			Debug.Log ("touch world pos.x " + pos.x);
//			Debug.Log ("touch world pos.y "+ pos.y);
//			Debug.Log ("touch world pos.z "+ pos.z);
		}
			
	}




}

