using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneList
{
    Title,
    Town,
    Game
}

public static class GameData
{
    public static int currStageIdx = 1001;
    public static int selectShopHeroID = -1;
    public static int countHeroInShop = 12;

    public static bool isBattle = true;
    public static int countLiveMonster = 0;

    public static SceneList nowScene = SceneList.Title;

    public static bool isStageUI = false;
    public static bool isShopUI = false;
    public static bool isStatUI = false;
    public static bool isSystemUI = false;
    public static int nowHeroStatPanel = 0;

    public static int clearStageNum = 1000;

    public static int gameSpeed = 1;

    public static bool hasLoadData = false;

    public static bool IsLiveMonster()
    {
        if(countLiveMonster == 0)
            return false;
        return true;
    }
}
