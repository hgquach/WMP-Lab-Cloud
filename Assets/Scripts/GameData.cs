using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData gamedata;
    public Dictionary<string,ThemeStruct> sessionTheme;
    public List<LevelStruct> sessionLevels;
    public TrialStruct trialData;
    public bool surveyToggled;
	// Use this for initialization
    void Awake()
    {
        if(gamedata == null)
        {
            DontDestroyOnLoad(gameObject);
            gamedata = this;

            this.sessionLevels = FileIO.readLevelFile("TestLevel1");
            this.sessionTheme = FileIO.readThemeFile("TestTheme");
            trialData.TaskName = "Cloud";

        }
        else if(gamedata != this)
        {
            Destroy(gameObject);
        }

    }



}

