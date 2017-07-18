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
    public bool haveCurrentPref;
	// Use this for initialization
    void Awake()
    {
        haveCurrentPref = false;
        if(gamedata == null)
        {
            DontDestroyOnLoad(gameObject);
            gamedata = this;
            /*check if current pref file exist and if it doenst create one in the pref folders
             * if it does exist then read the pref id and then update the game data
             * */
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
        FileIO.checkandCreateCurrentPrefFile();
    }


}

