using System.Collections;
using System.Collections.Generic;

public struct PreferanceStruct
{
    public int RoundLimit,ThemeChange,MaxError,MinError, trialsPerRd;
    public bool IsTimed,SurveyQuestion;
    public string Language;
    public PreferanceStruct(int roundLimit ,int themechange ,int maxerror, int minerror, int trials, bool timed, bool question,string language)
    {
        this.RoundLimit = roundLimit;
        this.IsTimed = timed;
        this.SurveyQuestion = question;
        this.ThemeChange = themechange;
        this.MaxError = maxerror;
        this.MinError = minerror;
        this.trialsPerRd = trials;
        this.Language = language;
    }
    
    public override string ToString()
    {
        return this.RoundLimit.ToString()+","+this.IsTimed.ToString()+","+this.SurveyQuestion.ToString()
            + ","+this.ThemeChange.ToString()+","+this.MaxError.ToString()+","+this.MinError.ToString()
            +","+this.trialsPerRd.ToString()+","+this.Language;

    }
}
