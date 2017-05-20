using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class SessionManagerTest: MonoBehaviour {

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
	// make sure the user can only enter input is valid during certain periods
	private bool readyForUser = true;
	private List<GameObject> dotList = new List<GameObject>();


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

	public bool isReadyForUser()
	{
		return this.readyForUser;
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
        _clearGameObjectList(this.dotList);
		RatioStruct dotPerSide = _dotPerSide (this.dotRatio,this.dotMax);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, dotPerSide.cloud1, this.dotColor, Sides.Left);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, dotPerSide.cloud2, this.dotColor, Sides.Right);
        
		this.CorrectAnswer= _correctAnswer (dotPerSide.cloud1, dotPerSide.cloud2);
	}
	private void _spawnLocation(Transform lContainer, Transform rContainer,string obj, int amount , Color objectColor,Sides side)
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
            bool overLapChecking = true;
            do {
                circlePos = Random.insideUnitCircle * 3;
                dotPos = new Vector2(circlePos.x + sideScale.x, circlePos.y + sideScale.y);
                spawnDot = Instantiate(dot, dotPos, transform.rotation) as GameObject;
                if(_checkOverlap(spawnDot,this.dotList))
                {
                    Destroy(spawnDot);
                }
                else
                {
                    overLapChecking = false;
                }
            }while ( overLapChecking);
            spawnDot.GetComponentInChildren<SpriteRenderer>().color = objectColor;
			spawnDot.transform.parent = container ;
            this.dotList.Add(spawnDot);

		}
	}

	private void _clearDot(Transform leftContainer ,Transform rightContainer)
	{

		for (int i = leftContainer.childCount-1; i >= 0; i--) {	
			
				GameObject temp = leftContainer.GetChild (i).gameObject;
				temp.transform.parent = null;
				Destroy(temp);
			
		}

		for (int i = rightContainer.childCount-1; i >= 0; i--) {
			
				GameObject temp = rightContainer.GetChild (i).gameObject;
				temp.transform.parent = null;
				Destroy(temp);

		}

		Debug.Log ("left container holds: "+leftContainer.childCount);
		Debug.Log ("right container holds: "+rightContainer.childCount);
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
		do {
			cloudDot = Random.Range (min, largestDot + 1);
		} while(!_isRatioInParameters (cloudDot, cloud1Ratio, cloud2Ratio, total));


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
			case 3:
				Debug.Log ("you messed up");
				break;
			default:
				Debug.Log ("something is wrong ");
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
		this.readyForUser = false;
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

		yield return new WaitForSeconds (.5f);
		Destroy (resultSprite);
	}

	public IEnumerator waitAndStartSession()
	{

		Debug.Log ("inside enumerator");
		yield return new WaitForSeconds (1);
		if (this.currentSession <= this.sessionMax) {
			this.incCurrentSession ();
			this.createSession ();
			this.readyForUser = true;
		}
		else{
			Debug.Log("finished");
			SceneManager.LoadScene (0);
		}
	}

    private bool _checkOverlap(GameObject dotPos, List<GameObject> dotList)
    {
        if (dotList.Count > 0)
        {
            foreach (GameObject checkDot in dotList)
            {
                if (dotPos != null && checkDot !=null)
                {


                    if (checkDot.GetComponentInChildren<Renderer>().bounds.Intersects(dotPos.GetComponentInChildren<Renderer>().bounds))
                    {
                        print(" overlapped");
                        return true;

                    }
                }
            }
        }

        return false;
    }

    private void _clearGameObjectList(List<GameObject> dotList)
    {
        foreach(GameObject dot in dotList)
        {
            if(dot!= null)
            {
                Destroy(dot);
            }
        }
    }

}
