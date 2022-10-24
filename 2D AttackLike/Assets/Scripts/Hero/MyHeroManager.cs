using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeroManager : MonoBehaviour
{
    Transform[] heroPos = new Transform[4];
    List<Hero> heroList = new List<Hero>();
    EnemySpawner enemySpawner;
    GameManager gameManager;

    public void Init()
    {
        for (int i = 0; i < heroPos.Length; i++)
        {
            heroPos[i] = transform.Find((i+1).ToString());
        }
        InitHero();

        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void InitHero()
    {
        int i = 0;
        foreach(HeroInfo hero in PlayerData.myHeroList)
        {
            string prefabName = DataManager.ToS(TableType.HeroTable, hero.charID, "PREFAB");
            heroList.Add(Instantiate(Resources.Load<Hero>("Prefabs/Heroes/" + prefabName),
                                                          heroPos[i]));
            heroList[i].SetInfo(hero);
            heroList[i].FillHp();
            heroList[i].SetMiniBar(true);
            i++;
        }
    }

    public void SaveToPlayerData()
    {
        PlayerData.SetMyHeroList(heroList);
    }

    public void AddHero(int charID)
    {
        string prefabName = DataManager.ToS(TableType.HeroTable, charID, "PREFAB");
        int slot = CheckEmptySlot();
        heroList.Add(Instantiate(Resources.Load<Hero>("Prefabs/Heroes/"+prefabName), heroPos[slot]));
        int count = heroList.Count;
        heroList[count - 1].SetInfo(DataManager.GetHero(charID));
        heroList[count - 1].SetMiniBar(true);
    }

    public List<Hero> GetHeroList()
    {
        return heroList;
    }

    public int CheckEmptySlot()
    {
        for(int i = 0; i < heroPos.Length; i++)
        {
            if (heroPos[i].childCount == 0)
                return i;
        }
        return -1;
    }

    public void StartBattle()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            heroList[i].StartBattle();
        }
    }

    public void AddEXP(int exp)
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            heroList[i].AddEXP(exp);
        }
    }

    public void SetTriggerAllHero(string triggerName)
    {
        for(int i=0; i < heroList.Count; i++)
        {
            heroList[i]?.SetTrigger(triggerName);
        }
    }

    public void ActiveBattle(bool state)
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            heroList[i]?.ActiveBattle(state);
        }
    }

    public void ChangeAnimatorSpeed()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            heroList[i]?.ChangeAnimatorSpeed();
        }
    }

    public void CheckWhoWillDamaged(int attackPower)
    {
        if (WhoLive() == null)
            return; // 살아있는 몹이 없음
        else
        {
            Hero hero = WhoLive();
            hero.DamagedFromEnemy(attackPower);
        }
    }

    public void HitEnemy(int attackPower, AttackType attackType)
    {
        enemySpawner.CheckWhoWillDamaged(attackPower, attackType);
    }

    public void Heal(int attackPower)
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            if (heroList[i].CurrentHP < heroList[i].MaxHP)
            {
                if (heroList[i].CharState == CharState.Dead) continue;
                heroList[i].Healed(attackPower);
                return;
            }
        }
    }

    public Hero WhoLive()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            if (heroList[i].CharState == CharState.Attack || heroList[i].CharState == CharState.Idle)
                return heroList[i];
        }
        return null;
    }

    public void Clear()
    {
        SetTriggerAllHero("Win");
    }

    public void UpdateHeroStatCost()
    {
        gameManager.UpdateHeroStatCost();
    }

    public void UpdateHeroStatPanel()
    {
        gameManager.UpdateHeroStatPanel();
    }

    public void CheckLiveHeroCount()
    {
        int cnt = 0;
        for(int i = 0; i < heroList.Count; i++)
        {
            if(heroList[i].CharState == CharState.Dead)
                cnt++;
        }
        if(cnt == heroList.Count)
            FollowUpHeroAllDie();
    }

    void FollowUpHeroAllDie()
    {
        enemySpawner.StopBattle();
        gameManager.OpenFailPanel(true);
    }

    public bool CheckOwnHero(int shopCharID)
    {
        for(int i=0; i<heroList.Count; i++)
        {
            HeroInfo myInfo = heroList[i].GetInfo();
            if (myInfo.charID == shopCharID) return true;
        }
        return false;
    }
}
