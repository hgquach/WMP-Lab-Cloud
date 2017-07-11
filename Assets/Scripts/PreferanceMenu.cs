using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class PreferanceMenu : MonoBehaviour {
	private InputField Participant; 
	private Toggle RoundField ;
	private Toggle TimedField ;
    private InputField LimitField;
    private Toggle SurveyField;
    private Button ApplyButton;
    private Button ExitButton;

	private string ParticipantId ;
	private bool RoundToggle;
	private bool TimedToggle;
    private int roundLimit;
    private bool SurveyToggle;
    private string theme;
    private int sessionNumber;
    private int level;

    private Canvas MainMenuCanvas;
    void Awake()
    {
        this.Assignment();
    }

    void Start()
    {
        this.addListeners();
    }
	
	private void onUpdateID(string arg0)
	{
		this.ParticipantId = arg0;
        string sessionFilePath = Path.Combine(Application.persistentDataPath, Path.Combine(@"Preferences",this.ParticipantId+".csv"));
        if(File.Exists(sessionFilePath))
        {
            PreferanceStruct savedPref = FileIO.readPrefFile(sessionFilePath);
            if(savedPref.IsTimed)
            {
                TimedField.isOn = true;
                this.TimedToggle = true;
            }
            else
            {
                RoundField.isOn = true;
                this.RoundToggle = true;
            }

            if(savedPref.SurveyQuestion)
            {
                SurveyField.isOn = true;
                this.SurveyToggle = true;
            }
            this.level = savedPref.Level; 
            this.theme = savedPref.Theme;
            this.sessionNumber = savedPref.SessionNumber;
            LimitField.text = savedPref.RoundLimit.ToString();
            this.roundLimit = savedPref.RoundLimit;
        }
		//Debug.Log(arg0);
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
    private void onUpdateSurveyQuestion(bool value)
    {
        this.SurveyToggle= value;
    }

    private void onSave()
    {

        string sessionFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences",this.ParticipantId+".csv"));
        PreferanceStruct newPref = new PreferanceStruct();
        if (File.Exists(sessionFilePath))
        {
            newPref.SessionNumber = this.sessionNumber += 1;
            newPref.Theme = this.theme;
        }
        else
        {
            
            newPref.SessionNumber = 1;
            newPref.Theme = "none";
        }

        if(this.TimedToggle)
        {
            newPref.IsTimed = true;
        }
        newPref.RoundLimit = this.roundLimit;
        if(this.SurveyToggle)
        {
            newPref.SurveyQuestion = true;
        }
        GameData.gamedata.trialData.PartcipantId=this.ParticipantId;
        GameData.gamedata.trialData.Session=newPref.SessionNumber;
        GameData.gamedata.trialData.Level=newPref.Level;
        GameData.gamedata.trialData.Theme=newPref.Theme;
        GameData.gamedata.trialData.isRoundTimed=newPref.IsTimed;
        GameData.gamedata.trialData.RoundLimit = newPref.RoundLimit;
        GameData.gamedata.surveyToggled=newPref.SurveyQuestion;
        FileIO.createPrefFile(newPref, int.Parse(this.ParticipantId));
        gameObject.GetComponent<Canvas>().enabled = false;
        MainMenuCanvas.enabled = true;
    }
    private void onExit()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        MainMenuCanvas.enabled = true;
        
    }

    private void Assignment()
    {
        MainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        Participant = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<InputField>();
        gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<InputField>().text = "100";
		RoundField = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Toggle>();
		TimedField = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Toggle>();
		LimitField = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).gameObject.GetComponent<InputField>();
		gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).gameObject.GetComponent<InputField>().text = "100";
        SurveyField = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<Toggle>();
        ApplyButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<Button>();
        ExitButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<Button>();
    }
    
    private void addListeners()
    {

		Participant.onEndEdit.AddListener(onUpdateID);
        LimitField.onEndEdit.AddListener(onLimitUpdate);
		RoundField.onValueChanged.AddListener(onUpdateRound);
		TimedField.onValueChanged.AddListener(onUpdateTimed);
        SurveyField.onValueChanged.AddListener(onUpdateSurveyQuestion);
        ApplyButton.onClick.AddListener(onSave);
        ExitButton.onClick.AddListener(onExit);
    }

}

