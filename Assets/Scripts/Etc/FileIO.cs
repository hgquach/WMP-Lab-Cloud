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
    static private string FILE_PATH_TO_LEVEL_FOLDER = Path.Combine(Application.persistentDataPath, "Levels");
    static private string FILE_PATH_TO_THEME_FOLDER = Path.Combine(Application.persistentDataPath, "Themes");
    private const int CORRECT_LINE_NUM = 21;

    static public void checkedAndCreateLevelFolder()
    {
        DirectoryInfo LevelFolder = new DirectoryInfo(FileIO.FILE_PATH_TO_LEVEL_FOLDER);
        if(!LevelFolder.Exists)
        {
            LevelFolder.Create();
        }
    }

    static public void checkandCreateThemeFolder()
    {
        DirectoryInfo ThemeFolder = new DirectoryInfo(FileIO.FILE_PATH_TO_THEME_FOLDER);
        if(!ThemeFolder.Exists)
        {
            ThemeFolder.Create();
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
        string pathway = Path.Combine(Application.persistentDataPath, "ParticipantData");
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
            //Debug.Log("who is the current participant: " + currentParticipant);
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
                    if (linenum != 0 && line != null && !line.Contains("***"))
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
        defaultPref.SurveyQuestion = true;
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
        defaultPref.RoundLimit = 3;
        defaultPref.IsTimed = false;
        defaultPref.SurveyQuestion = true;
        defaultPref.ThemeChange = 2;
        defaultPref.MaxError = 85;
        defaultPref.MinError = 65;
        defaultPref.trialsPerRd = 10;
        defaultPref.Language = "English";
        writePrefFile(defaultPref,true);
    }

    static private void writeEnglishFile()
    {
        string filepath = Path.Combine(Application.persistentDataPath, Path.Combine("Languages", "English.txt"));
        string[] englishText = { "START", "DEMO", "NEXT", "GO!", "DONE", "MAIN MENU", "LEVEL","xxx % correct",
        "Average Response Time: xxx seconds","Perfect Score Bonus: xxx","Points: xxx","How much did you like the game today?",
        "I really liked the game", "The game was okay","I did not like the game","How hard did you try today?","I tried my best",
        "I tried a little","I did not try","You got xxx points today!","You have xxx points so far!"};

        if (!File.Exists(filepath))
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                foreach (string s in englishText)
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

    static public ArrayList searchandFilterForThemeFolder()
    {
        ArrayList themeFolders = new ArrayList();
        DirectoryInfo RootThemeFolder = new DirectoryInfo(FileIO.FILE_PATH_TO_THEME_FOLDER);
        DirectoryInfo[] allThemeFolder = RootThemeFolder.GetDirectories();
        if (RootThemeFolder.Exists)
        {
            foreach (DirectoryInfo dir in allThemeFolder)
            {
                if (searchAndCheckAllThemeFolder(dir.FullName))
                {
                   //Debug.Log(dir.Name);
                   themeFolders.Add(dir.Name);
                }

            }
        }
        GameData.gamedata.haveThemes = themeFolders.Count > 0 ? true : false;
        return themeFolders;
    }

    static private bool searchAndCheckAllThemeFolder(string themeFolder)
    {
        DirectoryInfo targetFolder = new DirectoryInfo(themeFolder);
        if(targetFolder.GetFiles("*.jpg").Length == 1 && targetFolder.GetFiles("*.txt").Length == 1 && targetFolder.GetFiles("*.png").Length == 1)
        {
            return true;
        }
        return false;


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

    static public ThemeStruct returnThemeInFolder(string themefolder)
    {
        string pathway = Path.Combine(FileIO.FILE_PATH_TO_THEME_FOLDER, Path.Combine(themefolder, "ThemeParameter.txt"));
       // Debug.Log("this should point to the folders theme txt: "+pathway);
        if(File.Exists(pathway))
        {
            return readThemeFile(pathway);
        }
        else
        {
            Debug.Log("Does not exist");
        }
        return new ThemeStruct();
    }

    static public List<LevelStruct> returnLevelInFolder()
    {
        string pathway = Path.Combine(FileIO.FILE_PATH_TO_LEVEL_FOLDER, "Levels.txt");
        if(File.Exists(pathway))
        {
            GameData.gamedata.haveLevels = true;
            return readLevelFile(pathway);
        }
        else
        {
            Debug.Log("Does not exist");
            GameData.gamedata.haveLevels = false;
        }

        return new List<LevelStruct>();
    }
    
    static  public Sprite returnSpriteInfolder(string themefolder, string filename)
    {
       // Debug.Log("atempting to retrive sprite from folder");
        string pathway;
        if (filename == "background")
        {
            pathway = Path.Combine(FileIO.FILE_PATH_TO_THEME_FOLDER, Path.Combine(themefolder, filename + ".jpg"));
        }
        else
        {
            pathway = Path.Combine(FileIO.FILE_PATH_TO_THEME_FOLDER, Path.Combine(themefolder, filename + ".png"));
        }
        if(File.Exists(pathway))
        {
           // Debug.Log("image found");
            return LoadImageAsSprite(pathway);
        }
        else
        {
            Debug.Log("file does not exist");
            return null;
        }
    }

    static public string returnPathWayToCurrentParticipantFile()
    {
        string currentParticipantFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Participants", readCurrentParticipant() + ".txt"));
        return currentParticipantFilePath;
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
                if (line != null && !line.Contains("***"))
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
    
    static public void DirectoryCopy(string sourceDir,string destdir)
    {
        DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDir);

        if(!sourceDirectory.Exists)
        {
            throw new DirectoryNotFoundException
                ("Source directory does not exist or could not be found: "
                + sourceDir
                );
        }
        if(!Directory.Exists(destdir))
        {
            Directory.CreateDirectory(destdir);
        }
        DirectoryInfo[] dirsInSource = sourceDirectory.GetDirectories();
        FileInfo[] files = sourceDirectory.GetFiles();
        foreach(FileInfo file in files)
        {
            string temppath = Path.Combine(destdir, file.Name);
            file.CopyTo(temppath, true);
        }

        foreach(DirectoryInfo subdir in dirsInSource)
        {
            string temppath = Path.Combine(destdir, subdir.Name);
            DirectoryCopy(subdir.FullName, temppath);
        }

    }

    // functions used to populate the language, theme , and level folder in file structure along with functions that used the WWW class to access streamming assets
    static public void moveStreamingAssets(Dictionary<string, List<string>> assetDirectory)
    {
        string pathwaySrc, pathwayTrg;
        foreach (KeyValuePair<string, List<string>> pair in assetDirectory)
        {
            foreach(string filename in pair.Value)
            {

                Debug.Log(filename);

                switch(pair.Key)
                {
                    case "Level":
                        pathwaySrc = Application.streamingAssetsPath+"/"+pair.Key+"/"+filename;
                        pathwayTrg = FileIO.FILE_PATH_TO_LEVEL_FOLDER+"/"+filename;
                        break;
                    case "Language":
                        pathwaySrc = Application.streamingAssetsPath + "/" + pair.Key + "/" + filename;
                        pathwayTrg = FileIO.FILE_PATH_TO_LANGAUGE_FOLDER+"/"+filename;
                        break;
                    default:
                        DirectoryInfo themeFolder = new DirectoryInfo(Path.Combine(FileIO.FILE_PATH_TO_THEME_FOLDER, pair.Key.ToString()));
                        if(!themeFolder.Exists)
                        {
                            themeFolder.Create();
                        }
                        //string tempfilepath = Path.Combine(Application.streamingAssetsPath, filename );
                        //Debug.Log(pair.Key + "," + filename);
                        //Debug.Log(tempfilepath);
                        //Debug.Log(Path.Combine(Application.streamingAssetsPath,tempfilepath));
                        pathwaySrc = Application.streamingAssetsPath + "/" + "Background" + "/" + pair.Key + "/" + filename;
                        pathwayTrg = FileIO.FILE_PATH_TO_THEME_FOLDER + "/" + pair.Key + "/" + filename;
                        //pathwaySrc = Path.Combine(Application.streamingAssetsPath,Path.Combine("Background",Path.Combine(pair.Key,filename)));
                        //pathwayTrg = Path.Combine(FileIO.FILE_PATH_TO_THEME_FOLDER, Path.Combine(pair.Key.ToString(), filename));
                        break;
                }
                 Debug.Log(pathwaySrc);
                 pathwayTrg = pathwayTrg.Replace("\n", "").Replace("\r", "");
                 Debug.Log(pathwayTrg);
                 WWW reader = new WWW(pathwaySrc);
                 while (!reader.isDone) { }
                 if (!File.Exists(pathwayTrg))
                 {
                    if (pathwayTrg.Contains(".txt"))
                    {
                        //File.Create(pathwayTrg);
                        File.WriteAllBytes(pathwayTrg, reader.bytes);

                    }

                    if (pathwayTrg.Contains(".png"))
                    {
                        File.WriteAllBytes(pathwayTrg, reader.texture.EncodeToPNG());
                    }

                    if (pathwayTrg.Contains(".jpg"))
                    {
                        File.WriteAllBytes(pathwayTrg, reader.texture.EncodeToJPG());

                    }
                 }
                 reader.Dispose();


           }


        }
    }

    static public Dictionary<string, List<string>> readStreamingAssetDirectory()
    {
        Dictionary<string, List<string>> fileDirectory = new Dictionary<string, List<string>>();
        string pathway = Path.Combine(Application.streamingAssetsPath, "directory.txt");
        WWW reader = new WWW(pathway);
        while (!reader.isDone) { }
        string filecontent = reader.text.Trim();
        string[] splitContent = filecontent.Split('\n');

        foreach (string s in splitContent)
        {
            string[] tempSplit = s.Split(',');
            //Debug.Log(tempSplit[0] + "," + tempSplit[1]);
           if(fileDirectory.ContainsKey(tempSplit[0]) )
           {
                string trimmedFilename = tempSplit[1].TrimEnd('\r', '\n').TrimStart('\r','\n');
                fileDirectory[tempSplit[0]].Add(trimmedFilename);

           }
           else
           {
                fileDirectory.Add(tempSplit[0], new List<string>());
                fileDirectory[tempSplit[0]].Add(tempSplit[1]);
           }
        }
        return fileDirectory;

    }

    static public ParticipantStruct readParticipantFile(string filepathway)
    {
        string line;
        ParticipantStruct participant = new ParticipantStruct();
        using (StreamReader reader = new StreamReader(filepathway))
        {
            do
            {
                line = reader.ReadLine();
                Debug.Log(line);
                if(line != null && !line.Contains("ID") && !line.Contains("***"))
                {
                    line = line.Replace("/r", "").Replace("/n", "");
                    string[] participantInfo = line.Split(',');
                    participant = setParticipantInfo(participantInfo, participant);
                }
            }
            while (line != null);
        }
        return participant;
    }

    static public List<LevelStruct> readLevelFile(string filename)
    {
        List<LevelStruct> levelList = new List<LevelStruct>();
        string line;
        try
        {
            StreamReader reader = new StreamReader(filename);
            using (reader)
            {
                do
                {
                //    Debug.Log("reading files");
                    line = reader.ReadLine();
                    if (line != null && !line.Equals("") && !line.Contains("***") && !line.Contains("LevelNumber")) 
                    {
                        LevelStruct level;
                      //  Debug.Log("this is the level line: " + line);
                        string[] levelInfo = line.Split(new[] { ',' },System.StringSplitOptions.RemoveEmptyEntries);
                        //Debug.Log("this is the levels ratio" + levelInfo[3]);
                        RatioStruct levelRatio = new RatioStruct(int.Parse(levelInfo[3].Split('/')[0]), int.Parse(levelInfo[3].Split('/')[1]));
                        level = new LevelStruct(int.Parse(levelInfo[0]), int.Parse(levelInfo[1]), float.Parse(levelInfo[2]), levelRatio);
                        levelList.Add(level);
                    }
                }
                while (line != null);
                reader.Close();
            }
            //Debug.Log(levelList.Count);
            return levelList;
        }
        catch (IOException e)
        {
            Debug.Log(e);
        }
        return levelList;
    }

    static public Dictionary<string,string> readLanguageFile(string languageName)
    {
        string[] arrayOfKey = 
            { "START", "DEMO","NEXT","GO!", "DONE", "MAIN MENU", "LEVEL","correct",
        "AvgReact","BonusPoints","Points","Question1",
        "Q1res1", "Q1res2","Q1res3","Question2","Q2res1",
        "Q2res2","Q2res3","PointsToday","PointsSoFar"};

        string[] allLine;
        Dictionary<string, string> translatedDictionary = new Dictionary<string, string>();
        string languageFilePath = Path.Combine(Application.persistentDataPath, Path.Combine("Languages", languageName + ".txt"));

        allLine = File.ReadAllLines(languageFilePath,Encoding.GetEncoding("iso-8859-1"));
        
        for(int i = 0; i < allLine.Length && i < arrayOfKey.Length; i++)
        {
            if (!allLine[i].Contains("***"))
            {
                translatedDictionary[arrayOfKey[i]] = allLine[i];
            }
        }

        return translatedDictionary;




    }

    static public ThemeStruct readThemeFile(string filename)
    {
        Regex regExpression = new Regex(@"\w+|(\(?\d\,?\)?\/?)+",RegexOptions.IgnoreCase);
        ThemeStruct newTheme = new ThemeStruct();
        string line;
        //TextAsset themeFile = Resources.Load(filename) as TextAsset;
        
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                do
                {
                    line = reader.ReadLine();
                    //Debug.Log("this is the single line in the theme file: "+line);
                    if (line !=null && !line.Contains("ThemeName")&& !line.Contains("***"))
                    {
                        MatchCollection matches = regExpression.Matches(line);
                        //Debug.Log("Match count: " + matches.Count);
                        foreach(Match match in matches)
                        {
                      //      Debug.Log(match.Value);
                        }
                        List<Color> themeColorList = readColorRange(matches[2].Value);
                        Color UITextcolor = readUIColor(matches[4].Value);
                        return newTheme = new ThemeStruct(matches[0].Value,matches[1].Value,themeColorList,UITextcolor,bgImg : matches[3].Value);
                    }
                }
                while (line != null);
            }
        }

        catch(IOException e)
        {
            Debug.Log(e);
        }
        return newTheme;
        /*
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
        */
    }
    
    static private Texture2D LoadTexture(string filepath)
    {
        Texture2D image;
        byte[] FileData;
        if(File.Exists(filepath))
        {
            FileData = File.ReadAllBytes(filepath);
            image = new Texture2D(2, 2);
            if(image.LoadImage(FileData))
            {
                return image;
            }
        }
        return null;
    }

    static private Sprite LoadImageAsSprite(string filepath , float pixelPerUnit = 100.0f)
    {
        Sprite newImageSprite = new Sprite();
        Texture2D imageTexture = LoadTexture(filepath);
        newImageSprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(.5f,.5f), pixelPerUnit);
        //Debug.Log("sucessfully read and converted the sprite image");
        return newImageSprite;
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
        string baseName = GameData.gamedata.currentParticipant.ID + "-" + GameData.gamedata.currentParticipant.SessionNumber + "-" +currentDate+"-"+ currentTime+".csv";
        string Filename = Path.Combine(Application.persistentDataPath, Path.Combine("ParticipantData", baseName));
        StreamWriter writer;
        if (!GameData.gamedata.isDemo)
        {
            if (!File.Exists(Filename))
            {
                using (writer = new StreamWriter(Filename, true))
                {
                    writer.WriteLine("Task Name , Task Version , OS Information , Screen Resolution, ParticipantID, Date When Game Started," +
                        "Time When Game Started, Session,Level, Round Number, Trial Number, Accuracy,Correct Answer, Reaction Time, Points Won,Response(R/L)," +
                        "Num Of Elements in Cloud 1 , Num Of Elements in Cloud 2, Element sprite, Theme,Total Number of Round,Total Play Time , Session Time," +
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


}

