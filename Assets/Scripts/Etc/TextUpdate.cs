using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour {


    private string baseLevel;
    private Text informationText;
	void Awake()
	{
        informationText = this.gameObject.GetComponentInChildren<Text>();
        baseLevel = GameData.gamedata.translatedDictionary["LEVEL"];
	}
	// Use this for initialization
    public void updateRoundTitle(int level)
    {
		informationText.text = baseLevel + level;

    }

    public void updateFeedBack(float accuracy , float avgReactionTime , int points , int bonusPoints = 0)
    {
        string percentCorrect, averageReactionTime, pointsEarned,extraPoints;
        if(bonusPoints == 0)
        {
            percentCorrect = GameData.gamedata.translatedDictionary["correct"].Replace("xxx",((accuracy * 100).ToString("F0")));
            averageReactionTime = GameData.gamedata.translatedDictionary["Average Reaction Time"].Replace("xxx",avgReactionTime.ToString("F1"));
            pointsEarned = GameData.gamedata.translatedDictionary["Points"].Replace("xxx",points.ToString());
            this.informationText.text = percentCorrect + "\n" + averageReactionTime + "\n" + pointsEarned;
        }
        else
        {
            percentCorrect = GameData.gamedata.translatedDictionary["correct"].Replace("xxx",((accuracy * 100).ToString("F0")));
            averageReactionTime = GameData.gamedata.translatedDictionary["Average Reaction Time"].Replace("xxx",avgReactionTime.ToString("F1"));
            pointsEarned = GameData.gamedata.translatedDictionary["Points"].Replace("xxx",points.ToString());
            extraPoints = GameData.gamedata.translatedDictionary["Perfect Score Bonus"].Replace("xxx" , bonusPoints.ToString());
            this.informationText.text = percentCorrect + "\n" + averageReactionTime +"\n"+ extraPoints+"\n" + pointsEarned;


        }

    }

    public void updateParticipantsTotalPoints(int sessiontotal , int grandtotal)
    {
        string pointstoday = GameData.gamedata.translatedDictionary["PointsToday"];
        string pointsSoFar = GameData.gamedata.translatedDictionary["PointsSoFar"];
        pointstoday = pointstoday.Replace("xxx" , sessiontotal.ToString());
        pointsSoFar = pointsSoFar.Replace("xxx", grandtotal.ToString());


         this.informationText.text= pointstoday + "\n" + pointsSoFar ; 
    }
    public void displayText()
    {
        this.GetComponent<Canvas>().enabled = true;
    }

    public void hideText()
    {
        this.GetComponent<Canvas>().enabled = false;
    }
	
    public void updateTextColor(Color color)
    {
        this.informationText.color = color;
    }
	// Update is called once per frame

}
