using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class RoundManagerTest: MonoBehaviour {

    // holding left and right side information
	GameObject leftScreen;
	GameObject rightScreen;
	Transform leftContainer; 
	Transform rightContainer;
	Vector2 leftSidePos;
	Vector2 rightSidePos;
    SessionManager sessionManager;
    GameObject sideDivider;

	// used to make targeting left and right side easier
	public enum Sides {Right,Left,Null};

	// use to determine the user choice and correct choice
	// might change userChoice to private 2 lazy to create setter rn
	public Sides userChoice; 
	private Sides CorrectAnswer;

	// max trial amount before moving onto another round 
	private int trialMax;
	// current trial the player is on
    [SerializeField]
	private int currentTrial = 0;
	// the dot ratio for dot randomization 
    [SerializeField]
	private RatioStruct dotRatio;
    // the color of the dots

    [SerializeField]
    private Color dotColor;
	// not impelemented yet up a enum value that would determine the shape the dots are in
	// a string that tells what kind of object to represent the dot like an actual white dot or a dinosaur 
    [SerializeField]
	private string dotSprite;
	// maximum amount of dots allowed on the screen
    [SerializeField]
	private int dotMax;
	// make sure the user can only enter input is valid during certain periods
	private bool readyForUser = true;

    // keep a list of spawned dot gameobjects to iterate through to make filtering easier 
	private List<GameObject> dotList = new List<GameObject>();
    //private int for configuring dot separation
    private int dotSeparation;
    // private bools to symbolize when the round should start or not
    private bool isRoundStart;

    // bool to check if the round is currently underway 
    private bool roundRunning;
	void Awake()
	{
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen");
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen");
        sessionManager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        sideDivider = GameObject.FindGameObjectWithTag("Divider");
		rightSidePos = rightScreen.transform.position;
		leftSidePos = leftScreen.transform.position;

		leftContainer = leftScreen.transform.GetChild (0);
		rightContainer = rightScreen.transform.GetChild (0);	
		userChoice = Sides.Null;
        isRoundStart = false;
        roundRunning = false;

	}

	public bool isReadyForUser()
	{
		return this.readyForUser;
	}

	// use to set some of the mananger parameter 
	public void setTrialMax (int max)
	{
		this.trialMax = max;
	}

	public void setTrialRatio(RatioStruct desiredRatio)
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

    public void setDotSeperation(int distance)
    {
        this.dotSeparation = distance;
    }

    public void setDotMax(int max)
    {
        this.dotMax = max;
    }

    public int getMaxTrial()
	{
		return this.trialMax;
	}

	public int getCurrentTrial()
	{
		return this.currentTrial;
	}

    public void incCurrentTrial()
	{
		this.currentTrial += 1;
	}

    public bool getRoundStart()
    {
        return this.isRoundStart;
    }

    public void roundStart()
    {
        if(!this.isRoundStart)
        {
            this.isRoundStart = true;
            createTrial();
            this.currentTrial += 1;
            return;
        }
        this.isRoundStart = false;

    }

    public void createTrial()
	{

        _clearGameObjectList(this.dotList);
		RatioStruct dotPerSide = _dotPerSide (this.dotRatio,this.dotMax);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, this.dotSeparation, dotPerSide.cloud1, this.dotColor, Sides.Left);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, this.dotSeparation, dotPerSide.cloud2, this.dotColor, Sides.Right);
       StartCoroutine( this._addDivider());
		this.CorrectAnswer= _correctAnswer (dotPerSide.cloud1, dotPerSide.cloud2);
	}

    public void resetRoundValue()
    {
        this.userChoice = Sides.Null;
        this.currentTrial = 0;
        this.readyForUser = true;
    }

    private void _spawnLocation(Transform lContainer, Transform rContainer,string obj, int separation ,int amount , Color objectColor,Sides side)
	{

		switch(side)
		{
			case Sides.Left:
				_spawnDot (lContainer, obj, separation ,amount, leftSidePos , objectColor );
				break;
			case Sides.Right:
				_spawnDot (rContainer, obj, separation ,amount, rightSidePos  , objectColor);
				break; 
			default:
				Debug.Log ("Something Went Wrong");
				break;
		}

		
	}

	private void _spawnDot(Transform container,string obj, int separation ,int amount, Vector2 sideScale , Color objectColor)	
	{
		GameObject spawnDot;
		GameObject dot = Resources.Load (obj) as GameObject;
		Vector2 circlePos;
		Vector2 dotPos;
		for(int i = 0 ; i < amount ; i++)
		{
            bool restrictNotPass = true;
            do {
                circlePos = Random.insideUnitCircle * 3;
                dotPos = new Vector2(circlePos.x + sideScale.x, circlePos.y + sideScale.y);
                spawnDot = Instantiate(dot, dotPos, transform.rotation) as GameObject;
                if(!_checkOverlap(spawnDot,this.dotList) && !_checkSeparation(separation, spawnDot,this.dotList))
                {
                    restrictNotPass = false;
                }
                else
                {
                    Destroy(spawnDot);
                    //print("Destroyed dot");
                }
            }while ( restrictNotPass);
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

		//Debug.Log ("left container holds: "+leftContainer.childCount);
		//Debug.Log ("right container holds: "+rightContainer.childCount);
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
        this._removeDivider();
		yield return new WaitForSeconds (1);
		Destroy (resultSprite);
	}

	public IEnumerator waitAndStartSession()
	{

		//Debug.Log ("inside enumerator");
		yield return new WaitForSeconds (1.5f);
		if (this.currentTrial <= this.trialMax) {
			this.incCurrentTrial ();
			this.createTrial ();
			this.readyForUser = true;
		}
		else
        {
            Debug.Log("round is over");
            this.isRoundStart = false;
            sessionManager.incRound();
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
                        //print(" overlapped");
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
// there is a problem with this function because the dots positions are determined using the random.insideunitcirlce then multiplied by a multiplier 
// depending on the multiplier the distance of seperation required make the function's evaluation faulty. 
    private bool _checkSeparation(int distance,GameObject dotPos, List<GameObject> dotList)
    {
        if(dotList.Count > 0)
        {
            foreach(GameObject checkDot in dotList)
            {
                if (dotPos != null && checkDot != null)
                {
                    //print("distance of dots =: "+ Mathf.FloorToInt(Vector2.Distance(dotPos.GetComponent<Transform>().position,checkDot.GetComponent<Transform>().position)));
                    if (distance > (Mathf.CeilToInt(Vector2.Distance(dotPos.GetComponent<Transform>().position, checkDot.GetComponent<Transform>().position))))
                    {
                       // print("not far enough");
                        return true;
                    }
                }
            }
        }

        return false;   
    }

    private void _removeDivider()
    {
        sideDivider.GetComponent<LineRenderer>().enabled = false;
    }

    private IEnumerator _addDivider()
    {
        yield return new WaitForSeconds(.5f);
        sideDivider.GetComponent<LineRenderer>().enabled = true;
    }
}
