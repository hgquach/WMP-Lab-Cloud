using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour {

    private GameObject surveyObj, SessionTextObj;
    private TextUpdate textupdate;
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
    private int currentLevel , currentTheme,totalPointsInSess;
    public ThemeStruct roundTheme;
    public LevelStruct roundLevel;
    public bool isLevelDisplay;
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

            this.setupTimer();
        }
    }

    void Update()
    {
        //Debug.Log(roundManager.getRoundStart().ToString() + this.isLevelDisplay.ToString());
        if (roundManager.getRoundStart() && !this.isLevelDisplay)
        {
            //Debug.Log("new round");
            if (this.currentRound % GameData.gamedata.SessionPreferance.ThemeChange == 0 && this.currentRound != 1)
            {
                this.currentTheme = this.themeChange(this.currentTheme, GameData.gamedata.sessionTheme.Count-1);

                this.currentLevel = this.levelChange(GameData.gamedata.sessionLevels.Count,
                    this.currentLevel, roundManager.accuracySoFar, GameData.gamedata.SessionPreferance.MinError,
                    GameData.gamedata.SessionPreferance.MaxError);
            }
            Debug.Log("this is updated level: "+this.currentLevel);
            // adjust for zero based indexing
            this.roundLevel = GameData.gamedata.sessionLevels[this.currentLevel-1];
            this.roundTheme = FileIO.returnThemeInFolder((string)GameData.gamedata.sessionTheme[this.currentTheme]);
            this.textupdate.updateTextColor(this.roundTheme.UIColor);
            background.sprite = FileIO.returnSpriteInfolder((string)GameData.gamedata.sessionTheme[this.currentTheme],"background");
            background.GetComponent<BackgroundResize>().resizeBackGround();

            this.timeRoundstart = Time.time;
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

    private void UpdateRound(RatioStruct ratio,Color color , int trialMax , string dotSprite , int dotMax , float dotSepartaion)
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
            Debug.Log("level spread: "+tempLevel.spread);
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), GameData.gamedata.SessionPreferance.trialsPerRd, tempTheme.dotShape
                , tempLevel.dotMax, tempLevel.spread);
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
            if ( (currentLevel + 1) <= levelLength)
            {
                ++currentLevel;
            }
        }
        else if ( currentAccuracy <= minError)
        {
            if((currentLevel- 1) > 0)
            {
                --currentLevel;
            }
        }
        
        return currentLevel;



    }

    private int themeChange(int currentThemeIndex , int adjustTotalThemeCount)
    {
        if (currentThemeIndex < adjustTotalThemeCount)
        {
            currentThemeIndex += 1;
        }
        else
        {
            currentThemeIndex = 0;
        }

        return currentThemeIndex;
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
        yield return new WaitForSecondsRealtime(30f);
        Application.Quit();
    }

    public void runPointSummary()
    {
        StartCoroutine(displayPointSummary());
    }

    private void Assignment()
    {
        this.currentTheme = (GameData.gamedata.currentParticipant.Theme == 0) ? 0 : GameData.gamedata.currentParticipant.Theme;
        this.currentLevel = (GameData.gamedata.currentParticipant.Level == 0) ? 1 : GameData.gamedata.currentParticipant.Level;

        this.roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
        this.SessionTextObj = GameObject.FindGameObjectWithTag("LevelText");
        this.textupdate = SessionTextObj.GetComponent<TextUpdate>();
        this.background = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponentInChildren<SpriteRenderer>();
        this.sceneController = GameObject.FindGameObjectWithTag("GameSceneController").GetComponent<GameSceneController>();
        this.surveyObj = GameObject.FindGameObjectWithTag("Survey");

        this.isLevelDisplay = false;
        this.isTimedSession = GameData.gamedata.SessionPreferance.IsTimed;
        this.totalPointsInSess = 0;
        this.currentRound = 1;
    }
    
    public int getTotalPointsSes ()
    {
        return totalPointsInSess;
    }

    public void incTotalPointsInSess(int points)
    {
        this.totalPointsInSess += points;
    }

    private void setupTimer()
    {
        GameObject timerObject = (GameObject)Instantiate(Resources.Load("Timer"));
        timer = timerObject.GetComponent<Timer>();
        timer.setTimeLeft(this.timeLimit);
        roundManager.isTimed = true;
    }


}
