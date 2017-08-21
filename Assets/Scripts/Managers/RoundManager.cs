using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;
public class RoundManager : MonoBehaviour {

    // holding left and right side information
    // hold the line renderer that draws the line down the middle of the screen
    GameObject leftScreen, rightScreen, sideDivider,dotTemplate;
    Transform leftContainer, rightContainer;
    Vector2 leftSidePos, rightSidePos;
    private RecordingManager recordingmanager;
    public TextUpdate feedbackText;
    // holds the manager that updates the  round manager with information to create trials
    SessionManager sessionManager;
    // used to make targeting left and right side easier
    public enum Sides { Right, Left, Null };

    // use to determine the user choice and correct choice
    // might change userChoice to private 2 lazy to create setter rn
    public Sides userChoice, CorrectAnswer;

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
    public Color dotColor;
    // a string that tells what kind of object to represent the dot like an actual white dot or a dinosaur 
    [SerializeField]
    public string dotSprite;
    // maximum amount of dots allowed on the screen
    [SerializeField]
    private int dotMax;
    // make sure the user can only enter input is valid during certain periods
    private bool readyForUser = true;

    // keep a list of spawned dot gameobjects to iterate through to make filtering easier
    // split the list of spawned dot gameobject
    private List<List<GameObject>> dotLists = new List<List<GameObject>>();
    private List<List<Vector2>> allDotLocation = new List<List<Vector2>>();
    //int for configuring dot separation
    public int dotSeparation;
    // private bools to symbolize when the round should start or not
    // bool to determien if the feedback is being displayed
    private bool isRoundStart, isDisplayFeedback,isTrialRunning;
    //private bool to determine if the session is timed 
    public bool isTimed;
    // bool to check if the round is currently underway 
    private int trialSoFar, correctSoFar,feedbackRate;
    public float startTime,accuracySoFar,totalReactionTime;
    public GameObject RecordingManager;
    public RatioStruct dotPerSide;
    private GameSceneController sceneController;
    public int pointsPerRounds,bonusPoints;

    void Update()
    {
       //Debug.Log(Time.time - this.startTime);
        if( this.readyForUser && !this.isDisplayFeedback && isTrialRunning)
        {
          //  Debug.Log("inside of time expired");
            //Debug.Log(Time.time - this.startTime);
            if((Time.time - this.startTime) > 5f)
            {
                Debug.Log("TimedExpired");
                StartCoroutine(this.timeExpired());
                this.userChoice = Sides.Null;
            }
            

        }
        else
        {
            this.startTime = Time.time;
        }



    }

	void Awake()
	{
        this.Assignment();
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

    public bool getTrialRunning()
    {
        return this.isTrialRunning;
    }

    public void setTrialRunning()
    {
        if(isTrialRunning)
        {
            this.isTrialRunning = false;
            return;
        }
        this.isTrialRunning = true;
    }
    
    public bool getIsDisplay()
    {
        return this.isDisplayFeedback;
    }

    public void setDisplayFeedback()
    {
        this.isDisplayFeedback = this.isDisplayFeedback ? false : true;
    }

    public void roundStart()
    {
       if(!this.isTrialRunning)
        {
            this.sessionManager.isLevelDisplay = false;
            this.clearTrialScreen();
            this.isRoundStart = false;
            this.isTrialRunning = true;
            this.dotTemplate = new GameObject("dot");
            GameObject dotSprite = new GameObject("sprite");
            this.dotTemplate.AddComponent<PolygonCollider2D>();
            dotSprite.AddComponent<SpriteRenderer>().sprite = FileIO.returnSpriteInfolder(sessionManager.roundTheme.levelName,"stimuli");
            dotSprite.GetComponent<SpriteRenderer>().color = this.dotColor;
            dotSprite.transform.SetParent(this.dotTemplate.transform);
            this.dotTemplate.transform.localScale = new Vector3(.0625f, .0625f, 1);
            this.dotTemplate.SetActive(false);
            StartCoroutine(createTrial());
        }
        else { Debug.Log("round didnt start"); }

    }

    public void RoundEnd()
    {
        this.isRoundStart = false;
    }

    public void resetRoundValue()
    {
        this.userChoice = Sides.Null;
        this.currentTrial = 0;
        this.isTrialRunning = false;
    }

    public int getTrialSoFar()
    {
        return this.trialSoFar;
    }

    public int getCorrectSoFar()
    {
        return this.correctSoFar;
    }

    public void incCorrectSoFar()
    {
        this.correctSoFar++;
    }

    private void _spawnLocation(Transform lContainer, Transform rContainer,string obj, int separation ,int amount,Sides side)
	{

		switch(side)
		{
			case Sides.Left:
				_spawnDot (lContainer, obj, separation ,amount, leftSidePos,this.dotTemplate,side);
				break;
			case Sides.Right:
				_spawnDot (rContainer, obj, separation ,amount, rightSidePos,this.dotTemplate,side);
				break; 
			default:
				Debug.Log ("Something Went Wrong");
				break;
		}

		
	}

	private void _spawnDot(Transform container,string obj, int separation ,int amount, Vector2 sideScale,GameObject dotTemplate,Sides side)	
	{

		for(int i = 0 ; i < amount ; i++)
		{
            bool restrictNotPass = true;
            do {
                // see if we can do this with float values
                Vector2 circlePos = Random.insideUnitCircle * (separation + 1); 
                Vector2 dotPos = new Vector2(circlePos.x + sideScale.x, circlePos.y + sideScale.y);
                GameObject spawnDot = Instantiate(dotTemplate, dotPos, transform.rotation) as GameObject;
                spawnDot.SetActive(true);
               
                if(!_checkOverlap(spawnDot,this.dotLists ,side)  && ! _checkSeparation(separation,spawnDot,this.dotLists,side))
                {
                    restrictNotPass = false;
                    int sideIndex = (side == Sides.Left && side != Sides.Null ? 0 : 1);
                    this.dotLists[sideIndex].Add(spawnDot);
                    spawnDot.transform.SetParent(container);
                }
                else
                {
                    Destroy(spawnDot);
                }
            }while ( restrictNotPass);


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

	public bool checkAnswer(Sides correctAnswer ,Sides userChoice)
	{
        if (correctAnswer == userChoice)
        {
            return true;
        }
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

	public IEnumerator displayResult()
	{
		this.readyForUser = false;
        this.recordingmanager.StopRecording();
        this.clearTrialScreen();
        yield return new WaitForSecondsRealtime(.5f);
		if (checkAnswer (this.CorrectAnswer, this.userChoice)) {
            this.incCorrectSoFar();
			GameObject result = Resources.Load ("correct1") as GameObject;
			GameObject spawnedResult =Instantiate (result, new Vector2(0,0), Quaternion.identity);
            spawnedResult.GetComponent<SpriteRenderer>().color = sessionManager.roundTheme.UIColor;
            yield return new WaitForSecondsRealtime(.5f);
            this.sceneController.showCanvas();
            this.sceneController.assignContinueAfterSmiley();
		}
        else
        {
			GameObject result = Resources.Load ("incorrect1") as GameObject;
			GameObject spawnedResult = Instantiate (result, new Vector2(0,0), Quaternion.identity);
            spawnedResult.GetComponent<SpriteRenderer>().color = sessionManager.roundTheme.UIColor;
            yield return new WaitForSecondsRealtime(.5f);
            this.sceneController.showCanvas();
            this.sceneController.assignContinueAfterSmiley();
		}


	}

    public IEnumerator timeExpired()
    {
        this.readyForUser = false;
        this.recordingmanager.StopRecording();
        this.clearTrialScreen();
        Debug.Log("trying to load un happy face");
        GameObject result = Resources.Load("incorrect1") as GameObject;
        GameObject spawnedResult = Instantiate(result, new Vector2(0, 0), Quaternion.identity);
        spawnedResult.GetComponent<SpriteRenderer>().color = sessionManager.roundTheme.UIColor;
        yield return new WaitForSecondsRealtime(.5f);
        waitAndDelete();
        this.waitAndStartTrial();

    }

	public void  waitAndDelete()
	{
        this.clearTrialScreen();
        this.trialSoFar++;
        this.accuracySoFar= this.getTrialSoFar() == 0 ? .6f : (float)this.getCorrectSoFar() / (float)this.getTrialSoFar();
	}

    public IEnumerator createTrial()
	{
       // Debug.Log("create trials");
        this.clearTrialScreen();
		this.dotPerSide = _dotPerSide (this.dotRatio,this.dotMax);
        this.CorrectAnswer = this._correctAnswer(this.dotPerSide.cloud1, this.dotPerSide.cloud2);
        yield return new WaitForSecondsRealtime(1.5f);
        this._addDivider();
        yield return new WaitForSecondsRealtime(.5f);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, this.dotSeparation, dotPerSide.cloud1, Sides.Left);
		_spawnLocation ( this.leftContainer,this.rightContainer,this.dotSprite, this.dotSeparation, dotPerSide.cloud2, Sides.Right);
        this.readyForUser = true;
        this.incCurrentTrial();
        this.recordingmanager.StartRecording();
        this.startTime = Time.time;
	}

    public IEnumerator displayFeedBack()
    {
        this.clearTrialScreen();
        this.isTrialRunning = false;
        this.isDisplayFeedback = true;
        this.feedbackText.updateFeedBack(this.accuracySoFar, (this.totalReactionTime / this.trialSoFar),this.pointsPerRounds , this.bonusPoints);
        this.sessionManager.background.sprite = Resources.Load<Sprite>("Background/wood");
        this.sessionManager.background.GetComponent<BackgroundResize>().resizeBackGround();
        this.feedbackText.displayText();
        yield return new WaitForSecondsRealtime(1f);
        this.sceneController.showCanvas();
        this.sceneController.assignContinueAfterFeedback();

    }

    public void waitAndStartTrial()
	{
		//Debug.Log ("inside enumerator");
		if (this.currentTrial < this.trialMax)
        {
            this.isDisplayFeedback = false;
            StartCoroutine(this.createTrial ());
		}
		else
        {
            this.pointsPerRounds = this.calculateRegPts(this.correctSoFar, sessionManager.getCurrentLevel());
            if(this.accuracySoFar == 1f)
            {
                this.bonusPoints = calculatePerfectBonus(sessionManager.getCurrentLevel());
                this.pointsPerRounds += this.bonusPoints;
            }
            if (!this.isDisplayFeedback)
            {

                StartCoroutine(this.displayFeedBack());
            }
		}
	}

    private bool _checkOverlap(GameObject dotPos, List<List<GameObject>> dotList , Sides side)
    {
        int sideIndex = (side == Sides.Left&& side != Sides.Null) ? 0 : 1;
        if (dotList[sideIndex].Count > 0)
        {
            foreach (GameObject checkDot in dotList[sideIndex])
            {
                if (dotPos != null && checkDot !=null)
                {
                    if (checkDot.GetComponentInChildren<Renderer>().bounds.Intersects(dotPos.GetComponentInChildren<Renderer>().bounds))
                    {
                        return true;
                    }
                }
            }
        }
        return false;



    }

    private bool _checkSeparation(int distance,GameObject dotPos, List<List<GameObject>> dotList , Sides side)
    {
        int sideIndex = (side == Sides.Left && side != Sides.Null) ? 0 : 1;
        if(dotList[sideIndex].Count > 0)
        {
            foreach(GameObject checkDot in dotList[sideIndex])
            {
                if (dotPos != null && checkDot != null)
                {
                    //TODO: make distance an even smaller value use float values
                    int printDist = Mathf.FloorToInt(Vector2.Distance(dotPos.GetComponent<Transform>().position,checkDot.GetComponent<Transform>().position));
                    Debug.Log("this is the desired seperation: " + distance + " this is the distance a dot: " + printDist);
                    if (distance > (Mathf.CeilToInt(Vector2.Distance(dotPos.GetComponent<Transform>().position, checkDot.GetComponent<Transform>().position))))
                    {
                       print("not far enough");
                       return true;
                    }
                }
            }
        }

        return false;   
    }

    private void _clearGameObjectList(List<List<GameObject>> dotContainer)
    {
        foreach(List<GameObject> dotList in dotContainer)
        {
            foreach (GameObject dot in dotList)
            {
                if (dot != null)
                {
                    Destroy(dot);
                }
            }
        }
    }

    public void removeDivider()
    {
        sideDivider.GetComponent<LineRenderer>().enabled = false;
    }

    private void _addDivider()
    {
        sideDivider.GetComponent<LineRenderer>().startColor = sessionManager.roundTheme.UIColor;
        sideDivider.GetComponent<LineRenderer>().endColor = sessionManager.roundTheme.UIColor;
        sideDivider.GetComponent<LineRenderer>().enabled = true;
    }

    private void Assignment()
    {

        recordingmanager = RecordingManager.GetComponent<RecordingManager>();
		leftScreen = GameObject.FindGameObjectWithTag ("LeftScreen");
		rightScreen = GameObject.FindGameObjectWithTag ("RightScreen");
        sessionManager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        sideDivider = GameObject.FindGameObjectWithTag("Divider");
		rightSidePos = rightScreen.transform.position;
		leftSidePos = leftScreen.transform.position;
        this.feedbackText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();
		leftContainer = leftScreen.transform.GetChild (0);
		rightContainer = rightScreen.transform.GetChild (0);
        this.startTime = 0;
		userChoice = Sides.Null;
        this.isRoundStart = true;
        this.isTrialRunning = false;
        this.trialSoFar = 0;
        this.correctSoFar = 0;
        this.isDisplayFeedback = false;
        this.accuracySoFar = .6f;
        this.sceneController = GameObject.FindGameObjectWithTag("GameSceneController").GetComponent<GameSceneController>();
        this.pointsPerRounds = 0;
        this.bonusPoints = 0;
        this.dotLists.Add(new List<GameObject>());
        this.dotLists.Add(new List<GameObject>());
    }

    public void clearTrialScreen()
    {
        GameObject smileResult= GameObject.FindGameObjectWithTag("Smiley");
        this._clearGameObjectList(this.dotLists);
        this.removeDivider();
        this.feedbackText.hideText();
        if(smileResult != null)
        {
            Destroy(smileResult);
        }
    }

    public void resetForNewRound()
    {
        this.isDisplayFeedback = false;
        this.isRoundStart = true;
        this.totalReactionTime = 0f;
        this.correctSoFar = 0;
        this.trialSoFar = 0;
        sessionManager.incTotalPointsInSess(this.bonusPoints + this.pointsPerRounds);
        this.pointsPerRounds = 0;
        this.bonusPoints = 0;
        sessionManager.incRound();
    }

    private int calculateRegPts(int correct , int level)
    {
       return correct * level;
    }

    private int calculatePerfectBonus(int level)
    {
        return level * 10;
    }
}
