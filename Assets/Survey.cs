using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Survey : MonoBehaviour {

    private ArrayList questionList;
    private int currentQuestion;

    // ui buttons and usch
    public GameObject LikertScale;
    public GameObject EffortScale;
    public GameObject DifficultyScale; 

    private ToggleGroup
    void Awake()
    {
        questionList.Add("How Much Did You Enjoy This Game");
        questionList.Add("How Easy Was The Task");
        questionList.Add("How Hard Did You Try To Do Your Best");
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
