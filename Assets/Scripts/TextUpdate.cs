using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextUpdate : MonoBehaviour {


	private string baseText ="Level ";
	private TextMesh textMesh; 

	void Awake()
	{
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
	}
}
