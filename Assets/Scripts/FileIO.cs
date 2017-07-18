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
        //string path = Application.dataPath+"Resources/" + filename;
        Debug.Log("Loading file");
        TextAsset levelFile= Resources.Load(filename) as TextAsset;
        string[] splitLevelFile = levelFile.text.Split("\n"[0]);
        foreach(string levelInfo in splitLevelFile)
        {
            if(lineNum != 0)
            {
                LevelStruct level;
                string[] individualLevelInfo = levelInfo.Split(","[0]);
                RatioStruct levelRatio = new RatioStruct(int.Parse(individualLevelInfo[3].Split('/')[0]), int.Parse(individualLevelInfo[3].Split('/')[1]));
                level = new LevelStruct(int.Parse(individualLevelInfo[0]), int.Parse(individualLevelInfo[1]), int.Parse(individualLevelInfo[2]), levelRatio, int.Parse(individualLevelInfo[4]));
                levelList.Add(level);
            }
            lineNum++;
        }
        //try
        //{
        //    StreamReader reader = new StreamReader(path);
        //    using (reader)
        //    {
        //        do
        //        {
        //            Debug.Log("reading files");
        //            line = reader.ReadLine();
        //            if (lineNum != 0 && line!= null)
        //            {
        //                LevelStruct level;
        //                line = line.Replace("/r", "").Replace("/n", "");
        //                Debug.Log("this is the level line: " + line);
        //                string[] levelInfo = line.Split(',');
        //                foreach (string s in levelInfo)
        //                {
        //                    Debug.Log(s);
        //                }
        //                RatioStruct levelRatio = new RatioStruct(int.Parse(levelInfo[3].Split('/')[0]), int.Parse(levelInfo[3].Split('/')[1]));
        //                level = new LevelStruct(int.Parse(levelInfo[0]), int.Parse(levelInfo[1]), int.Parse(levelInfo[2]), levelRatio, int.Parse(levelInfo[4]));
        //                levelList.Add(level);
        //            }
        //            lineNum++;
        //        }   
        //        while (line != null);
        //        reader.Close();
        //    }
        //    Debug.Log(levelList.Count);
            return levelList;
        //}
        //catch(IOException e)
        //{
        //    Debug.Log(e);
        //}
        //return levelList;
    }

    static public Dictionary<string , ThemeStruct> readThemeFile(string filename)
    {
        Regex regExpression = new Regex(@"\w+|(\(?\d\,?\)?\/?)+",RegexOptions.IgnoreCase);
        Dictionary<string, ThemeStruct> themeDict = new Dictionary<string, ThemeStruct>();
        int lineNum = 0;
        TextAsset themeFile = Resources.Load(filename) as TextAsset;
        string[] splitThemeFile = themeFile.text.Split(new[] { '\r','\n'},System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in splitThemeFile)
        {

           // Debug.Log(line);
            if (lineNum != 0)
            {
                line.Trim();
                MatchCollection matches = regExpression.Matches(line);
                List<Color> themeColorList = readColorRange(matches[2].Value);
                themeDict.Add(matches[0].Value,new ThemeStruct(matches[0].Value,matches[1].Value,themeColorList,bgImg : matches[3].Value));
            }
            lineNum++;
        }
        //Debug.Log("Loading file");
        //try
        //{
        //    StreamReader reader = new StreamReader(path);
        //    using (reader)
        //    {
        //        do
        //        {
        //            //Debug.Log("reading file");
        //            line = reader.ReadLine();
        //            if (lineNum != 0 && line != null)
        //            {
        //                line = line.Trim();
        //                //print("this is the level line: " + line);
        //                MatchCollection matches = regExpression.Matches(line);
        //                List<Color> themeColorList = readColorRange(matches[3].Value);
        //                themeDict.Add(matches[0].Value,new ThemeStruct(matches[0].Value,matches[1].Value,themeColorList,bgImg : matches[3].Value));




        //            }
        //            lineNum++;
        //        }
        //        while (line != null);
        //    }
        //    return themeDict;
        //}
        //catch (IOException e)
        //{
        //    Debug.Log(e);
        //}
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
        string baseName = GameData.gamedata.trialData.PartcipantId + "-" + GameData.gamedata.trialData.Session + "-" + GameData.gamedata.trialData.Date + ".csv";
        string Filename = Path.Combine(Application.persistentDataPath, Path.Combine("Trials", baseName));
        StreamWriter writer;
        if (!GameData.gamedata.isDemo)
        {
            if (!File.Exists(Filename))
            {
                using (writer = new StreamWriter(Filename, true))
                {
                    writer.WriteLine("TaskName,PartcipantId, Session,Screen Resolution,Timed Session, Round Limit ,Date/Time," +
                        "DotSprite,RoundNumber,Level," + "accuracy,ReactionTime, Response, sizeLeftCloud,sizeRightCloud,color,Seperation,AndroidOSVersion," +
            "GameVersion,TimeElasped");
                }
            }
            else
            {
                using (writer = new StreamWriter(Filename, true))
                {
                    writer.WriteLine(trialInfo.ToString());
                }
            }
        }

    }

    static public void writeSurveyResponse(string responseString)
    {
        string baseName = GameData.gamedata.trialData.PartcipantId + "-" + GameData.gamedata.trialData.Session + "-" + GameData.gamedata.trialData.Date + ".csv";
        string Filename = Path.Combine(Application.persistentDataPath, Path.Combine("Trials", baseName));

        if (!GameData.gamedata.isDemo)
        {
            if(File.Exists(Filename))
            {
                using (StreamWriter writer = new StreamWriter(Filename,true))
                {
                    writer.WriteLine(responseString);
                }
            }
        }

    }

    static public void checkAndCreatePrefFolder()
    {
        string pathway = Path.Combine(Application.persistentDataPath, "Preferences");
        DirectoryInfo prefFolder = new DirectoryInfo(pathway);
        if (!prefFolder.Exists)
        {
            //Debug.Log("folder does not exist");
            prefFolder.Create();
        }
        else
        {
            //Debug.Log("folder exist");
        }
    }

    static public void checkAndCreateTrialFolder()
    {
        string pathway = Path.Combine(Application.persistentDataPath, "Trials");
        DirectoryInfo prefFolder = new DirectoryInfo(pathway);
        if (!prefFolder.Exists)
        {
            //Debug.Log("folder does not exist");
            prefFolder.Create();
        }
        else
        {
            //Debug.Log("folder exist");
        }
    }

    static public void checkandCreateCurrentPrefFile()
    {
        string pathway = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences", "CurrentPref.txt"));
        if (!File.Exists(pathway))
        {
            File.Create(pathway);
            GameData.gamedata.haveCurrentPref = false;
        }
        else
        {
            
            string currentParticipantID;
            currentParticipantID = readCurrentPref(pathway);
            if (currentParticipantID != "" && currentParticipantID != null)
            {
                string prefFilePathway = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences", currentParticipantID+".csv"));
                GameData.gamedata.trialData.PartcipantId = currentParticipantID;
                GameData.gamedata.playerPref = readPrefFile(prefFilePathway);
                GameData.gamedata.haveCurrentPref = true;
            }
        }
    }
    // all these function assume that the Preference directory exists

    static public PreferanceStruct readPrefFile(string filePathway)
    {
        PreferanceStruct playerPref  = new PreferanceStruct();
        int linenum = 0;
        string line;
        try
        {
            StreamReader reader = new StreamReader(filePathway);
            using (reader)
            {
                do
                {
                    Debug.Log("reading files");
                    line = reader.ReadLine();
                    if (linenum != 0 && line != null)
                    {
                        line = line.Replace("/r", "").Replace("/n", "");
                        string[] prefInfo = line.Split(',');
                        playerPref= setPlayerPref(prefInfo, playerPref);
                    }
                    linenum++;
                }
                while (line != null);
            }
        }
        catch (IOException e)
        {
            Debug.Log(e);
        }
        return playerPref;
    }

    static public void createPrefFile(PreferanceStruct pref , int participantID)
    {
        StreamWriter writer;
        string filePath = Path.Combine(Application.persistentDataPath , Path.Combine(@"Preferences" ,participantID+".csv"));
        //Debug.Log("this is where the file are saved: " + filePath);
        using (writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("SessionNumber,Level,RoundLimit,Theme,IsTimed,SurveyQuestion");
            writer.WriteLine(pref.ToString());
        }
    }

    static private PreferanceStruct setPlayerPref(string[] prefInfo, PreferanceStruct playerPref)
    {
        playerPref.SessionNumber = int.Parse(prefInfo[0]);
        playerPref.Level = int.Parse(prefInfo[1]);
        playerPref.Theme = prefInfo[3];
        playerPref.IsTimed = bool.Parse(prefInfo[4]);
        playerPref.RoundLimit = int.Parse(prefInfo[2]);
        playerPref.SurveyQuestion = bool.Parse(prefInfo[5]);
        return playerPref;
    }

    // read and write to the current pref file

    static public string readCurrentPref(string filepathway)
    {
        string line ="";
        try
        {
            StreamReader reader = new StreamReader(filepathway);
            using (reader)
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    line.Replace("\n", "").Replace("\r", "");
                    if (line != null)
                    {
                        return line;
                    }
                }

            }
        }

        catch(IOException e)
        {
            Debug.Log(e);
        }

        return line;
    }

    static public void writeCurrentPref(string participantID)
    {
        StreamWriter writer;
        string filePath = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences", "CurrentPref.txt"));
        using (writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine(participantID);
        }
    }
}

