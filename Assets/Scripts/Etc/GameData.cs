using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameData : MonoBehaviour {

    public static GameData gamedata;
    // might redo the sessionsTheme becuase
    public ArrayList sessionTheme;
    public List<LevelStruct> sessionLevels;
    public TrialStruct trialData;
    public PreferanceStruct SessionPreferance;
    public ParticipantStruct currentParticipant;
    public bool surveyToggled,isDemo,haveCurrentParticipant,haveLevels,haveThemes;
    public Dictionary<string, string> translatedDictionary;
    public string SessionTimeStartStamp;
	// Use this for initialization
    void Awake()
    {
        if(gamedata == null)
        {
            DontDestroyOnLoad(gameObject);
            gamedata = this;

            GameData.gamedata.haveLevels = false;
            GameData.gamedata.haveThemes = false;
            //Dictionary<string,string> tempDict = FileIO.readStreamingAssetDirectory();
            //FileIO.moveStreamingAssets(tempDict);
            this.checkAndCreateFolders();
            ////  FileIO.populateThemeFolder();
            ////FileIO.populateLevelFolder();
            this.sessionLevels = FileIO.returnLevelInFolder();
            this.sessionTheme = FileIO.searchandFilterForThemeFolder();
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
        FileIO.checkAndCreateParticipantFolder();
        FileIO.checkAndCreateCurrentParticipantFile();
        FileIO.checkAndCreateLanguageFolder();
        FileIO.checkandCreateThemeFolder();
        FileIO.checkedAndCreateLevelFolder();
    }


}

