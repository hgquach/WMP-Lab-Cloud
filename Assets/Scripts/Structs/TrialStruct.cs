using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TrialStruct
{
    public string TaskName, PartcipantId, Session, Theme,Response,GameVersion,AndroidOSVersion;
    public int RoundNumber, Level , sizeLeftCloud , sizeRightCloud,Seperation;
    public float ReactionTime,TimeElasped,accuracy;
    public Color color;

    public TrialStruct( string TaskName, string PartcipantId, string Session, string Theme,string Response,string GameVersion,string AndroidOSVersion,
     int RoundNumber, int Level , int sizeLeftCloud , int sizeRightCloud,int Seperation,
     float ReactionTime,float TimeElasped, float accuracy,
     Color color)
    {
        this.TaskName = TaskName;
        this.PartcipantId = PartcipantId;
        this.Session = Session;
        this.Theme = Theme;
        this.Response = Response;
        this.GameVersion = GameVersion;
        this.AndroidOSVersion = AndroidOSVersion;
        this.RoundNumber = RoundNumber;
        this.Level = Level;
        this.sizeLeftCloud = sizeLeftCloud;
        this.sizeRightCloud = sizeRightCloud;
        this.Seperation = Seperation;
        this.ReactionTime = ReactionTime;
        this.TimeElasped = TimeElasped;
        this.accuracy = accuracy;
        this.color = color;   
    }

    public override string ToString()
    {
        return TaskName + PartcipantId + Session + RoundNumber.ToString() + Level.ToString() +
        accuracy.ToString() + ReactionTime.ToString() + Response + sizeLeftCloud.ToString() + sizeRightCloud.ToString() +
        color.ToString() + Seperation.ToString() + AndroidOSVersion + GameVersion + TimeElasped.ToString();
    }

}
