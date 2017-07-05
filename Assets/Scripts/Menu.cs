using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private bool TL, TR, BL, BR;
    private Canvas MainMenuCanvas;
    private Canvas PreferenceMenu;

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
            Debug.Log("Secret Menu should open");
        }
    }
    public void playButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
}