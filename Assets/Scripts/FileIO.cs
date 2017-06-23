using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public static class FileIO{

	// Use this for initialization

	// Update is called once per frame

    static public List<LevelStruct> readLevelFile(string filename)
    {
        List<LevelStruct> levelList = new List<LevelStruct>();
        int lineNum = 0;
        string path = "Assets/Resources/" + filename;
        Debug.Log("Loading file");
        string line;
        try
        {
            StreamReader reader = new StreamReader(path);
            using (reader)
            {
                do
                {
                    Debug.Log("reading files");
                    line = reader.ReadLine();
                    if (lineNum != 0 && line!= null)
                    {
                        LevelStruct level;
                        line = line.Replace("/r", "").Replace("/n", "");
                        Debug.Log("this is the level line: " + line);
                        string[] levelInfo = line.Split(',');
                        foreach (string s in levelInfo)
                        {
                            Debug.Log(s);
                        }
                        RatioStruct levelRatio = new RatioStruct(int.Parse(levelInfo[3].Split('/')[0]), int.Parse(levelInfo[3].Split('/')[1]));
                        level = new LevelStruct(int.Parse(levelInfo[0]), int.Parse(levelInfo[1]), int.Parse(levelInfo[2]), levelRatio, int.Parse(levelInfo[4]));
                        levelList.Add(level);
                    }
                    lineNum++;
                }   
                while (line != null);
                reader.Close();
            }
            Debug.Log(levelList.Count);
            return levelList;
        }
        catch(IOException e)
        {
            Debug.Log(e);
        }
        return levelList;
    }
    static public Dictionary<string , ThemeStruct> readThemeFile(string filename)
    {
        Regex regExpression = new Regex(@"\w+|(\(?\d\,?\)?\/?)+",RegexOptions.IgnoreCase);
        Dictionary<string, ThemeStruct> themeDict = new Dictionary<string, ThemeStruct>();
        int lineNum = 0;
        string line;
        string path = "Assets/Resources/" + filename;
        //Debug.Log("Loading file");
        try
        {
            StreamReader reader = new StreamReader(path);
            using (reader)
            {
                do
                {
                    //Debug.Log("reading file");
                    line = reader.ReadLine();
                    if (lineNum != 0 && line != null)
                    {
                        line = line.Trim();
                        //print("this is the level line: " + line);
                        MatchCollection matches = regExpression.Matches(line);
                        List<Color> themeColorList = readColorRange(matches[3].Value);
                        themeDict.Add(matches[0].Value,new ThemeStruct(matches[0].Value,matches[1].Value,themeColorList,bgImg : matches[3].Value));




                    }
                    lineNum++;
                }
                while (line != null);
            }
            return themeDict;
        }
        catch (IOException e)
        {
            Debug.Log(e);
        }
        return themeDict;
    }
    static private List<Color> readColorRange(string colorRangestring)
    {
        List<Color> colorList = new List<Color>();
        string[] colorSplit = colorRangestring.Split('/');
        for (int i = 0; i < colorSplit.Length; i++)
        {
            colorSplit[i] = colorSplit[i].Trim(')', '(');

        }
        foreach (string color in colorSplit)
        {
            string[] rgbv = color.Split(',');
            Color temp = new Color(float.Parse(rgbv[0]), float.Parse(rgbv[1]), float.Parse(rgbv[2]), float.Parse(rgbv[3]));
            colorList.Add(temp);
        }

        return colorList; 
    }
    static public void writeTrialData(TrialStruct trialInfo)
    {
        string Filename = "TestOutput.txt";
        StreamWriter writer;
        if (File.Exists(Filename))
        {
            using (writer = new StreamWriter(Filename))
            {
                writer.WriteLine(trialInfo.ToString());
            }
        }
        else
        {
            writer = File.CreateText("TestOutput.txt");
            writer.WriteLine(trialInfo.ToString());
        }
        writer.Close();
    }


}

