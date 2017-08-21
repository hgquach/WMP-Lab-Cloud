using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour {


    public Button goButton;
    private TextUpdate textupdate;
    public GameObject roundManagerObject;
    public GameObject sessionManagerObject;

    private RoundManager roundmanager;
    private SessionManager sessionmanager;
    private Canvas mainCanvas;
	// Use this for initialization
	void Awake()
    {
        this.Assignment();
	}

    void Update()
    {
       // Debug.Log(mainCanvas.enabled);
    }

    public void hideCanvas()
    {
        if(this.mainCanvas.enabled == true)
        {
            this.mainCanvas.enabled = false;
        }
    }

    public void showCanvas()
    {
       // Debug.Log("inside show canvas and trying to open the canvas");

        if(this.mainCanvas.enabled == false)
        {
            this.mainCanvas.enabled = true;
        }


    }

    public void assignContinueLevel()
    {
        goButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(continueLevel);
    }

    public void assignContinueAfterSmiley()
    {
        goButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(continueAfterSmiley);
    }

    public void assignContinueAfterFeedback()
    {
        goButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(continueAfterFeedBack);
    }

    public void assignContinueAfterCoinSummary()
    {
        goButton.GetComponentInChildren<Text>().text = GameData.gamedata.translatedDictionary["DONE"];
        goButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(continueAfterCoinSummary);
    }

    private void continueAfterSmiley()
    {
        this.hideCanvas();
        roundmanager.waitAndDelete();
        roundmanager.waitAndStartTrial();
    }

    private void continueLevel()
    {
        this.hideCanvas();
        textupdate.hideText();
        roundmanager.roundStart();

    }

    private void continueAfterFeedBack()
    {
        this.hideCanvas();
        textupdate.hideText();
        roundmanager.resetForNewRound();

    }

    private void continueAfterCoinSummary()
    {
        this.hideCanvas();
       // sessionmanager.isLevelDisplay = false;
        Debug.Log("loading main menu");
        SceneManager.LoadScene(0);
    }

    private void Assignment()
    {

        this.textupdate = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextUpdate>();
        this.roundmanager = roundManagerObject.GetComponent<RoundManager>();
        this.sessionmanager = sessionManagerObject.GetComponent<SessionManager>();
        this.mainCanvas = gameObject.GetComponent<Canvas>();
        this.goButton.GetComponentInChildren<Text>().text = GameData.gamedata.translatedDictionary["NEXT"];

    }
}
