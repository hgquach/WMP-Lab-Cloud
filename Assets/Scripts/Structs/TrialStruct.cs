using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TrialStruct
{
    public string TaskName, PartcipantId,Response,GameVersion,OSSystem,accuracy
        ,ScreenReso,Date,Time,Dot,Theme,Answer;
    public int RoundNumber, Level,Point, sizeLeftCloud , sizeRightCloud,RoundLimit,TimeLimit,Session,EffortReponse,DifficultyReponse,TrialNumber;
    public float ReactionTime,TimeElasped;
    public Color color;

    public TrialStruct( 
     string TaskName, string PartcipantId, int Session, string Theme,string Response,
     string GameVersion,string ossytem,string background,string Screen , string date,string time, string dot,string answer,
     int RoundNumber, int Level , int sizeLeftCloud , int sizeRightCloud,int eReponse,
     int dReponse, int trial,int point, int timelimit,
     float ReactionTime,float TimeElasped, string accuracy,
     Color color , bool roundTimed = false, int limit = 5)
    {
        this.TaskName = TaskName;
        this.PartcipantId = PartcipantId;
        this.Session = Session;
        this.Response = Response;
        this.GameVersion = GameVersion;
        this.ScreenReso = Screen;
        this.Date = date;
        this.Dot = dot;
        this.OSSystem = ossytem;
        this.RoundNumber = RoundNumber;
        this.Level = Level;
        this.Answer = answer;
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
        this.Theme = background;
    }

    public override string ToString()
    {
        string colorStringAdjusted = this.color.ToString();
        string strTimeLimit, strRoundLimit , strDReponse,strEReponse;

        strTimeLimit = (this.TimeLimit == 0 ) ? "n/a" : this.TimeLimit.ToString();
        strRoundLimit = (this.RoundLimit == 0) ? "n/a" : this.RoundLimit.ToString();
        strDReponse = (this.DifficultyReponse == 0) ? "n/a" : this.DifficultyReponse.ToString();
        strEReponse = (this.EffortReponse == 0) ? "n/a" : this.EffortReponse.ToString();

        colorStringAdjusted = colorStringAdjusted.Replace(",","-");
        string[] TrialStringArray = { this.TaskName,GameVersion,OSSystem,this.ScreenReso,this.PartcipantId
        ,this.Date,this.Time,this.Session.ToString(),this.Level.ToString(),this.RoundNumber.ToString(),this.TrialNumber.ToString()
        , this.accuracy.ToString() ,this.Answer, this.ReactionTime.ToString(),this.Point.ToString(),this.Response,this.sizeLeftCloud.ToString()
        ,this.sizeRightCloud.ToString(),this.Dot,this.Theme,strRoundLimit,strTimeLimit
        ,this.TimeElasped.ToString("F2"),strDReponse,strEReponse};

        return string.Join(",", TrialStringArray);
    }



}
