using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Survey : MonoBehaviour {

    private ArrayList questionList;
    private int currentQuestion;

    // ui buttons and usch
    public GameObject LikertScale;
    public GameObject EffortScale;

    private GameObject likertscale;
    private GameObject effortscale;

    private Button doneButton;
    
    private string LikertResponse, EffortResponse;
    private bool questionAppear;

    void Awake()
    {
        doneButton = this.gameObject.GetComponentInChildren<Button>();
        this.questionList = new ArrayList();
        questionList.Add("How Much Did You Like The Game Today ?");
        questionList.Add("How Hard Did You Try Today?");
        this.questionAppear = false;
    }

    void Update()
    {
        Debug.Log("the current question is:  " + this.currentQuestion);
        Debug.Log("did a question appear:  " + this.questionAppear);
        switch(currentQuestion)
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
                Debug.Log(" Likert Reponse "+this.LikertResponse +" Effort Reponse "+ this.EffortResponse);
                FileIO.writeSurveyResponse(this.LikertResponse +","+ this.EffortResponse+ ","+ System.DateTime.UtcNow.ToString("HH:mm dd MMMM, yyyy"));
                SceneManager.LoadScene(0);
                break;
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

    private string toggleToResponse(int toggleNum , GameObject scale)
    {

        switch(toggleNum)
        {
            case 0:
                return null;
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
                return "Strongly Agree";
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
                return "Neutral";
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
                return "Strongly Disagree";
            default:
                return "no option";
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
    /*
     * This script will handle all event regarding the survey menu including recording user answer
     * writing to the output file , and update the question and correct UI prefab to answer the question by touch
     * 
     * The question updating system:
     * simple version is an arraylist / list of preloaded question (they dont seem to change at all ask Professor about it)
     * after answering a question the arraylist index will be increased by one and the text will be updated
     * 
     * int CurrentQuestion = 0 determines which question to load
     * once index has reached the length of the arraylist the game will transport the user to the main menu 
     * 
     * questionTextUpdate(string )
     *  get text object text script and change the text value
     * 
     * the answer to each question will be recoreded similar to the preference menu and only after the last question 
     * has been answer will the questions be recorded to the output file
     * 
     * 
     * in FileIO:
     * create a function to write out the reponse. should i create another struct to hold the answers? 
     * write out format each question is a on a sepearted line
     * {question},{answer/reponse},timestamp
     * */

}
