using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Transform readyPos;
    Transform[] enemyPos = new Transform[4];
    //List<Monster> enemyList = new List<Monster>();
    Dictionary<int, List<Monster>> newEnemyList = new Dictionary<int, List<Monster>>();

    int monSequence = -1;

    MyHeroManager myHeroManager;
    GameManager gameManager;

    public void Init()
    {
        readyPos = transform.Find("ReadyPos");

        for (int i = 0; i < enemyPos.Length; i++)
        {
            enemyPos[i] = transform.Find("BattlePos/"+((i + 1).ToString()));
        }

        myHeroManager = FindObjectOfType<MyHeroManager>();
        gameManager = FindObjectOfType<GameManager>();

        InitEnemy();
    }

    void InitEnemy()
    {
        monSequence = -1;

        newEnemyList.Clear();
        int stage = GameData.currStageIdx;
        for (int i = 0; i < 10; i++)
        {
            List<Monster> newMonsterList = new List<Monster>();
            for (int j=0; j < 4; j++)
            {
                int monID = DataManager.ItoI(TableType.NewEnemySpawnListTable, stage*100+(i+1), (j+1).ToString());
                if (monID == 0) continue;
                string prefabName = DataManager.ToS(TableType.MonsterTable, monID, "PREFAB");
                
                newMonsterList.Add(Instantiate(Resources.Load<Monster>("Prefabs/Monsters/" + prefabName), readyPos));
                newMonsterList[j].SetInfo(DataManager.SetMonsterInfo(monID));
            }
            newEnemyList.Add(i, newMonsterList);
            
        }
    }

    public void SpawnMonster(float time = 1)
    {
        monSequence++;
        GameData.countLiveMonster=0;
        int cnt = 0;
        foreach(Monster mon in newEnemyList[monSequence])
        {
            if (mon != null)
            {
                mon.transform.parent = enemyPos[cnt];
                mon.transform.position = enemyPos[cnt].position;
                mon.SetMiniBar(true);
                GameData.countLiveMonster++;
                mon.StartBattle();
                cnt++;
            }
        }
    }

    public void StopBattle()
    {
        foreach (Monster mon in newEnemyList[monSequence])
        {
            if (mon != null)
            {
                mon.StopBattle();
            }
        }
    }

    public void SetTriggerAllEnemy(string triggerName)
    {

        for(int i=0; i< newEnemyList.Count; i++)
        {
            foreach (Monster mon in newEnemyList[i])
            {
                mon?.SetTrigger(triggerName);
            }
        }
    }

    public void ActiveBattle(bool state)
    {
        if (monSequence == -1) return;
        for (int i = monSequence; i < newEnemyList.Count; i++)
        {
            foreach (Monster mon in newEnemyList[i])
            {
                if(mon != null)
                    mon.ActiveBattle(state);
            }
        }
    }

    public void ChangeAnimatorSpeed()
    {
        if (monSequence == -1) return;
        for (int i = monSequence; i < newEnemyList.Count; i++)
        {
            foreach (Monster mon in newEnemyList[i])
            {
                if (mon != null)
                    mon.ChangeAnimatorSpeed();
            }
        }
    }

    public void CheckWhoWillDamaged(int attackPower, AttackType attackType)
    {
        Monster monster = WhoLive();
        if (monster==null)
            return; // 살아있는 몹이 없음
        else
        {
            monster.DamagedFromEnemy(attackPower, attackType);
        }
    }

    public void HitEnemy(int attackPower)
    {
        myHeroManager.CheckWhoWillDamaged(attackPower);
    }

    public Monster WhoLive()
    {
        for (int i = 0; i < enemyPos.Length; i++)
        {
            int count = enemyPos[i].childCount-1;
            if(count >= 0)
            {
                Monster monster = enemyPos[i].GetChild(0).GetComponent<Monster>();
                if (monster == null) return null;
                if (monster.CharState == CharState.Dead)
                    continue;
                if (monster.CharState == CharState.Attack || monster.CharState == CharState.Idle)
                    return monster;
            }
        }
        return null;
    }

    // 일정 시간 후 MonsterDead() 함수를 실행하는 코루틴을 호출하는 함수
    public void MonsterDead(float time)
    {
        StartCoroutine(IEMonsterDead(time));
    }

    // 일정 시간 후 MonsterDead() 함수를 실행하는 코루틴
    public IEnumerator IEMonsterDead(float time)
    {
        float elapsed = 0;
        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            while(!GameData.isBattle)
                yield return null;
            yield return null;
        }
        MonsterDead();
    }

    void MonsterDead()
    {
        if (GameData.countLiveMonster == 0)
        {
            if (newEnemyList.Count - 1 == monSequence)
                Clear();
            else
                SpawnMonster();
        }
    }

    void Clear()
    {
        gameManager.OpenClearPanel(true);
        myHeroManager.Clear();
    }

    public void SendEXP(int exp)
    {
        myHeroManager.AddEXP(exp);
    }
}
