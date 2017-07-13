using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUpdate : MonoBehaviour {


	private string baseLevel ="Level ";
	private TextMesh textMesh; 
	void Awake()
	{
		textMesh = this.GetComponent<TextMesh>();
        this.GetComponent<MeshRenderer>().enabled = false;
	}
	// Use this for initialization
    public void updateRoundTitle(int level , string themeName)
    {
		textMesh.text = baseLevel + level + "\n"+ themeName ;

    }

    public void updateFeedBack(float accuracy , float avgReactionTime)
    {
        textMesh.text = "Accuracy So Far: " + accuracy.ToString("F2") + "\n" +
            "Average Reaction Time: " + avgReactionTime.ToString("F2");
    }

    public void displayText()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    public void hideText()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
	
	// Update is called once per frame

}
