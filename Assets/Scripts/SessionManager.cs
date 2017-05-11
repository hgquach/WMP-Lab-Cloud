﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class SessionManager: MonoBehaviour {

    // holding left and right side information
	GameObject leftScreen;
	GameObject rightScreen;
	Transform leftContainer; 
	Transform rightContainer;
	Vector2 leftSidePos;
	Vector2 rightSidePos;

	// used to make targeting left and right side easier
	public enum Sides {Right,Left,Null};

	// use to determine the user choice and correct choice
	// might change userChoice to private 2 lazy to create setter rn
	public Sides userChoice; 
	private Sides CorrectAnswer;

	// max session amount before moving onto another round 
	int sessionMax;
	private int currentSession = 1;

	// setting dot ratio
	private RatioStruct dotRatio;

	void Awake()
	{
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen");
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen");

		rightSidePos = rightScreen.transform.position;
		leftSidePos = leftScreen.transform.position;

		leftContainer = leftScreen.transform.GetChild (0);
		rightContainer = rightScreen.transform.GetChild (0);
		sessionMax = 10;
		userChoice = Sides.Null;
	}

	void Start () 
	{

	}

	void Update()
	{
		Debug.Log (userChoice);
		for (int currentSession = 1; currentSession < sessionMax ;) 
		{
			RatioStruct tempRatio = new RatioStruct (1, 2);
			RatioStruct dotPerSide = _dotPerSide (tempRatio, 50);
			_spawnLocation ("WhiteDot", dotPerSide.cloud1, Color.magenta, Sides.Left);
			_spawnLocation ("WhiteDot", dotPerSide.cloud2, Color.magenta, Sides.Right);

			CorrectAnswer = _correctAnswer (dotPerSide.cloud1, dotPerSide.cloud2);
			Debug.Log ("The correct answer is?: " + CorrectAnswer);
			Debug.Log ("Correct answer ?: " + _checkAnswer (CorrectAnswer, userChoice));
			
			StartCoroutine (WaitForUserInput ());
			_clearDot (leftScreen, rightScreen);
			currentSession++;
		}
	}

	// use to set some of the mananger parameter 
	public void setSessionMax (int max)
	{
		this.sessionMax = max;
	}

	public void setSessionRatio(RatioStruct desiredRatio)
	{
		this.dotRatio = desiredRatio;
	}

	// need to create some enumerations to make cleaner code
	// might need to create some sort of switch case to determine: which object to spawn and which shape to spawn the object in 
	// ignoring what shape the object should spawn in 


	void _spawnLocation(string obj, int amount , Color objectColor,Sides side)
	{

		switch(side)
		{
			case Sides.Left:
				_spawnDot (leftContainer, obj, amount, leftSidePos , objectColor );
				break;
			case Sides.Right:
				_spawnDot (rightContainer, obj, amount, rightSidePos  , objectColor);
				break; 
			default:
				Debug.Log ("Something Went Wrong");
				break;
		}

		
	}

	private void _spawnDot(Transform container,string obj, int amount, Vector2 sideScale , Color objectColor)	
	{
		GameObject spawnDot;
		GameObject dot = Resources.Load (obj) as GameObject;
		Vector2 circlePos;
		Vector2 dotPos;
		for(int i = 0 ; i < amount ; i++)
		{
			circlePos = Random.insideUnitCircle * 3 ; 
			dotPos = new Vector2 (circlePos.x + sideScale.x, circlePos.y + sideScale.y);
			spawnDot = Instantiate(dot,dotPos,transform.rotation) as GameObject;
			spawnDot.GetComponentInChildren<SpriteRenderer> ().color = objectColor;
			spawnDot.transform.parent = container;
		}
	}

	public void _clearDot(GameObject leftside ,GameObject rightside)
	{
		
		for (int i = 0; i < leftside.transform.childCount; i++) {	
			if (leftside.transform.childCount > 0) {
				GameObject temp = leftside.transform.GetChild (i).gameObject;
				temp.transform.parent = null;
				Destroy (temp);
			}
		}

		for (int i = 0; i < rightside.transform.childCount; i++) {
			if (rightside.transform.childCount > 0) {
				GameObject temp = rightside.transform.GetChild (i).gameObject;
				temp.transform.parent = null;
				Destroy (temp);
			}
		}
	}


	private Sides _correctAnswer(int leftTotal , int rightTotal)
	{
		if(leftTotal > rightTotal)
		{
			return Sides.Left;
		}
		else
		{
			return Sides.Right;
		}
	}

	private bool _checkAnswer(Sides correctAnswer ,Sides userChoice)
	{
		if (correctAnswer == userChoice)
			return true;
		
		return false;
	}

	private RatioStruct _dotPerSide(RatioStruct ratio,  int totalDots, int min =1)
	{
		RatioStruct dotPerSide = new RatioStruct(); 
		int cloud1Ratio = ratio.cloud1;
		int cloud2Ratio = ratio.cloud2; 
		int cloudDot = 0;
		int total = totalDots;
		int largestDot = _LargestRatio(cloud1Ratio,cloud2Ratio,total);
		int leftOrRight = Random.Range (1, 3);

		Debug.Log (leftOrRight);
		while(!_isRatioInParameters(cloudDot,cloud1Ratio,cloud2Ratio,total))
		{
			cloudDot = Random.Range (min, largestDot+1); 

		}


		switch (leftOrRight) 
		{
			case 1: 
				dotPerSide.cloud1 = Mathf.FloorToInt(cloudDot * cloud1Ratio);
				dotPerSide.cloud2 = Mathf.FloorToInt(cloudDot * cloud2Ratio);
				break;
			case 2:
				dotPerSide.cloud2 = Mathf.FloorToInt (cloudDot * cloud1Ratio);
				dotPerSide.cloud1 = Mathf.FloorToInt (cloudDot * cloud2Ratio);
				break;
		}

		return dotPerSide;

	}

	private bool _isRatioInParameters(int cloudDot ,int ratio1 , int ratio2, int totalDot)
	{
		if( (cloudDot*ratio1 + cloudDot*ratio2 <= totalDot)&& (cloudDot*ratio1 + cloudDot*ratio2) > 0)
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	private int _LargestRatio(int ratio1 , int ratio2 , int totalDot)
	{
		return  Mathf.FloorToInt (totalDot / (ratio1 + ratio2)); 
	}

	 IEnumerator WaitForUserInput()
	{
		while (Input.touchCount == 0) {
			yield return null;
		
		}
	}
}
