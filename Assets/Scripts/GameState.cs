using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	public static int currentLevel = 1;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	static public void incCurrentLevel()
	{
		currentLevel += 1;
	}

	static public int getCurrentLevel()
	{
		return currentLevel;
	}
}
