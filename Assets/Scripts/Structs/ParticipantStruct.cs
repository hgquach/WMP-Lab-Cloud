using System.Collections;
using System.Collections.Generic;


public struct ParticipantStruct{

    public int ID, PointTotal, SessionNumber, Level, Theme;

    public ParticipantStruct(int id ,int pointotal , int session , int level , int theme)
    {
        this.ID = id;
        this.PointTotal = pointotal;
        this.SessionNumber = session;
        this.Level = level;
        this.Theme = theme;
    }

    public override string  ToString()
    {
        //return string.Format("{0},{1},{2},{3},{4}", this.ID.ToString(), this.PointTotal.ToString(),
        //    this.SessionNumber.ToString(), this.Level.ToString(),this.Theme.ToString());
        return this.ID.ToString() + "," + this.PointTotal.ToString() + "," +
           this.SessionNumber.ToString() + "," + this.Level.ToString() + "," + this.Theme.ToString();
    }


}
