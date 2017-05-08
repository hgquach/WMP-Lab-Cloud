using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}



	// need to create some enumerations to make cleaner code
	// might need to create some sort of switch case to determine: which object to spawn and which shape to spawn the object in 
	// ignoring what shape the object should spawn in 


	void spawnLocation(string obj, int amount , Color objectColor,Sides side)
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
}
