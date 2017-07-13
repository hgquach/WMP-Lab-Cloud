using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour {


    TextUpdate textupdate;
    RoundManagerTest roundManager;
    SpriteRenderer background;
    Timer timer;
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
    private int currentLevel;
    public ThemeStruct roundTheme;
    public LevelStruct roundLevel;
    [SerializeField]
    void Awake()
    {
        if(GameData.gamedata.isDemo)
        {
            this.isTimedSession = false;
        }
        this.Assignment();
        background.sprite = Resources.Load<Sprite>("Background/"+this.roundTheme.backgroundImage);

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
        if (!roundManager.getRoundStart())
        {
            this.currentLevel = this.levelChange(GameData.gamedata.sessionLevels.Count, this.currentLevel, roundManager.accuracySoFar);
            int adjustLevel = this.currentLevel - 1;
            roundLevel = GameData.gamedata.sessionLevels[adjustLevel];
            if (this.isTimedSession)
            {
                if (!timer.CheckTime())
                {
                    this.timeRoundstart = Time.time;
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

    private IEnumerator waitDisplay(int level ,string themeName)
    {
        yield return new WaitForSeconds(.5f);
        textupdate.updateRoundTitle(level, themeName);
        textupdate.displayText();
        yield return new WaitForSeconds(.5f);
        textupdate.hideText();
    }

    private void createNumberOfRounds(int currentRound , int maxRound,LevelStruct currentLevel,ThemeStruct roundTheme)
    {
        if (currentRound <= maxRound)
        {
            LevelStruct tempLevel = currentLevel;
            ThemeStruct tempTheme = roundTheme;
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();
        }
        else
        {
          this.EndSession();
        }
    }

    private int levelChange(int levelLength, int currentLevel ,  float accuracy)
    {
        if(accuracy > .75f)
        {
            if ( (currentLevel + 1) <= levelLength)
            {
                currentLevel++;
            }
        }
        else if ( accuracy <= .5f)
        {
            if((currentLevel- 1) >= 0)
            {
                currentLevel--;
            }
        }
        Debug.Log("this is the current level: " + currentLevel);
        return currentLevel;



    }

    private void createTimeLimitRounds(LevelStruct currentLevel , ThemeStruct roundTheme)
    {
            LevelStruct tempLevel = currentLevel;
            ThemeStruct tempTheme = roundTheme;
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();

        
    }
    
    private void EndSession()
    {
        GameData.gamedata.playerPref.Level = this.currentLevel;
        GameData.gamedata.playerPref.Theme = this.roundTheme.levelName;
        if (!GameData.gamedata.isDemo)
        {
            FileIO.createPrefFile(GameData.gamedata.playerPref, int.Parse(GameData.gamedata.trialData.PartcipantId));
        }
        SceneManager.LoadScene(0);
    }
    
    private void Assignment()
    {

        this.isTimedSession = GameData.gamedata.trialData.isRoundTimed;
        this.roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManagerTest>();
        this.textupdate = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();
        this.background = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponentInChildren<SpriteRenderer>();
        this.currentRound = 1;
        this.currentLevel = (GameData.gamedata.trialData.Level == 0) ? 1 : GameData.gamedata.trialData.Level;
        this.roundTheme = GameData.gamedata.sessionTheme["Moon"];

    }
}
