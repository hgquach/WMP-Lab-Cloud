using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUpdate : MonoBehaviour {


	private string baseLevel ="Level ";
    private string baseTheme = "";
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

    public void displayTitle()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    public void hideTitle()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
	
	// Update is called once per frame

}
