using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Survey : MonoBehaviour {

    private ArrayList questionList;
    private int currentQuestion;

    public GameObject sessionmanager;

    // ui buttons and usch
    public GameObject LikertScale;
    public GameObject EffortScale;

    private GameObject likertscale;
    private GameObject effortscale;

    private Button doneButton;
    
    private int LikertResponse, EffortResponse;
    private bool questionAppear;

    void Awake()
    {

        doneButton = this.gameObject.GetComponentInChildren<Button>();
        doneButton.GetComponentInChildren<Text>().text = GameData.gamedata.translatedDictionary["DONE"];
        this.questionList = new ArrayList();
        questionList.Add(GameData.gamedata.translatedDictionary["Question1"]);
        questionList.Add(GameData.gamedata.translatedDictionary["Question2"]);
        this.questionAppear = false;
    }

    void Update()
    {
        if (this.gameObject.GetComponent<Canvas>().enabled == true)
        {

            switch (currentQuestion)
            {
                case 0:
                    if (!this.questionAppear)
                    {
                        this.updateQuestionString(currentQuestion);
                        CreateLikertScale();
                        this.questionAppear = true;
                    }
                    break;
                case 1:
                    if (!this.questionAppear)
                    {
                        this.updateQuestionString(currentQuestion);
                        CreateEffortScale();
                        this.questionAppear = true;
                    }
                    break;
                case 2:
                    GameData.gamedata.trialData.DifficultyReponse = this.LikertResponse;
                    GameData.gamedata.trialData.EffortReponse = this.EffortResponse;
                    FileIO.writeTrialData(GameData.gamedata.trialData, GameData.gamedata.SessionTimeStartStamp);
                    this.gameObject.GetComponent<Canvas>().enabled = false;
                    this.sessionmanager.GetComponent<SessionManager>().runPointSummary();
                    break;
            }
        }

    }
    private void CreateLikertScale()
    {
        this.likertscale = Instantiate(LikertScale) as GameObject;
        likertscale.transform.SetParent(this.gameObject.transform,false);
        likertscale.name = "likertscale";
        doneButton.onClick.RemoveAllListeners();
        doneButton.onClick.AddListener(returnLikertScaleChoice);
        
    }

    private void removeLikertScale()
    {
        this.likertscale.transform.SetParent(null);
        Destroy(this.likertscale);
    }

    private void removeEffortScale()
    {
        this.effortscale.transform.SetParent(null);
        Destroy(this.effortscale);
    }

    private void returnLikertScaleChoice()
    {
        int toggleNumber = 0;
        if(this.likertscale.GetComponent<ToggleGroup>().ActiveToggles() != null && this.currentQuestion ==0)
        {
            for(int i = 0; i < this.likertscale.transform.childCount; i++)
            {
                if(this.likertscale.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    toggleNumber = (i+1);
                    this.currentQuestion += 1;
                }
            }
        }
  
        this.LikertResponse = this.toggleToResponse(toggleNumber, this.likertscale);

    }

    private int toggleToResponse(int toggleNum , GameObject scale)
    {

        switch(toggleNum)
        {
            case 0:
                return 0;
            case 1:
                if(scale.name == "likertscale")
                {
                    this.removeLikertScale();
                }
                else if (scale.name == "effortscale")
                {
                    this.removeEffortScale();
                }
                this.questionAppear = false;
                return 1;
            case 2:
                if(scale.name == "likertscale")
                {
                    this.removeLikertScale();
                }
                else if (scale.name == "effortscale")
                {
                    this.removeEffortScale();
                }
                this.questionAppear = false;
                return 2;
            case 3:
                if(scale.name == "likertscale")
                {
                    this.removeLikertScale();
                }
                else if (scale.name == "effortscale")
                {
                    this.removeEffortScale();
                }
                this.questionAppear = false;
                return 3;
            default:
                return 0;
        }
    }

    private void returnEffortScaleChoice()
    {
        int toggleNumber = 0;
        if(this.effortscale.GetComponent<ToggleGroup>().ActiveToggles() != null && this.currentQuestion == 1)
        {
            for(int i = 0; i < this.effortscale.transform.childCount; i++)
            {
                if(this.effortscale.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    toggleNumber = (i+1);
                    this.currentQuestion += 1;
                }
            }
        }
  
        this.EffortResponse = this.toggleToResponse(toggleNumber,this.effortscale);



    }
    private void CreateEffortScale()
    {
        this.effortscale = Instantiate(EffortScale) as GameObject;
        effortscale.transform.SetParent(this.gameObject.transform, false);
        effortscale.name = "effortscale";
        doneButton.onClick.RemoveAllListeners();
        this.doneButton.onClick.AddListener(this.returnEffortScaleChoice);
    }
    
    private void updateQuestionString(int questionNumber)
    {
        this.gameObject.GetComponentInChildren<Text>().text = (string)this.questionList[questionNumber];
    }


}
