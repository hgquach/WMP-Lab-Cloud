using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Linq;

public class PreferanceMenu : MonoBehaviour {
#region class public and private variables 
    public InputField Participant, LimitField , ThemeChangeField, MaxError,MinError, TrialPerRd;
    public Toggle RoundField , TimedField, SurveyField;
    public Button ApplyButton,ExitButton,PopulateButton;
    public Dropdown languageDropDown;

    private string ParticipantId,selectedLanguage;
    private bool RoundToggle, TimedToggle, SurveyToggle;
    private int roundLimit,themeChange,maxE,minE, trialPerRd;
    private List<string> avaiableLanguage;
    private Canvas MainMenuCanvas;
#endregion

    void Start()
    {
        this.Assignment();
        this.addListeners();
    }
#region button functions
    private void onUpdateID(string arg0)
	{
		this.ParticipantId = arg0;

        //string sessionFilePath = Path.Combine(Application.persistentDataPath, Path.Combine(@"Preferences",this.ParticipantId+".csv"));
        //if(File.Exists(sessionFilePath))
        //{
        //    PreferanceStruct savedPref = FileIO.readPrefFile(sessionFilePath);
        //    if(savedPref.IsTimed)
        //    {
        //        TimedField.isOn = true;
        //        this.TimedToggle = true;
        //    }
        //    else
        //    {
        //        RoundField.isOn = true;
        //        this.RoundToggle = true;
        //    }

        //    if(savedPref.SurveyQuestion)
        //    {
        //        SurveyField.isOn = true;
        //        this.SurveyToggle = true;
        //    }
        //    this.level = savedPref.Level; 
        //    this.theme = savedPref.Theme;
        //    this.sessionNumber = savedPref.SessionNumber;
        //    LimitField.text = savedPref.RoundLimit.ToString();
        //    this.roundLimit = savedPref.RoundLimit;
        //}
		//Debug.Log(arg0);
	}

    private void onThemeChange(string arg0)
    {
        int input;
        if (int.TryParse(arg0, out input))
        {
            this.themeChange = input;
        }
    }

    private void onMaxEChange(string arg0)
    {
        this.maxE = int.Parse(arg0);
    }

    private void onMinEChange(string arg0)
    {
        this.minE = int.Parse(arg0);
    }

    private void onUpdateRound(bool value)
	{

		this.RoundToggle = value; 
		this.TimedToggle = false;
		

	}

    private void onUpdateTimed(bool value)
	{
		this.TimedToggle = value;
		this.RoundToggle = false;
		


	}

    private void onLimitUpdate(string arg0)
    {
        this.roundLimit = int.Parse(arg0);
    }

    private void onUpdatetrialPerRd(string arg0)
    {
        this.trialPerRd = int.Parse(arg0);
    }

    private void onUpdateSurveyQuestion(bool value)
    {
        this.SurveyToggle= value;
    }

    private void onUpdateLanguage(int choice)
    {
        this.selectedLanguage = avaiableLanguage[choice];
        Debug.Log("current language: " + this.selectedLanguage);
    }

    private void onSave()
    {

       // string preferanceFilePathway = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences","Preference"+".csv"));
        //Debug.Log("this is the current participant id: "+this.ParticipantId); 
        PreferanceStruct newPref = new PreferanceStruct();
        string currentParticipantID = FileIO.readCurrentParticipant();
        if (!this.ParticipantId.Equals(currentParticipantID) || currentParticipantID == "")
        {
            string participantPathway = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", this.ParticipantId+".txt"));
            if(File.Exists(participantPathway))
            {
                GameData.gamedata.currentParticipant = FileIO.readParticipantFile(participantPathway);
            }
            else
            {
                ParticipantStruct participant = new ParticipantStruct(int.Parse(this.ParticipantId),0,0,0,0);
                FileIO.writeParticipantFile(participant);
                GameData.gamedata.currentParticipant = participant; 

            }

            FileIO.writeCurrentParticipant(this.ParticipantId);
        }

        if(this.TimedToggle)
        {
            newPref.IsTimed = true;
        }

        newPref.RoundLimit = this.roundLimit;
        newPref.ThemeChange = this.themeChange;
        newPref.MinError = this.minE;
        newPref.MaxError = this.maxE;
        newPref.trialsPerRd = this.trialPerRd;
        newPref.Language = this.selectedLanguage;
        if(this.SurveyToggle)
        {
            newPref.SurveyQuestion = true;
        }

        this.updateGameDataPref(newPref, this.ParticipantId);

        FileIO.writePrefFile(newPref);
        GameData.gamedata.translatedDictionary = FileIO.readLanguageFile(newPref.Language);
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Menu>().assignNamesToButton();
    }

    private void onExit()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        MainMenuCanvas.enabled = true;
        MainMenuCanvas.GetComponent<Menu>().updateCurrentParticipantID();

        
    }

    private void onPopulate()
    {
        Dictionary<string, List<string>> tempDict = FileIO.readStreamingAssetDirectory();
        FileIO.moveStreamingAssets(tempDict);
        GameData.gamedata.sessionLevels = FileIO.returnLevelInFolder();
        GameData.gamedata.sessionTheme = FileIO.searchandFilterForThemeFolder();
        GameData.gamedata.translatedDictionary = FileIO.readLanguageFile("English");
        Assignment();
        

    }
    #endregion

    private void Assignment()
    {

        MainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        //Debug.Log("do we have a current participant?: "+GameData.gamedata.haveCurrentParticipant);
        this.avaiableLanguage = FileIO.searchandFilterForLanguageFile();
        this.avaiableLanguage.Insert(0, "Select Language...");
        this.avaiableLanguage = this.avaiableLanguage.Distinct().ToList();

        this.languageDropDown.ClearOptions();
        this.languageDropDown.AddOptions(this.avaiableLanguage);
       if(!GameData.gamedata.haveCurrentParticipant)
        {
            this.Participant.text = "999";
        }
        else
        {
            //Debug.Log(GameData.gamedata.currentParticipant.ID);
            this.Participant.text = GameData.gamedata.currentParticipant.ID.ToString();
            this.ParticipantId = GameData.gamedata.currentParticipant.ID.ToString();
        }

        PreferanceStruct savedPref = GameData.gamedata.SessionPreferance;
        if (savedPref.IsTimed)
        {
            TimedField.isOn = true;
            this.TimedToggle = true;
        }
        else
        {
            RoundField.isOn = true;
            this.RoundToggle = true;
        }

        if (savedPref.SurveyQuestion)
        {
            SurveyField.isOn = true;
            this.SurveyToggle = true;
        }

        this.LimitField.text = savedPref.RoundLimit.ToString();
        this.themeChange = savedPref.ThemeChange;
        this.maxE =  savedPref.MaxError;
        this.minE = savedPref.MinError;
        this.trialPerRd = savedPref.trialsPerRd;
        this.roundLimit = savedPref.RoundLimit;
        this.selectedLanguage = savedPref.Language;
        this.languageDropDown.value = this.avaiableLanguage.IndexOf(savedPref.Language);
        LimitField.text = savedPref.RoundLimit.ToString();
        this.ThemeChangeField.text = savedPref.ThemeChange.ToString();
        this.MaxError.text = savedPref.MaxError.ToString();
        this.MinError.text = savedPref.MinError.ToString();
        this.TrialPerRd.text = savedPref.trialsPerRd.ToString();
        GameData.gamedata.translatedDictionary = FileIO.readLanguageFile(savedPref.Language);
        //foreach(KeyValuePair<string ,string> pair in GameData.gamedata.translatedDictionary)
        //{
        //    Debug.Log(pair.ToString());
        //}
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Menu>().assignNamesToButton();

        
    }
    
    private void addListeners()
    {

		Participant.onEndEdit.AddListener(onUpdateID);
        LimitField.onEndEdit.AddListener(onLimitUpdate);
		RoundField.onValueChanged.AddListener(onUpdateRound);
		TimedField.onValueChanged.AddListener(onUpdateTimed);
        SurveyField.onValueChanged.AddListener(onUpdateSurveyQuestion);
        ThemeChangeField.onValueChanged.AddListener(onThemeChange);
        MaxError.onValueChanged.AddListener(onMaxEChange);
        MinError.onValueChanged.AddListener(onMinEChange);
        TrialPerRd.onValueChanged.AddListener(onUpdatetrialPerRd);
        ApplyButton.onClick.AddListener(onSave);
        ExitButton.onClick.AddListener(onExit);
        PopulateButton.onClick.AddListener(onPopulate);
        this.languageDropDown.onValueChanged.AddListener(onUpdateLanguage);
    }

    private void updateGameDataPref(PreferanceStruct newPref , string prefID)
    {
        GameData.gamedata.trialData.PartcipantId= prefID;
        GameData.gamedata.trialData.RoundLimit = newPref.RoundLimit;
        GameData.gamedata.surveyToggled = newPref.SurveyQuestion;
        GameData.gamedata.SessionPreferance = newPref;
    }
}

