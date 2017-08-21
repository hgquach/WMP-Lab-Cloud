using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelStruct
{
    /*
     * This sturcture is used to hold all level parameters. That will be access to generate the trials. These structures will be filled
     * via reading a text file that contains all the levels
     */

    public int levelNum, dotMax;
    public RatioStruct ratio;
    public float spread;

    public LevelStruct(int level , int max , float spread , RatioStruct Ratio)
    {
        this.levelNum = level;
        this.dotMax = max;
        this.spread = spread;
        this.ratio = Ratio;
    }
    

}

