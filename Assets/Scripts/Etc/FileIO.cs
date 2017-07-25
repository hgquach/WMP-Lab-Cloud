using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public static class FileIO
{
    static public string FILE_PATH_TO_MAIN_PREFERENCE = Path.Combine(Application.persistentDataPath , Path.Combine("Preferences" ,"Preference.csv"));
    
    static public string FILE_PATH_TO_DEMO_PREFERENCE =  Path.Combine(Application.persistentDataPath, Path.Combine("Preferences", "DemoPreference.csv"));

    static public string FILE_PATH_TO_DEMO_PARTICIPANTS = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", "DemoParticipant.txt"));

    static private string FILE_PATH_TO_LANGAUGE_FOLDER =  Path.Combine(Application.persistentDataPath, "Languages");

    private const int CORRECT_LINE_NUM = 20;

    static public List<LevelStruct> readLevelFile(string filename)
    {
        List<LevelStruct> levelList = new List<LevelStruct>();
        int lineNum = 0;
        //string path = Application.dataPath+"Resources/" + filename;
        //Debug.Log("Loading file");
        TextAsset levelFile= Resources.Load(filename) as TextAsset;
        string[] splitLevelFile = levelFile.text.Split("\n"[0]);
        foreach(string levelInfo in splitLevelFile)
        {
            if(lineNum != 0)
            {
                LevelStruct level;
                string[] individualLevelInfo = levelInfo.Split(","[0]);
                RatioStruct levelRatio = new RatioStruct(int.Parse(individualLevelInfo[3].Split('/')[0]), int.Parse(individualLevelInfo[3].Split('/')[1]));
                level = new LevelStruct(int.Parse(individualLevelInfo[0]), int.Parse(individualLevelInfo[1]), int.Parse(individualLevelInfo[2]), levelRatio);
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

    static public ArrayList readThemeFile(string filename)
    {
        Regex regExpression = new Regex(@"\w+|(\(?\d\,?\)?\/?)+",RegexOptions.IgnoreCase);
        ArrayList themeArrayList = new ArrayList();
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
                Color UITextColor = readUIColor(matches[4].Value);
                themeArrayList.Add(new ThemeStruct(matches[0].Value,matches[1].Value,themeColorList,UITextColor,bgImg : matches[3].Value));
            }
            lineNum++;
        }
        return themeArrayList;
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

    static private Color readUIColor(string colorString)
    {
        colorString= colorString.Trim(')', '(');
        string[] colorSplit = colorString.Split(',');
        Color uiColor = new Color(float.Parse(colorSplit[0]), float.Parse(colorSplit[1]), float.Parse(colorSplit[2]));
        //Debug.Log(uiColor);
        return uiColor;

    }

    static public void writeTrialData(TrialStruct trialInfo,string currentTime)
    {
        string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        //string currentTime = System.DateTime.Now.ToString("h-mm-ss");
        string baseName = GameData.gamedata.currentParticipant.ID + "-" + GameData.gamedata.currentParticipant.SessionNumber + "-" +currentDate+"-"+ currentTime+".csv";
        string Filename = Path.Combine(Application.persistentDataPath, Path.Combine("Trials", baseName));
        StreamWriter writer;
        if (!GameData.gamedata.isDemo)
        {
            if (!File.Exists(Filename))
            {
                using (writer = new StreamWriter(Filename, true))
                {
                    writer.WriteLine("Task Name , Task Version , OS Information , Screen Resolution, ParticipantID, Date When Game Started," +
                        "Time When Game Started, Session,Level, Round Number, Trial Number, Accuracy, Reaction Time, Points Won,Response(R/L)," +
                        "Size of/Num Of Element in Cloud 1 , Size of/Num Of Element in Cloud 2, Element sprite, Background,Total Number of Round,Total Play Time , Session Time," +
                        "Difficulty, Effort");
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

    static public void checkAndCreateLanguageFolder()
    {
        DirectoryInfo LanguageFolder = new DirectoryInfo(FileIO.FILE_PATH_TO_LANGAUGE_FOLDER);
        if(!LanguageFolder.Exists)
        {
            LanguageFolder.Create();
            writeEnglishFile();
            GameData.gamedata.translatedDictionary = readLanguageFile("English");


        }
    }

    static public void checkAndCreatePrefFolder()
    {
        string folderPathway = Path.Combine(Application.persistentDataPath, "Preferences");
        string perfFilePath = FileIO.FILE_PATH_TO_MAIN_PREFERENCE;
        string demoPathWay = FileIO.FILE_PATH_TO_DEMO_PREFERENCE;
        DirectoryInfo prefFolder = new DirectoryInfo(folderPathway);
        if (!prefFolder.Exists)
        {
            //Debug.Log("folder does not exist");
            prefFolder.Create();
            writeDemoPreferenceFile();
            writeDefaultPreferenceFile();
        }
        else
        {
            if(!File.Exists(perfFilePath))
            {
                writeDefaultPreferenceFile();
            }
            if(!File.Exists(demoPathWay))
            {
                writeDemoPreferenceFile();
            }

        }
        GameData.gamedata.SessionPreferance = readPrefFile(perfFilePath);

    }

    static public void checkAndCreateParticipantFolder()
    {
        string pathway = Path.Combine(Application.persistentDataPath, "Participants");
        DirectoryInfo participantFolder = new DirectoryInfo(pathway);
        if(!participantFolder.Exists)
        {
            participantFolder.Create();
            writeDemoParticipant();
        }
        else
        {
            if (!File.Exists(FileIO.FILE_PATH_TO_DEMO_PARTICIPANTS))
            {
                writeDemoParticipant();
            }
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

    static public void checkAndCreateCurrentParticipantFile()
    {
        string pathway =Path.Combine(Application.persistentDataPath, Path.Combine("Participants", "CurrentParticipant.txt"));

        if(!File.Exists(pathway))
        {
            File.Create(pathway);
            GameData.gamedata.haveCurrentParticipant = false;
        }
        else
        {
            string currentParticipant = readCurrentParticipant();
            Debug.Log("who is the current participant: " + currentParticipant);
            if(currentParticipant != "" && currentParticipant != null)
            {
                string participantFilePathway = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", currentParticipant + ".txt"));
                GameData.gamedata.trialData.PartcipantId = currentParticipant;
                GameData.gamedata.currentParticipant = readParticipantFile(participantFilePathway);
                GameData.gamedata.haveCurrentParticipant = true;
            }
            else
            {
                GameData.gamedata.haveCurrentParticipant = false;

            }
        }
    }

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

    static public void writePrefFile(PreferanceStruct pref, bool isDemo = false)
    {
        StreamWriter writer;
        string filePath;
        if(isDemo)
        {
            filePath = Path.Combine(Application.persistentDataPath, Path.Combine("Preferences", "DemoPreference.csv"));
        }
        else
        {
           filePath = Path.Combine(Application.persistentDataPath , Path.Combine("Preferences" ,"Preference.csv"));
        }
        using (writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("RoundLimit,IsTimed,SurveyQuestion,ThemeChange,MaxError,MinError,TrialsPerRd,Language");
            writer.WriteLine(pref.ToString());
        }
    }

    static public ParticipantStruct readParticipantFile(string filepathway)
    {
        string line;
        int linenumber = 0;
        ParticipantStruct participant = new ParticipantStruct();
        using (StreamReader reader = new StreamReader(filepathway))
        {
            do
            {
                Debug.Log("reading participant file");
                line = reader.ReadLine();
                if(linenumber != 0 && line != null)
                {
                    line = line.Replace("/r", "").Replace("/n", "");
                    string[] participantInfo= line.Split(',');
                    participant = setParticipantInfo(participantInfo, participant);
                }
                linenumber++;
            }
            while (line != null);
        }
        return participant;
    }

    static public void writeParticipantFile(ParticipantStruct participant ,bool isDemo = false)
    {
        Debug.Log("writing to participant file");
        string filepath;
        if(isDemo)
        {

            filepath = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", "DemoParticipant.txt"));
        }
        else
        {
            filepath = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", participant.ID.ToString() + ".txt"));
        }

        using (StreamWriter writer = new StreamWriter(filepath, false))
        {
            writer.WriteLine("ID,TotalPoint,SessionNumber,Level,ThemeIndex");
            writer.WriteLine(participant.ToString());
        }

    }

    static public string readCurrentParticipant()
    {
        string currentParticipant =Path.Combine(Application.persistentDataPath, Path.Combine("Participants", "CurrentParticipant.txt"));
        string line = "";
        try
        {
            StreamReader reader = new StreamReader(currentParticipant);
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

        catch (IOException e)
        {
            Debug.Log(e);
        }

        return line;
    }

    static public void writeCurrentParticipant(string participantID)
    {
        Debug.Log("writing to the current participant file");
        StreamWriter writer;
        string filePath = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", "CurrentParticipant"+".txt"));
        using (writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine(participantID);
        }
    }

    static public void writeDemoParticipant()
    {
        ParticipantStruct demoParticipant = new ParticipantStruct(999,0,0,0,0);
        writeParticipantFile(demoParticipant, true);
        
    }

    static public void writeDefaultPreferenceFile()
    {
        PreferanceStruct defaultPref = new PreferanceStruct();
        defaultPref.RoundLimit = 10;
        defaultPref.IsTimed = false;
        defaultPref.SurveyQuestion = false;
        defaultPref.ThemeChange = 2;
        defaultPref.MaxError = 85;
        defaultPref.MinError = 65;
        defaultPref.trialsPerRd = 25;
        defaultPref.Language = "English";
        writePrefFile(defaultPref,false);

    }

    static public void writeDemoPreferenceFile()
    {

        PreferanceStruct defaultPref = new PreferanceStruct();
        defaultPref.RoundLimit = 10;
        defaultPref.IsTimed = false;
        defaultPref.SurveyQuestion = false;
        defaultPref.ThemeChange = 2;
        defaultPref.MaxError = 85;
        defaultPref.MinError = 65;
        defaultPref.trialsPerRd = 25;
        defaultPref.Language = "English";
        writePrefFile(defaultPref,true);
    }

    static private void writeEnglishFile()
    {
        string filepath = Path.Combine(Application.persistentDataPath, Path.Combine("Languages","English.txt"));
        string[] englishText = { "START", "DEMO", "GO!", "DONE", "MAIN MENU", "LEVEL","xxx% correct",
        "Average Reaction Time: xxx seconds","Perfect Score Bonus: xxx","Points:xxx","How much did you like the game today?",
        "I really liked the game", "the game was okay","I did not like the game","How hard did you try today?","I tried my best",
        "I tried a little","I did not try","You got xxx points today!","You have xxx points so far!"};

        if(!File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                foreach(string s in englishText)
                    writer.WriteLine(s, true);
            }
        }
    }
    
    static public List<string> searchandFilterForLanguageFile()
    {
        string[] LanguageFiles = Directory.GetFiles(FileIO.FILE_PATH_TO_LANGAUGE_FOLDER, "*.txt");
        List<string> AcceptedLanguage = new List<string>(); 
        foreach(string s in LanguageFiles)
        {
            string[] allLine;
            string languageFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Languages", s )); 
            allLine = File.ReadAllLines(languageFilePath);
            if(allLine.Length > 0 && allLine.Length == FileIO.CORRECT_LINE_NUM)
            {
                AcceptedLanguage.Add(Path.GetFileNameWithoutExtension(s));
            }
            
        }

        return AcceptedLanguage;
    }

    static public Dictionary<string,string> readLanguageFile(string languageName)
    {
        string[] arrayOfKey = 
            { "START", "DEMO", "GO!", "DONE", "MAIN MENU", "LEVEL","correct",
        "Average Reaction Time","Perfect Score Bonus","Points","Question1",
        "Q1res1", "Q1res2","Q1res3","Question2","Q2res1",
        "Q2res2","Q2res3","PointsToday","PointsSoFar"};
        string[] allLine;
        Dictionary<string, string> translatedDictionary = new Dictionary<string, string>();
        string languageFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Languages", languageName + ".txt"));

        allLine = File.ReadAllLines(languageFilePath,Encoding.GetEncoding("iso-8859-1"));
        
        for(int i = 0; i < allLine.Length && i < arrayOfKey.Length; i++)
        {
            translatedDictionary[arrayOfKey[i]] = allLine[i];
        }

        return translatedDictionary;




    }

    static public void changePreferenceToFilePath(string filepath)
    {
        GameData.gamedata.SessionPreferance = readPrefFile(filepath);
    }

    static public void changeParticipantToFilePath(string filepath)
    {
        GameData.gamedata.currentParticipant = readParticipantFile(filepath);
    }

    static private ParticipantStruct setParticipantInfo(string [] participantInfo , ParticipantStruct participant)
    {
        participant.ID = int.Parse(participantInfo[0]);
        participant.PointTotal = int.Parse(participantInfo[1]);
        participant.SessionNumber = int.Parse(participantInfo[2]);
        participant.Level = int.Parse(participantInfo[3]);
        participant.Theme = int.Parse(participantInfo[4]);
        return participant;

    }

    static private PreferanceStruct setPlayerPref(string[] prefInfo, PreferanceStruct playerPref)
    {
        playerPref.RoundLimit = int.Parse(prefInfo[0]);
        playerPref.IsTimed = bool.Parse(prefInfo[1]);
        playerPref.SurveyQuestion = bool.Parse(prefInfo[2]);
        playerPref.ThemeChange = int.Parse(prefInfo[3]);
        playerPref.MaxError = int.Parse(prefInfo[4]);
        playerPref.MinError = int.Parse(prefInfo[5]);
        playerPref.trialsPerRd = int.Parse(prefInfo[6]);
        playerPref.Language = prefInfo[7];
        return playerPref;
    }

    static public string returnPathWayToCurrentParticipantFile()
    {
        string currentParticipantFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", readCurrentParticipant() + ".txt"));
        return currentParticipantFilePath;
    }



}

