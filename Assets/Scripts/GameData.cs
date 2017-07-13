using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameData : MonoBehaviour {

    public static GameData gamedata;
    public Dictionary<string,ThemeStruct> sessionTheme;
    public List<LevelStruct> sessionLevels;
    public TrialStruct trialData;
    public PreferanceStruct playerPref;
    public bool surveyToggled;
    public bool isDemo;
	// Use this for initialization
    void Awake()
    {

        if(gamedata == null)
        {
            DontDestroyOnLoad(gameObject);
            gamedata = this;
            // create a function that would check if a preferences folder exist on the device in the persistance data path
            // if the file doesnt exist create one
            // do the same for output files.
            this.checkAndCreateFolders();
            this.sessionLevels = FileIO.readLevelFile("TestLevel1");
            this.sessionTheme = FileIO.readThemeFile("TestTheme");
            trialData.TaskName = "Cloud";

        }
        else if(gamedata != this)
        {
            Destroy(gameObject);
        }

    }

    private void checkAndCreateFolders()
    {
        FileIO.checkAndCreateTrialFolder();
        FileIO.checkAndCreatePrefFolder();
    }


}

