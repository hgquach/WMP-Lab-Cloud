﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TrialStruct
{
    public string TaskName, PartcipantId, Theme,Response,GameVersion,AndroidOSVersion,accuracy
        ,ScreenReso,Date,Dot;
    public int RoundNumber, Level , sizeLeftCloud , sizeRightCloud,RoundLimit,Session;
    public float ReactionTime,TimeElasped;
    public Color color;
    public bool isRoundTimed;

    public TrialStruct( string TaskName, string PartcipantId, int Session, string Theme,string Response,string GameVersion,
        string AndroidOSVersion,string Screen , string date,string dot,
     int RoundNumber, int Level , int sizeLeftCloud , int sizeRightCloud,
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
        this.AndroidOSVersion = AndroidOSVersion;
        this.RoundNumber = RoundNumber;
        this.Level = Level;
        this.sizeLeftCloud = sizeLeftCloud;
        this.sizeRightCloud = sizeRightCloud;
        this.ReactionTime = ReactionTime;
        this.TimeElasped = TimeElasped;
        this.accuracy = accuracy;
        this.color = color;
        this.isRoundTimed = roundTimed;
        this.RoundLimit = limit;
    }

    public override string ToString()
    {
        string colorStringAdjusted = this.color.ToString();
        colorStringAdjusted = colorStringAdjusted.Replace(",","-");
        Debug.Log(colorStringAdjusted);
        return TaskName +","+ PartcipantId +","+ Session.ToString()+","+ScreenReso+","+isRoundTimed+","+RoundLimit+","+Date+","+Dot+"," + RoundNumber.ToString()+"," + Level.ToString()+"," +
        accuracy +","+ ReactionTime.ToString()+"," + Response+"," + sizeLeftCloud.ToString()+"," + sizeRightCloud.ToString()+"," +
        colorStringAdjusted+"," + AndroidOSVersion+"," + GameVersion+"," + TimeElasped.ToString()+"\r\n";
    }



}
