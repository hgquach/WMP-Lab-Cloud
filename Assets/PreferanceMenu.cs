using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PreferanceMenu : MonoBehaviour {
	private InputField Participant; 
	private InputField SessionField;
	private Toggle RoundField ;
	private Toggle TimedField ;
    private InputField LimitField;
    private Toggle SurveyField;
    private Button ApplyButton;
    private Button ExitButton;

	private string ParticipantId ;
	private string SessionId;
	private bool RoundToggle;
	private bool TimedToggle;
    private int roundLimit;
    private bool SurveyToggle;

    private Canvas MainMenuCanvas;
    void Awake()
    {
        MainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        Participant = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<InputField>();
		SessionField = gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<InputField>();
		RoundField = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<Toggle>();
		TimedField = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<Toggle>();
		LimitField = gameObject.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(2).gameObject.GetComponent<InputField>();
        SurveyField = gameObject.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).gameObject.GetComponent<Toggle>();
        ApplyButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).gameObject.GetComponent<Button>();
        ExitButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<Button>();
    }

    void Start()
    {	
		SessionField.onEndEdit.AddListener(onUpdateSession);
		Participant.onEndEdit.AddListener(onUpdateID);
        LimitField.onEndEdit.AddListener(onLimitUpdate);
		RoundField.onValueChanged.AddListener(onUpdateRound);
		TimedField.onValueChanged.AddListener(onUpdateTimed);
        SurveyField.onValueChanged.AddListener(onUpdateSurveyQuestion);
        ApplyButton.onClick.AddListener(onApply);
        ExitButton.onClick.AddListener(onExit);
    }
	
	private void onUpdateID(string arg0)
	{
		this.ParticipantId = arg0;
		Debug.Log(arg0);
	}
	
	private void onUpdateSession(string arg0)
	{
		this.SessionId = arg0;
		Debug.Log(arg0);
	}
	
	private void onUpdateRound(bool value)
	{

		this.RoundToggle = value; 
		this.TimedToggle = false;
		
		Debug.Log("TimedToggle: "+TimedToggle);
		Debug.Log("RoundToggle: "+RoundToggle);
	}
	
	private void onUpdateTimed(bool value)
	{
		this.TimedToggle = value;
		this.RoundToggle = false;
		
		Debug.Log("TimedToggle: "+TimedToggle);
		Debug.Log("RoundToggle: "+RoundToggle);

	}
    private void onLimitUpdate(string arg0)
    {
        this.roundLimit = int.Parse(arg0);
    }
    private void onUpdateSurveyQuestion(bool value)
    {
        this.SurveyToggle= value;
    }

    private void onApply()
    {
        GameData.gamedata.trialData.Session = this.SessionId;
        GameData.gamedata.trialData.PartcipantId = this.ParticipantId;
        if(this.TimedToggle)
        {
            GameData.gamedata.trialData.isRoundTimed = true;
        }
        GameData.gamedata.trialData.RoundLimit = this.roundLimit;
        Debug.Log(GameData.gamedata.trialData.ToString());
    }
    private void onExit()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        MainMenuCanvas.enabled = true;
        
    }
}
