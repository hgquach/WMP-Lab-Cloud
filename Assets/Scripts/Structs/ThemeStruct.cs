using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/*
 * this struct will contain data that would help determine the correct aesthetic changes for the rounds
 * these field will be filled out by reading in a theme file
 */
public struct ThemeStruct
{
    public string levelName,dotShape;
    public string backgroundImage;
    public List<Color> dotColorRange;
    // distractor opition 

    public ThemeStruct(string name , string shape , List<Color> colorRange, string bgImg = "none")
    {
        this.backgroundImage = bgImg;
        this.levelName = name;
        this.dotShape = shape;
        this.dotColorRange = colorRange;
    }

    public Color returnRandomColor()
    {
        return this.dotColorRange[Random.Range(0, this.dotColorRange.Count)];
    }

}
