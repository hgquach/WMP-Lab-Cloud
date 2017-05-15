using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;
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
	// current session the player is on
	private int currentSession = 1;
	// the dot ratio for dot randomization 
	private RatioStruct dotRatio;
	// the color of the dots
	private Color dotColor;
	// not impelemented yet up a enum value that would determine the shape the dots are in
	// a string that tells what kind of object to represent the dot like an actual white dot or a dinosaur 
	private string dotSprite;
	// maximum amount of dots allowed on the screen
	private int dotMax;



	void Awake()
	{
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen");
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen");

		rightSidePos = rightScreen.transform.position;
		leftSidePos = leftScreen.transform.position;

		leftContainer = leftScreen.transform.GetChild (0);
		rightContainer = rightScreen.transform.GetChild (0);	
		userChoice = Sides.Null;


	}

	void Start () 
	{
		
		this.dotRatio = new RatioStruct(2,3);
		this.dotColor= Color.white;
		this.sessionMax = 10;
		this.dotSprite = "WhiteDot";
		this.dotMax = 50;

		createSession ();
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

	public void setColor (Color color)
	{
		this.dotColor = color;
	}

	public void setDotSprite(string sprite)
	{
		this.dotSprite = sprite;
	}

	// need to create some enumerations to make cleaner code
	// might need to create some sort of switch case to determine: which object to spawn and which shape to spawn the object in 
	// ignoring what shape the object should spawn in 

	public int getMaxSession()
	{
		return this.sessionMax;
	}

	public int getCurrentSession()
	{
		return this.currentSession;
	}
	public void incCurrentSession()
	{
		this.currentSession += 1;
	}
 	public void createSession()
	{

		
		RatioStruct dotPerSide = _dotPerSide (this.dotRatio,this.dotMax);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, dotPerSide.cloud1, this.dotColor, Sides.Left);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, dotPerSide.cloud2, this.dotColor, Sides.Right);

		this.CorrectAnswer= _correctAnswer (dotPerSide.cloud1, dotPerSide.cloud2);
		_clearDot (this.leftContainer, this.rightContainer);
	}
	void _spawnLocation(Transform lContainer, Transform rContainer,string obj, int amount , Color objectColor,Sides side)
	{

		switch(side)
		{
			case Sides.Left:
				_spawnDot (lContainer, obj, amount, leftSidePos , objectColor );
				break;
			case Sides.Right:
				_spawnDot (rContainer, obj, amount, rightSidePos  , objectColor);
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
			spawnDot.transform.parent = container ;
		}
	}

	public void _clearDot(Transform leftContainer ,Transform rightContainer)
	{
		
		for (int i = 0; i < leftContainer.childCount; i++) {	
			if (leftContainer.childCount > 0) {
				GameObject temp = leftContainer.GetChild (i).gameObject;
				temp.transform.parent = null;
				Destroy (temp);
			}
		}

		for (int i = 0; i < rightContainer.childCount; i++) {
			if (rightContainer.childCount > 0) {
				GameObject temp = rightContainer.GetChild (i).gameObject;
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

// 			Debug.Log (leftOrRight);
		while(!_isRatioInParameters(cloudDot,cloud1Ratio,cloud2Ratio,total))
		{
			cloudDot = Random.Range (min, largestDot+1); 
			Debug.Log ("cloud dot: " + cloudDot);
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
		if( (cloudDot*ratio1 + cloudDot*ratio2 <= totalDot)&& ((cloudDot*ratio1 + cloudDot*ratio2) > 0))
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

	public void displayResult()
	{
		_clearDot (this.leftContainer, this.rightContainer);
		if (_checkAnswer (this.CorrectAnswer, this.userChoice)) {
			GameObject result = Resources.Load ("correct1") as GameObject;
			GameObject spawnedResult =Instantiate (result, new Vector2(0,0), Quaternion.identity);
			StartCoroutine(waitAndDelete (spawnedResult));
		} else {

			GameObject result = Resources.Load ("incorrect1") as GameObject;
			GameObject spawnedResult = Instantiate (result, new Vector2(0,0), Quaternion.identity);
			StartCoroutine( waitAndDelete (spawnedResult));
		}
	}

	private IEnumerator waitAndDelete(GameObject resultSprite)
	{

		yield return new WaitForSeconds (1);
		Destroy (resultSprite);
	}

	public IEnumerator waitAndStartSession()
	{

		Debug.Log ("inside enumerator");
		_clearDot (this.leftContainer, this.rightContainer);
		yield return new WaitForSeconds (2);
		if (this.currentSession <= this.sessionMax) {
			this.incCurrentSession ();
			this.createSession ();
		}
		else{
			Debug.Log("finished");
			SceneManager.LoadScene (0);
		}
	}


}
