using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUpdate : MonoBehaviour {


	private string baseText ="Level ";
	private TextMesh textMesh; 
	private GameState gamestate;
	void Awake()
	{
		gamestate = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameState> ();
		textMesh = this.GetComponent<TextMesh>();
	}
	// Use this for initialization
	void Start () 
	{
		textMesh.text = baseText + GameState.getCurrentLevel();
      
	}
	
	// Update is called once per frame
	void Update () 
	{
		gamestate.transistionStart();
	}
}
