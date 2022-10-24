using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    public static SaveData saveData = new SaveData();

    public static void Save(string fileName)
    {
        saveData.countHeroInShop = GameData.countHeroInShop;
        saveData.clearStageNum = GameData.clearStageNum;
        saveData.playerGold = PlayerData.playerGold;

        List<HeroInfo> data = new List<HeroInfo> ();
        for(int i = 0; i < PlayerData.myHeroList.Count; i++)
        {
            HeroInfo hero = new HeroInfo();
            hero = PlayerData.myHeroList[i];
            data.Add(hero);
            
        }
        var heroList = new HeroListForSave(data);
        saveData.heroListForSave = heroList;

        string json = JsonConvert.ToJson<SaveData>(saveData, true);
        WriteFile(GetPath(fileName), json);
    }

    public static void Load(string fileName)
    {
        string json = ReadFile(GetPath(fileName));
        saveData = JsonConvert.FromJson<SaveData>(json);

        GameData.countHeroInShop = saveData.countHeroInShop;
        GameData.clearStageNum= saveData.clearStageNum;
        PlayerData.playerGold = saveData.playerGold;

        PlayerData.myHeroList.Clear();
        for(int i = 0; i < saveData.heroListForSave.data.Count; i++)
        {
            PlayerData.myHeroList.Add(saveData.heroListForSave.data[i]);
        }
    }

    public static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using(StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }

    }

    public static string ReadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open);
        string readAllText = string.Empty;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            readAllText = reader.ReadToEnd();
        }
        return readAllText;
    }

    public static string GetPath(string fileName)
    {
        string filePath = "";
        if (Application.isMobilePlatform || Application.isConsolePlatform)
            filePath = Application.persistentDataPath;
        else
            filePath = Application.dataPath;
        return Path.Combine(filePath, fileName);
    }
}
