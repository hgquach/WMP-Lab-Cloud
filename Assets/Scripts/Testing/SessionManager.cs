﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour {

    // 		this.dotRatio = new RatioStruct(2,3);
    //this.dotColor= Color.blue;
    //this.trialMax = 10;
    //this.dotSprite = "WhiteDot";
    //this.dotMax = 50;
    //      this.dotSeparation = 1;
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
    private int currentLevel;
    void Awake()
    {
        roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManagerTest>();
        textupdate = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();

        this.currentRound = 1;
        this.currentLevel = 0;
        this.maxRound = 3;

        this.timeLimit = 120f;
        this.isTimedSession = false; 
        if (this.isTimedSession)
        {
            GameObject timerObject = (GameObject)Instantiate(Resources.Load("Timer"));
            timer = timerObject.GetComponent<Timer>();
            timer.setTimeLeft(this.timeLimit);
            roundManager.isTimed = true;

        }

    }

    void Update()
    {
        if (!roundManager.getRoundStart())
        {
            ThemeStruct roundTheme = GameData.gamedata.sessionTheme["WoodLand"];
            LevelStruct roundLevel = GameData.gamedata.sessionLevels[0];
            if (this.isTimedSession)
            {
                if (!timer.CheckTime())
                {
                    this.createNumberOfRounds(this.currentRound, this.maxRound,roundLevel,roundTheme);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                this.createNumberOfRounds(this.currentRound, this.maxRound , roundLevel,roundTheme);
            }
        }
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
