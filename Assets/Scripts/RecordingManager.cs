using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    private SessionManager sessionmanager;
    private RoundManagerTest roundmanager;
    private float trialStartTime;
    private float trialEndTime; 

    void Awake()
    {
        sessionmanager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        roundmanager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManagerTest>();
    }
    public void StartRecording()
    {
        this.trialStartTime = Time.time;
        GameData.gamedata.trialData.RoundNumber = sessionmanager.getCurrentRound();
        GameData.gamedata.trialData.Level = sessionmanager.getCurrentLevel();
        //not sure how theme selection works 
        //GameData.gamedata.trialData.Theme = sessionmanager.gettheme()
        GameData.gamedata.trialData.ScreenReso = Screen.currentResolution.ToString();
        GameData.gamedata.trialData.Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        GameData.gamedata.trialData.Dot = roundmanager.dotSprite;
        GameData.gamedata.trialData.Response = roundmanager.userChoice.ToString();
        GameData.gamedata.trialData.sizeLeftCloud = roundmanager.dotPerSide.cloud1;
        GameData.gamedata.trialData.sizeRightCloud = roundmanager.dotPerSide.cloud2;
        GameData.gamedata.trialData.color = roundmanager.dotColor;
        GameData.gamedata.trialData.AndroidOSVersion = SystemInfo.operatingSystem;
        GameData.gamedata.trialData.GameVersion = Application.version;
    }

    public void StopRecording()
    {
        this.trialEndTime = Time.time;
        GameData.gamedata.trialData.accuracy = roundmanager.checkAnswer(roundmanager.CorrectAnswer,roundmanager.userChoice).ToString();
        GameData.gamedata.trialData.ReactionTime = this.trialEndTime - this.trialStartTime;
        roundmanager.totalReactionTime += this.trialEndTime = this.trialStartTime;
        GameData.gamedata.trialData.TimeElasped = Time.time - sessionmanager.timeRoundstart;
        FileIO.writeTrialData(GameData.gamedata.trialData);
    }


}
