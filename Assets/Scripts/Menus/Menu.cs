using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{

    private bool TL, TR, BL, BR;
    private Canvas MainMenuCanvas;
    private Canvas PreferenceMenu;
    public Text errorMessage;

    public Button PlayButton;
    public Button DemoButton;

    void Awake()
    {
        MainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        PreferenceMenu = GameObject.FindGameObjectWithTag("PreferenceMenu").GetComponent<Canvas>();
    }


    void Update()
    {
       //Debug.Log(TL.ToString() + TR.ToString() + BL.ToString() + BR.ToString());
        if(cornersTouched())
        {
            MainMenuCanvas.enabled = false;
            PreferenceMenu.enabled = true;
            //Debug.Log("Secret Menu should open");
        }
    }
    public void playButton()
    {
       // Debug.Log(GameData.gamedata.haveLevels.ToString() + GameData.gamedata.haveThemes.ToString());
        if (GameData.gamedata.trialData.PartcipantId != null && GameData.gamedata.haveLevels && GameData.gamedata.haveThemes)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            GameData.gamedata.isDemo = false;
            FileIO.changePreferenceToFilePath(FileIO.FILE_PATH_TO_MAIN_PREFERENCE);
            FileIO.changeParticipantToFilePath(FileIO.returnPathWayToCurrentParticipantFile());
            GameData.gamedata.currentParticipant.SessionNumber += 1;
            GameData.gamedata.SessionTimeStartStamp =  System.DateTime.Now.ToString("h-mm-ss");

        }
        else
        {
            StartCoroutine(this.displayError("There is Something Missing!!!",1f));
        }

    }
    public void demoButton()
    {
        if (GameData.gamedata.haveThemes && GameData.gamedata.haveLevels)
        {
            GameData.gamedata.isDemo = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            FileIO.changePreferenceToFilePath(FileIO.FILE_PATH_TO_DEMO_PREFERENCE);
            FileIO.changeParticipantToFilePath(FileIO.FILE_PATH_TO_DEMO_PARTICIPANTS);
        }
        else
        {
           StartCoroutine(this.displayError("There is Something Missing!!!",1f));

        }
    }
    public void TLPressedDown()
    {
        this.TL = true;
    }
    public void TRPressedDown()
    {
        this.TR = true;
    }
    public void BLPressedDown()
    {
        this.BL = true;
    }
    public void BRPressedDown()
    {
        this.BR = true;
    }

    public void TRPressedUp()
    {
        this.TL = false;    
    }
    public void TLPressedUp()
    {
        this.TR = false;    
    }
    public void BRPressedUp()
    {
        this.BL = false;    
    }
    public void BLPressedUp()
    {
        this.BR = false;    
    }

    private bool cornersTouched()
    {
        if(TL && TR && BR && BL)
        {
            return true;
        }

        return false;
    }

    private IEnumerator displayError(string msg , float delay)
    {
        this.errorMessage.text = msg;
        this.errorMessage.enabled = true;
        yield return new WaitForSeconds(delay);
        this.errorMessage.enabled = false;
    }
     public void  assignNamesToButton()
    {
        PlayButton.GetComponentInChildren<Text>().text = GameData.gamedata.translatedDictionary["START"];
        DemoButton.GetComponentInChildren<Text>().text = GameData.gamedata.translatedDictionary["DEMO"];
    }
}