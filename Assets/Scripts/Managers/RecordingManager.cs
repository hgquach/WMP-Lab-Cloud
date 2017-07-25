using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    private SessionManager sessionmanager;
    private RoundManager roundmanager;
    private float trialStartTime;
    private float trialEndTime; 

    void Awake()
    {
        sessionmanager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        roundmanager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
    }
    public void StartRecording()
    {
        this.trialStartTime = Time.time;
        if(GameData.gamedata.SessionPreferance.IsTimed)
        {
            GameData.gamedata.trialData.TimeLimit = GameData.gamedata.SessionPreferance.RoundLimit;
            GameData.gamedata.trialData.RoundLimit = 0;
        }
        else
        {

            GameData.gamedata.trialData.RoundLimit = GameData.gamedata.SessionPreferance.RoundLimit;
            GameData.gamedata.trialData.TimeLimit = 0;
        }
        GameData.gamedata.trialData.Session = GameData.gamedata.currentParticipant.SessionNumber;
        GameData.gamedata.trialData.RoundNumber = sessionmanager.getCurrentRound();
        GameData.gamedata.trialData.Level = sessionmanager.getCurrentLevel();
        GameData.gamedata.trialData.ScreenReso = Screen.currentResolution.ToString();
        GameData.gamedata.trialData.Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        GameData.gamedata.trialData.Time = System.DateTime.Now.ToString("h:mm:ss");
        GameData.gamedata.trialData.TrialNumber = roundmanager.getCurrentTrial();
        GameData.gamedata.trialData.Dot = roundmanager.dotSprite;
        GameData.gamedata.trialData.Theme = sessionmanager.roundTheme.levelName;
        GameData.gamedata.trialData.Response = roundmanager.userChoice.ToString();
        GameData.gamedata.trialData.sizeLeftCloud = roundmanager.dotPerSide.cloud1;
        GameData.gamedata.trialData.sizeRightCloud = roundmanager.dotPerSide.cloud2;
        GameData.gamedata.trialData.color = roundmanager.dotColor;
        GameData.gamedata.trialData.OSSystem = SystemInfo.operatingSystem;
        GameData.gamedata.trialData.GameVersion = Application.version;
        GameData.gamedata.trialData.Background = sessionmanager.roundTheme.backgroundImage;
    }

    public void StopRecording()
    {
        this.trialEndTime = Time.time;
        GameData.gamedata.trialData.accuracy = roundmanager.checkAnswer(roundmanager.CorrectAnswer,roundmanager.userChoice).ToString();
        if( roundmanager.checkAnswer(roundmanager.CorrectAnswer,roundmanager.userChoice))
        {
            GameData.gamedata.trialData.Point = sessionmanager.getCurrentLevel() ;
        }
        else
        {
            GameData.gamedata.trialData.Point = 0;
        }
        GameData.gamedata.trialData.Response = roundmanager.userChoice.ToString();
        GameData.gamedata.trialData.ReactionTime = this.trialEndTime - this.trialStartTime;
        roundmanager.totalReactionTime += this.trialEndTime - this.trialStartTime;
        GameData.gamedata.trialData.TimeElasped = this.trialEndTime - sessionmanager.timeRoundstart;
        FileIO.writeTrialData(GameData.gamedata.trialData, GameData.gamedata.SessionTimeStartStamp);

    }


}
