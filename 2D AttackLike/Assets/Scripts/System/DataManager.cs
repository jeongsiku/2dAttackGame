using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType
{
    StageTable,
    HeroTable,
    MonsterTable,
    NewEnemySpawnListTable,
    CharLevelTable,
    UpgradeTable
}

public static class DataManager
{
    private static Dictionary<TableType, LowBase> tableList = new Dictionary<TableType, LowBase>();
    public static Dictionary<int, StageInfo> stageDic = new Dictionary<int, StageInfo>();
    public static Dictionary<int, HeroInfo> heroDic = new Dictionary<int,HeroInfo>();
    public static Dictionary<int, MonsterInfo> monsterDic = new Dictionary<int, MonsterInfo>();

    public static void SetStage(int uniqueID, int stageID)
    {
        StageInfo stageInfo = new StageInfo();
        stageInfo.stageId = stageID;
        stageInfo.stageName = DataManager.ToS(TableType.StageTable, stageID, "SNAME");
        stageInfo.level = DataManager.ToI(TableType.StageTable, stageID, "SLEVEL");

        if (stageDic.ContainsKey(uniqueID) == false)
            stageDic.Add(uniqueID, stageInfo);
    }

    public static void SetHero(int uniqueID, int heroID)
    {
        HeroInfo heroInfo = SetHeroInfo(heroID);
        
        if (heroDic.ContainsKey(uniqueID) == false)
            heroDic.Add(uniqueID, heroInfo);
    }

    public static HeroInfo GetHero(int heroID)
    {
        HeroInfo heroInfo = SetHeroInfo(heroID);
        return heroInfo;
    }

    static HeroInfo SetHeroInfo(int heroID)
    {
        HeroInfo heroInfo = new HeroInfo();
        heroInfo.charID = heroID;
        string classType = DataManager.ToS(TableType.HeroTable, heroID, "CLASSTYPE");
        heroInfo.charClassType = (CharacterClassType)System.Enum.Parse(typeof(CharacterClassType), classType);
        string attackType = DataManager.ToS(TableType.HeroTable, heroID, "ATTACKTYPE");
        heroInfo.attackType = (AttackType)System.Enum.Parse(typeof(AttackType), attackType);
        heroInfo.name = DataManager.ToS(TableType.HeroTable, heroID, "NAME");
        heroInfo.level = DataManager.ToI(TableType.HeroTable, heroID, "LEVEL");
        heroInfo.attackPower = DataManager.ToI(TableType.HeroTable, heroID, "ATTACKPOWER");
        heroInfo.attackInterval = DataManager.ToF(TableType.HeroTable, heroID, "ATTACKINTERVAL");
        heroInfo.healthPower = DataManager.ToI(TableType.HeroTable, heroID, "HEALTHPOWER");
        heroInfo.currentHP = heroInfo.healthPower;
        heroInfo.tacticPoint = DataManager.ToI(TableType.HeroTable, heroID, "TACTICPOINT");
        heroInfo.heroCost = DataManager.ToI(TableType.HeroTable, heroID, "COST");
        heroInfo.currentEXP = 0;
        heroInfo.nextEXP = DataManager.ToI(TableType.CharLevelTable, heroInfo.level, "EXP");
        return heroInfo;
    }

    public static void SetMonster(int uniqueID, int monID)
    {
        MonsterInfo monInfo = SetMonsterInfo(monID);

        if (monsterDic.ContainsKey(uniqueID) == false)
            monsterDic.Add(uniqueID, monInfo);
    }

    public static MonsterInfo GetMonster(int monID)
    {
        MonsterInfo monInfo = SetMonsterInfo(monID);
        return monInfo;
    }

    public static MonsterInfo SetMonsterInfo(int monID)
    {
        MonsterInfo monInfo = new MonsterInfo();
        monInfo.charID = monID;
        monInfo.name = DataManager.ToS(TableType.MonsterTable, monID, "NAME");
        monInfo.level = DataManager.ToI(TableType.MonsterTable, monID, "LEVEL");
        monInfo.attackPower = DataManager.ToI(TableType.MonsterTable, monID, "ATTACKPOWER");
        monInfo.attackInterval = DataManager.ToF(TableType.MonsterTable, monID, "ATTACKINTERVAL");
        monInfo.healthPower = DataManager.ToI(TableType.MonsterTable, monID, "HEALTHPOWER");
        monInfo.currentHP = monInfo.healthPower;
        monInfo.tacticPoint = DataManager.ToI(TableType.MonsterTable, monID, "TACTICPOINT");
        monInfo.rewardGold = monInfo.healthPower + monInfo.attackPower;
        return monInfo;
    }

    public static void SetStageDic(TableType tableType)
    {
        int uniqueCount = 0;
        for(int i = 0; i < GetStageLength(tableType); i++)
        {
            uniqueCount = i + 1;
            int stageID = uniqueCount + 1000;
            SetStage(uniqueCount, stageID);
        }
    }

    public static void SetHeroDic(TableType tableType)
    {
        int uniqueCount = 0;
        for (int i = 0; i < GetHeroLength(tableType); i++)
        {
            uniqueCount = i + 1;
            int heroID = uniqueCount + 2000;
            SetHero(uniqueCount, heroID);
        }
    }

    public static void SetMonsterDic(TableType tableType)
    {
        int uniqueCount = 0;
        for (int i = 0; i < GetMonsterLength(tableType); i++)
        {
            uniqueCount = i + 1;
            int monID = uniqueCount + 3000;
            SetMonster(uniqueCount, monID);
        }
    }

    public static List<StageInfo> GetStages()
    {
        List<StageInfo> stageList = new List<StageInfo>();
        foreach(var pair in stageDic)
        {
            stageList.Add(pair.Value);
        }
        return stageList;
    }

    public static List<HeroInfo> GetHeroes()
    {
        List<HeroInfo> heroList = new List<HeroInfo>();
        foreach (var pair in heroDic)
        {
            heroList.Add(pair.Value);
        }
        return heroList;
    }

    public static List<MonsterInfo> GetMonsters()
    {
        List<MonsterInfo> monsterList = new List<MonsterInfo>();
        foreach (var pair in monsterDic)
        {
            monsterList.Add(pair.Value);
        }
        return monsterList;
    }

    public static LowBase Get(TableType tableType)
    {
        LowBase lowBase = null;
        if(tableList.ContainsKey(tableType))
            lowBase = tableList[tableType];
        return lowBase;
    }

    public static int GetStageLength(TableType tableType)
    {
        if (tableList.ContainsKey(tableType))
        {
            LowBase lowbase = tableList[tableType];
            return lowbase.Count;
        }
        return 0;
    }

    public static int GetHeroLength(TableType tableType)
    {
        if (tableList.ContainsKey(tableType))
        {
            LowBase lowbase = tableList[tableType];
            return lowbase.Count;
        }
        return 0;
    }

    public static int GetMonsterLength(TableType tableType)
    {
        if (tableList.ContainsKey(tableType))
        {
            LowBase lowbase = tableList[tableType];
            return lowbase.Count;
        }
        return 0;
    }

    public static int GetStageSpawnMonsterLength(TableType tableType)
    {
        if (tableList.ContainsKey(tableType))
        {
            LowBase lowbase = tableList[tableType];
            return lowbase.ColumnCount;
        }
        return 0;
    }

    public static string ToS(TableType tableType, int tableID, string subkey)
    {
        string str = string.Empty;
        if(tableList.ContainsKey(tableType))
            str = tableList[tableType].ToS(tableID,subkey);
        return str;
    }

    public static int ToI(TableType tableType, int tableID, string subkey)
    {
        int val = -1;
        if (tableList.ContainsKey(tableType))
            val = tableList[tableType].ToI(tableID, subkey);
        return val;
    }

    public static int ItoI(TableType tableType, int tableID, string subkey)
    {
        int val = -1;
        if (tableList.ContainsKey(tableType))
            val = tableList[tableType].ItoI(tableID, subkey);
        return val;
    }

    public static float ToF(TableType tableType, int tableID, string subkey)
    {
        float val = -1;
        if (tableList.ContainsKey(tableType))
            val = tableList[tableType].ToF(tableID, subkey);
        return val;
    }

    public static LowBase Load(TableType tableType, string path = "Tables/")
    {
        LowBase lowBase = null;
        if (tableList.ContainsKey(tableType))
            lowBase = tableList[tableType];
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path + tableType);
            if(textAsset != null)
            {
                lowBase = new LowBase();
                lowBase.Load(textAsset.text);
                tableList.Add(tableType, lowBase);
            }
        }
        return lowBase;
    }
}
