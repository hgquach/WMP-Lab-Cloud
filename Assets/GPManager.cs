using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPManager : MonoBehaviour {

	// Use this for initialization
	GameObject leftScreen;
	GameObject rightScreen;
	Transform leftContainer; 
	Transform rightContainer;

	Vector2 leftSidePos;
	Vector2 rightSidePos;
	enum Sides {Right,Left};
	void Awake()
	{
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen");
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen");

		rightSidePos = rightScreen.transform.position;
		leftSidePos = leftScreen.transform.position;

		leftContainer = leftScreen.transform.GetChild (0);
		rightContainer = rightScreen.transform.GetChild (0);
	}

	void Start () 
	{
		
		spawnLocation("WhiteDot",50,Color.white,Sides.Left);
		spawnLocation("WhiteDot",50,Color.blue,Sides.Right);
		
	}

	void LateUpdate()
	{
		_clearDot (leftContainer.gameObject, rightContainer.gameObject);
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

		int leftTotal = leftside.transform.childCount;
		int rightTotal = rightside.transform.childCount; 
		for (int i = 0; i <= leftTotal; i++) 
		{
			GameObject temp = leftside.transform.GetChild (i).gameObject;
			temp.transform.parent = null;
			Destroy (temp);
		}
		for (int i = 0; i <= rightTotal; i++) 
		{
			GameObject temp = leftside.transform.GetChild (i).gameObject;
			temp.transform.parent = null;
			Destroy (temp);
		}


	}
}
