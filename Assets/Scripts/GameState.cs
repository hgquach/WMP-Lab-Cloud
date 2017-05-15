using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {
	public static int currentLevel = 0;
	public static int choice = 0;
	public static int correctAnswer = 1;
	public static bool setResult = false;
	public static Level[] levelArray; 
	// Use this for initialization
	void start()
	{
		levelArray[0] = new Level("Test", new RatioStruct(1,2) , 50 , Color.blue , 10 );
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

	static private bool isCorrectAnswer(int userChoice , int correctAns)
	{
		//Debug.Log ("isCorrectAnswerDebug: "+ (userChoice == correctAns));
		if (userChoice == correctAns) {
			setResult = true;
			return true;
		} else { 
			setResult = false;
			return false;
		}
	}
		
	public  IEnumerator waitAndTransistion()
	{
		yield return new WaitForSeconds(1);
		if (SceneManager.GetActiveScene ().buildIndex == 3) {
			SceneManager.LoadScene (1);	
		} else {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		}
			//print ("waitAndTransistion" + Time.time);
	}

	public void transistionStart(bool isNextLevel = false)
	{
		//print("starting "+Time.time);
		if (isNextLevel) {
			StartCoroutine (waitAndTransistion ());
		} else {
			StartCoroutine (waitAndTransistion ());
		}
		//print("done"+Time.time);
	}
}
