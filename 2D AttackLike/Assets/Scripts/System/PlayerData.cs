using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int playerGold = 2000;

    // 내가 갖고 있는 장수 리스트
    public static List<HeroInfo> myHeroList = new List<HeroInfo>();

    public static int CheckEmptySlot()
    {
        int count = 0;
        for(int i = 0; i < myHeroList.Count; i++)
        {
            if(myHeroList[i] != null)
            {
                count++;
            }
        }
        return count;
    }

    public static void AddHero(HeroInfo info)
    {
        myHeroList.Add(info);
    }

    public static void DelHero(HeroInfo info)
    {
        myHeroList.Remove(info);
    }

    public static void SetMyHeroList(List<Hero> heroList)
    {
        myHeroList.Clear();
        for (int i = 0; i < heroList.Count; i++)
        {
            HeroInfo info = new HeroInfo();
            info = heroList[i].GetInfo();
            myHeroList.Add(info);
        }
    }
}
