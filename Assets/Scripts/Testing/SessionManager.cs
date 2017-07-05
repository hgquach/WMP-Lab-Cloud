using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour {


    TextUpdate textupdate;
    RoundManagerTest roundManager;
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
    private float roundAccuracy;
    void Awake()
    {
        this.isTimedSession = GameData.gamedata.trialData.isRoundTimed;
        roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManagerTest>();
        textupdate = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();

        this.currentRound = 1;
        this.currentLevel = 1;
        if (!this.isTimedSession)
        {
            this.maxRound = GameData.gamedata.trialData.RoundLimit;
        }
        else
        {
            this.timeLimit = GameData.gamedata.trialData.RoundLimit * 60f;
            GameObject timerObject = (GameObject)Instantiate(Resources.Load("Timer"));
            timer = timerObject.GetComponent<Timer>();
            timer.setTimeLeft(this.timeLimit);
            roundManager.isTimed = true;
        }
        this.roundAccuracy = 1;

    }

    void Update()
    {
        if (!roundManager.getRoundStart())
        {
            roundAccuracy = roundManager.getCorrectSoFar() == 0 ? .6f : (float)roundManager.getCorrectSoFar() / (float)roundManager.getTrialSoFar();
            this.currentLevel = this.levelChange(GameData.gamedata.sessionLevels.Count, this.currentLevel, roundAccuracy);
            int adjustLevel = this.currentLevel - 1;
            roundLevel = GameData.gamedata.sessionLevels[adjustLevel];
            roundTheme = GameData.gamedata.sessionTheme["WoodLand"];
            if (this.isTimedSession)
            {
                if (!timer.CheckTime())
                {
                    this.timeRoundstart = Time.time;
                    this.createNumberOfRounds(this.currentRound, this.maxRound, roundLevel, roundTheme);
                }
                else
                {
                    // temporary until end scene is figured out
                    SceneManager.LoadScene(0);
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
        Debug.Log("dot color should be: " + color);
        Debug.Log("updating round");
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
        textupdate.displayTitle();
        yield return new WaitForSeconds(.5f);
        textupdate.hideTitle();
    }

    private void createNumberOfRounds(int currentRound , int maxRound,LevelStruct currentLevel,ThemeStruct roundTheme)
    {
        if (currentRound <= maxRound)
        {
            // this is all temp
            LevelStruct tempLevel = currentLevel;
            ThemeStruct tempTheme = roundTheme;
            Debug.Log("new round");

            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();
        }
        else
        {
            Debug.Log("Session over");
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
        else if ( currentLevel <= .5f)
        {
            if((currentLevel- 1) > 0)
            {
                currentLevel--;
            }
        }
        return currentLevel;



    }
    /* used to update the round manager for timed sessions
    private void createTimeLimitRounds(float timeLeft)
    {

            // this is all temp
            LevelStruct tempLevel = levelArray[this.currentLevel];
            ThemeStruct tempTheme = themeArray[0];
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();

        
    }
    */
}
