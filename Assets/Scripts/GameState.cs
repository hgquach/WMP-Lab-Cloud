using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	public static int currentLevel = 1;
	public static int choice;
	public static int correctAnswer;
	public static bool setResult;
	// Use this for initialization
	void Start () 
	{
		correctAnswer = 1;
		choice = 1;
		setResult = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isCorrectAnswer (choice, correctAnswer)) {
			setResult = true;
		} else {
			setResult = false;
		}
		
		
	}

	static public void incCurrentLevel()
	{
		currentLevel += 1;
	}

	static public int getCurrentLevel()
	{
		return currentLevel;
	}

	static public bool isCorrectAnswer(int userChoice , int correctAns)
	{
		if (userChoice == correctAns) {
			return true;
		}
		else 
			return false;
	}
}
