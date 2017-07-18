﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerWithTimes : MonoBehaviour {

    // 		this.dotRatio = new RatioStruct(2,3);
    //this.dotColor= Color.blue;
    //this.trialMax = 10;
    //this.dotSprite = "WhiteDot";
    //this.dotMax = 50;
    //      this.dotSeparation = 1;
    TextUpdate textupdate;
    RoundManager roundManager;
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
    private ThemeStruct[] themeArray = new ThemeStruct[2];
    private LevelStruct[] levelArray = new LevelStruct[3];

    void Awake()
    {
        roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
        textupdate = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();

        levelArray[0] = new LevelStruct(1,10,1,new RatioStruct(1,3));
        levelArray[1] = new LevelStruct(2,50,1,new RatioStruct(3,4));

        //themeArray[0] = new ThemeStruct("WoodLand","star",new List<Color> { Color.white,Color.blue,Color.yellow,Color.red},Color.black);
        this.currentRound = 1;
        this.currentLevel = 0;
        this.maxRound = 3;

        this.timeLimit = 120f;
        this.isTimedSession = true; 
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
            if (this.isTimedSession)
            {
                if (!timer.CheckTime())
                {
                    this.createTimeLimitRounds(this.timeLimit);
                }
            }
            else
            {
                this.createNumberOfRounds(this.currentRound, this.maxRound);
            }
        }
    }

    private void UpdateRound(RatioStruct ratio,Color color , int trialMax , string dotSprite , int dotMax , int dotSepartaion = 2)
    {
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
        textupdate.updateRoundTitle(level);
        textupdate.displayText();
        yield return new WaitForSeconds(.5f);
        textupdate.hideText();
    }

    private void createNumberOfRounds(int currentRound , int maxRound)
    {
        if (currentRound <= maxRound)
        {
            // this is all temp
            LevelStruct tempLevel = levelArray[this.currentLevel];
            ThemeStruct tempTheme = themeArray[0];
            Debug.Log("new round");

            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();
            if (this.currentLevel > 1)
            {
                Debug.Log("Session Over");
            }
            this.currentLevel += 1;
        }
    }
    
    private void createTimeLimitRounds(float timeLeft)
    {

            // this is all temp
            LevelStruct tempLevel = levelArray[this.currentLevel];
            ThemeStruct tempTheme = themeArray[0];
            this.UpdateRound(tempLevel.ratio, tempTheme.returnRandomColor(), tempLevel.trialMax, tempTheme.dotShape, tempLevel.dotMax, tempLevel.spread);
            StartCoroutine(this.waitDisplay(tempLevel.levelNum, tempTheme.levelName));
            roundManager.roundStart();

        
    }
}
