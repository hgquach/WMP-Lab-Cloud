using System.Collections;
using System.Collections.Generic;

public struct PreferanceStruct
{
    public int SessionNumber, Level, RoundLimit;
    public string Theme;
    public bool IsTimed,SurveyQuestion;

    public PreferanceStruct(int session , int level , int roundLimit , string theme , bool timed, bool question)
    {
        this.SessionNumber = session;
        this.Level = level;
        this.RoundLimit = roundLimit;
        this.Theme = theme;
        this.IsTimed = timed;
        this.SurveyQuestion = question;
    }
    
    public override string ToString()
    {
        return this.SessionNumber.ToString()+"," + this.Level.ToString()+","
            +this.RoundLimit.ToString()+","+this.Theme+","+this.IsTimed.ToString()+","+this.SurveyQuestion.ToString();
    }
}
