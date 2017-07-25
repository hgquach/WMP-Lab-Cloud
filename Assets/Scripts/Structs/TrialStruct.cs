using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TrialStruct
{
    public string TaskName, PartcipantId, Theme,Response,GameVersion,OSSystem,accuracy
        ,ScreenReso,Date,Time,Dot,Background;
    public int RoundNumber, Level,Point, sizeLeftCloud , sizeRightCloud,RoundLimit,TimeLimit,Session,EffortReponse,DifficultyReponse,TrialNumber;
    public float ReactionTime,TimeElasped;
    public Color color;

    public TrialStruct( string TaskName, string PartcipantId, int Session, string Theme,string Response,
        string GameVersion,string ossytem,string background,string Screen , string date,string time, string dot,
     int RoundNumber, int Level , int sizeLeftCloud , int sizeRightCloud,int eReponse,
     int dReponse, int trial,int point, int timelimit,
     float ReactionTime,float TimeElasped, string accuracy,
     Color color , bool roundTimed = false, int limit = 5)
    {
        this.TaskName = TaskName;
        this.PartcipantId = PartcipantId;
        this.Session = Session;
        this.Theme = Theme;
        this.Response = Response;
        this.GameVersion = GameVersion;
        this.ScreenReso = Screen;
        this.Date = date;
        this.Dot = dot;
        this.OSSystem = ossytem;
        this.RoundNumber = RoundNumber;
        this.Level = Level;
        this.sizeLeftCloud = sizeLeftCloud;
        this.sizeRightCloud = sizeRightCloud;
        this.ReactionTime = ReactionTime;
        this.TimeElasped = TimeElasped;
        this.accuracy = accuracy;
        this.color = color;
        this.RoundLimit = limit;
        this.Time = time;
        this.DifficultyReponse = dReponse;
        this.EffortReponse = eReponse;
        this.TrialNumber = trial;
        this.Point = point;
        this.TimeLimit = timelimit;
        this.Background = background;
    }

    public override string ToString()
    {
        string colorStringAdjusted = this.color.ToString();
        colorStringAdjusted = colorStringAdjusted.Replace(",","-");
        string[] TrialStringArray = { this.TaskName,GameVersion,OSSystem,this.ScreenReso,this.PartcipantId
        ,this.Date,this.Time,this.Session.ToString(),this.Level.ToString(),this.RoundNumber.ToString(),this.TrialNumber.ToString()
        , this.accuracy.ToString() , this.ReactionTime.ToString(),this.Point.ToString(),this.Response,this.sizeLeftCloud.ToString()
        ,this.sizeRightCloud.ToString(),this.Dot,this.Background,this.RoundLimit.ToString(),this.TimeLimit.ToString()
        ,this.TimeElasped.ToString("F2"),this.DifficultyReponse.ToString(),this.EffortReponse.ToString()};

        return string.Join(",", TrialStringArray);
    }



}
