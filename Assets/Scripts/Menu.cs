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
    private Text errorMessage;

    void Awake()
    {
        MainMenuCanvas = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        PreferenceMenu = GameObject.FindGameObjectWithTag("PreferenceMenu").GetComponent<Canvas>();
        errorMessage = gameObject.transform.GetChild(2).GetComponent<Text>();
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
        Debug.Log(GameData.gamedata.trialData.PartcipantId);
        if (GameData.gamedata.trialData.PartcipantId != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            StartCoroutine(this.displayError("Need Partipant ID to Continue",1f));
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
}