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
    public bool surveyToggled,isDemo,haveCurrentParticipant;
    public Dictionary<string, string> translatedDictionary;
    public string SessionTimeStartStamp;
	// Use this for initialization
    void Awake()
    {
        if(gamedata == null)
        {
            DontDestroyOnLoad(gameObject);
            gamedata = this;

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
        FileIO.checkAndCreateParticipantFolder();
        FileIO.checkAndCreateCurrentParticipantFile();
        FileIO.checkAndCreateLanguageFolder();
    }


}

