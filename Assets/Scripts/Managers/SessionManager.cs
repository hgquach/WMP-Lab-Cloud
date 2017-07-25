using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour {

    private GameObject surveyObj;
    private TextUpdate textupdate;
    private GameObject SessionTextObj;
    private RoundManager roundManager;
    public SpriteRenderer background;
    private Timer timer;
    private GameSceneController sceneController;
    [SerializeField]
    private bool isTimedSession;
    // currently in seconds for testing but i will create a function to convert mins to seconds
    [SerializeField]
    private float timeLimit;
    [SerializeField]
    private int maxRound;
    [SerializeField]
    private int currentRound;
    [SerializeField]
    public float timeRoundstart;
    [SerializeField]
    private int currentLevel , currentTheme;
    public ThemeStruct roundTheme;
    public LevelStruct roundLevel;
    public bool isLevelDisplay;
    private int totalPointsInSess;
    void Awake()
    {
        if(GameData.gamedata.isDemo)
        {
            this.isTimedSession = false;
        }
        this.Assignment();
        if (!this.isTimedSession)
        {
            if (GameData.gamedata.trialData.RoundLimit != 0)
            {
                this.maxRound = GameData.gamedata.trialData.RoundLimit;
            }
            else
            {
                GameData.gamedata.trialData.RoundLimit = 3;
                this.maxRound = 3;
            }
        }
        else
        {
            if(GameData.gamedata.trialData.RoundLimit != 0)
            {
                this.timeLimit = (float)GameData.gamedata.trialData.RoundLimit;
            }
            else
            {
                GameData.gamedata.trialData.RoundLimit = 120;
                this.timeLimit = 120f;
            }
            GameObject timerObject = (GameObject)Instantiate(Resources.Load("Timer"));
            timer = timerObject.GetComponent<Timer>();
            timer.setTimeLeft(this.timeLimit);
            roundManager.isTimed = true;
        }
    }

    void Update()
    {
        //Debug.Log("current level is: " + currentLevel);
        //Debug.Log(string.Format("{0}correct so far {1} trials so far", roundManager.getCorrectSoFar(),roundManager.getTrialSoFar()));
        if (!roundManager.getRoundStart() && !this.isLevelDisplay)
        {
            if (this.currentRound % GameData.gamedata.SessionPreferance.ThemeChange == 0)
            {
                if (this.currentTheme < GameData.gamedata.sessionTheme.Count - 1)
                {
                    this.currentTheme += 1;
                }
                else
                {
                    this.currentTheme = 0;
                }
            }
            if (this.currentRound != 1)
            {
                this.currentLevel = this.levelChange(GameData.gamedata.sessionLevels.Count-1,
                    this.currentLevel, roundManager.accuracySoFar, GameData.gamedata.SessionPreferance.MinError,
                    GameData.gamedata.SessionPreferance.MaxError);
            }
            // the adjustment is for indexing the array
            int adjustLevel = this.currentLevel - 1;
            roundLevel = GameData.gamedata.sessionLevels[adjustLevel];
            this.roundTheme = (ThemeStruct)GameData.gamedata.sessionTheme[this.currentTheme];
            this.textupdate.updateTextColor(this.roundTheme.UIColor);
            background.sprite = Resources.Load<Sprite>("Background/" + this.roundTheme.backgroundImage);
            background.GetComponent<BackgroundResize>().resizeBackGround();
            if (this.isTimedSession)
            {
                if (!timer.CheckTime())
                {
                    this.createTimeLimitRounds(this.roundLevel,this.roundTheme);
                }
                else
                {
                    this.EndSession();
                }
            }
            else
            {

                this.createNumberOfRounds(this.currentRound, this.maxRound, roundLevel, roundTheme);
            }
        }
    }

    public int getCurrentRound()
    {
        return this.currentRound;
    }

    public int getCurrentLevel()
    {
        return this.currentLevel;
    }

    private void UpdateRound(RatioStruct ratio,Color color , int trialMax , string dotSprite , int dotMax , int dotSepartaion = 2)
    {
        this.roundManager.resetRoundValue();
        this.roundManager.setColor(color);
        this.roundManager.setDotSeperation(dotSepartaion);
        this.roundManager.setTrialMax(trialMax);
        this.roundManager.setDotSprite(dotSprite);
        this.roundManager.setTrialRatio(ratio);
        this.roundManager.setDotMax(dotMax);
    }

    public void incRound()
    {
        this.currentRound += 1;
    }

    private IEnumerator waitDisplay(int level)
    {
        this.isLevelDisplay = true;
        textupdate.updateRoundTitle(level);
        roundManager.clearTrialScreen();
        yield return new WaitForSecondsRealtime(1f);
        this.timeRoundstart = Time.time;
        textupdate.displayText();
        sceneController.showCanvas();
        sceneController.assignContinueLevel();
        // go button can call this function
        //roundManager.roundStart();
        //textupdate.hideText();


    }

    private void createNumberOfRounds(int currentRound , int maxRound,LevelStruct currentLevel,ThemeStruct roundTheme) 
    {
        if (currentRound <= maxRound)
        {
            LevelStruct tempLevel = currentLevel;
            ThemeStruct tempTheme = roundTheme;
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), GameData.gamedata.SessionPreferance.trialsPerRd, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum));
        }
        else
        {
            this.EndSession();
        }
    }

    private int levelChange(int levelLength,int currentLevel ,float currentAccuracy , int minError , int maxError)
    {
        float minErrorPercent = ((float)minError / 100);
        float maxErrorPercent = ((float)maxError / 100);
        //Debug.Log(minErrorPercent.ToString() + "," + maxErrorPercent.ToString());
        if(currentAccuracy > maxErrorPercent)
        {
            if ( (currentLevel + 1) < levelLength)
            {
                currentLevel++;
            }
        }
        // the min error you can make to move down
        else if ( currentAccuracy <= minError)
        {
            if((currentLevel- 1) > 0)
            {
                currentLevel--;
            }
        }
        return currentLevel;



    }

    private void createTimeLimitRounds(LevelStruct currentLevel , ThemeStruct roundTheme)
    {
            LevelStruct tempLevel = currentLevel;
            ThemeStruct tempTheme = roundTheme;
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), GameData.gamedata.SessionPreferance.trialsPerRd, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum));
            //roundManager.roundStart();

        
    }
    
    private void EndSession()
    {
        this.roundManager.RoundEnd();
        GameData.gamedata.currentParticipant.Level = this.currentLevel;
        GameData.gamedata.currentParticipant.Theme = this.currentTheme;
        GameData.gamedata.currentParticipant.PointTotal += this.totalPointsInSess;
        Debug.Log("current total: "+GameData.gamedata.currentParticipant.PointTotal);
        if (!GameData.gamedata.isDemo)
        {
            FileIO.writeParticipantFile(GameData.gamedata.currentParticipant);
        }
        roundManager.clearTrialScreen();
        textupdate.updateParticipantsTotalPoints(this.totalPointsInSess, GameData.gamedata.currentParticipant.PointTotal);
        sceneController.assignContinueAfterCoinSummary();

        if(GameData.gamedata.SessionPreferance.SurveyQuestion && !GameData.gamedata.isDemo)
        {
            background.sprite = Resources.Load<Sprite>("Background/wood");
            background.GetComponent<BackgroundResize>().resizeBackGround();
            this.surveyObj.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            StartCoroutine(this.displayPointSummary());
        }


    }

    public IEnumerator displayPointSummary()
    {
        background.sprite = Resources.Load<Sprite>("Background/Final");
        background.GetComponent<BackgroundResize>().resizeBackGround();
        this.isLevelDisplay = true;
        textupdate.displayText();
        yield return new WaitForSecondsRealtime(1f);
        sceneController.showCanvas();
    }

    public void runPointSummary()
    {
        StartCoroutine(displayPointSummary());
    }
    private void Assignment()
    {
        this.currentTheme = (GameData.gamedata.currentParticipant.Theme == 0) ? 0 : GameData.gamedata.currentParticipant.Theme;
        this.isTimedSession = GameData.gamedata.SessionPreferance.IsTimed;
        this.roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
        this.SessionTextObj = GameObject.FindGameObjectWithTag("LevelText");
        this.textupdate = SessionTextObj.GetComponent<TextUpdate>();
        this.background = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponentInChildren<SpriteRenderer>();
        this.currentRound = 1;
        this.currentLevel = (GameData.gamedata.currentParticipant.Level == 0) ? 1 : GameData.gamedata.currentParticipant.Level;
        this.sceneController = GameObject.FindGameObjectWithTag("GameSceneController").GetComponent<GameSceneController>();
        this.surveyObj = GameObject.FindGameObjectWithTag("Survey");
        this.isLevelDisplay = false;
        this.totalPointsInSess = 0;
    }
    
    public int getTotalPointsSes ()
    {
        return totalPointsInSess;
    }

    public void incTotalPointsInSess(int points)
    {
        this.totalPointsInSess += points;
    }

}
