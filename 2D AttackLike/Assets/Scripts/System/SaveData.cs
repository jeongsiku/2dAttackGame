using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class SaveData
{
    public int countHeroInShop; // ������ ���� ���� <=12
    public int clearStageNum; // Ŭ������ �������� idx >= 1000

    public int playerGold;

    //public List<HeroInfo> saveHeroInfoList;
    public HeroListForSave heroListForSave;
}

[System.Serializable]
public class HeroListForSave
{
    public List<HeroInfo> data;

    public HeroListForSave(List<HeroInfo> data)
    {
        this.data = data;
    }
}
