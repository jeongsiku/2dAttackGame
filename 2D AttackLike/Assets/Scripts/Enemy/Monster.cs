using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster : Character
{
    MonsterInfo info;
    MonsterState monsterState;
    Vector3 originPos;
    GameObject miniBar;
    Transform miniBarPos;
    new TMP_Text name;
    Image hpBar;
    Image expBar;

    EnemySpawner enemySpawner;
    GameManager gameManager;

    public CharState CharState { get { return monsterState.charState; } }

    private void Start()
    {
        charState = GetComponent<CharacterState>();
        monsterState = GetComponent<MonsterState>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();
        
    }

    public void SetInfo(MonsterInfo monInfo)
    {
        this.info = monInfo;
    }



    public void StartBattle()
    {
        StartCoroutine(IEStartBattle());
        charState.charState = CharState.Attack;
        originPos = transform.position;
    }

    public void StopBattle()
    {
        //StopCoroutine(IEStartBattle());
        StopAllCoroutines();
        charState.charState = CharState.Idle;
    }

    IEnumerator IEStartBattle()
    {
        while (true)
        {
            float time = 0;
            float attackTime = 1 / info.attackInterval;
            while (time < attackTime)
            {
                time += Time.deltaTime;
                if (charState.charState == CharState.Dead)
                {
                    yield break;
                }
                while (!GameData.isBattle)
                    yield return null;
                yield return null;
            }
            SetTrigger("Attack");
        }
    }

    public void SetMiniBar(bool state)
    {
        if (transform.Find("MiniBar(Clone)"))
        {

        }
        else
        {
            InitMiniBar();
        }

        miniBar.gameObject.SetActive(state);

    }

    void InitMiniBar()
    {
        miniBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MiniBar"), this.transform);
        miniBarPos = transform.Find("MiniBar(Clone)/Root").GetComponent<Transform>();
        miniBarPos.position = Camera.main.WorldToScreenPoint(transform.position);

        name = transform.Find("MiniBar(Clone)/Root/Name").GetComponent<TMP_Text>();
        name.text = info.name;

        hpBar = transform.Find("MiniBar(Clone)/Root/HpBar").GetComponent<Image>();
        hpBar.fillAmount = (float)info.currentHP / (float)info.healthPower;
        
    }

    void UpdateMinibar()
    {
        hpBar.fillAmount = (float)info.currentHP / (float)info.healthPower;
    }

    public void ActiveBattle(bool state)
    {
        monsterState.PlayAnimator(state);
    }

    public void ChangeAnimatorSpeed()
    {
        monsterState.SetAnimatorSpeed();
    }

    public void DamagedFromEnemy(int attackPower, AttackType attackType)
    {
        int result = CalcDamage(attackPower);
        
        if(result > 0)
        {
            info.currentHP -= result;
            ShowDamage(result);
            UpdateMinibar();
        }
        ParticleSystem effect;
        switch (attackType)
        {
            case AttackType.Melee:
                AudioMng.Instance.PlayActionEffect("bullet_impact_body_flesh_01");
                effect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Effect/slash-1"),this.transform);
                effect.transform.position = transform.position + Vector3.up * 0.5f;
                break;

            case AttackType.Arrow:
                AudioMng.Instance.PlayActionEffect("bow_crossbow_arrow_shoot_type1_01");
                effect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Effect/hit-white-1"), this.transform);
                effect.transform.position = transform.position + Vector3.up * 0.5f;
                break;

            case AttackType.Magic:
                
                break;
        }

        if (info.currentHP > 0) // hit
        {
            monsterState.SetTrigger("Damage");
            StartCoroutine(IEAttackedReaction());
        }
        else // die
        {
            monsterState.charState = CharState.Dead;
            monsterState.SetTrigger("Dead");
            AudioMng.Instance.PlayActionEffect("goblin_fairy_death_10");
            GameData.countLiveMonster--;
            PlayerData.playerGold += info.rewardGold;
            gameManager.RefreshGoldUI();
            enemySpawner.SendEXP(info.rewardGold);
            ShowDamage(info.rewardGold, TypeOfInputSkill.Reward);
            transform.parent = null;
            
            Invoke("MonsterDead", 1);
        }
    }

    

    public int CalcDamage(int enemyAttack)
    {
        int result = enemyAttack;

        return result;
    }

    IEnumerator IEAttackedReaction()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + Vector3.right * 0.2f;

        float time = 0;
        
        while(time <= 1f)
        {
            time += Time.deltaTime * 4 ;
            float interpolation = (-Mathf.Pow(time, 2) + time)*4;
            transform.position = Vector3.Lerp(originPos, targetPosition, interpolation);
            while (!GameData.isBattle)
                yield return null;
            yield return null;
        }
    }

    public void AttackEvent()
    {
        enemySpawner.HitEnemy(info.attackPower);
        //enemySpawner.HitEnemy(5);
        //enemySpawner.HitEnemy(100);
    }

    void MonsterDead()
    {
        miniBar.gameObject.SetActive(false);
        StartCoroutine(DestroyMonster());
    }

    IEnumerator DestroyMonster()
    {
        float time = 0;
        while (time <= 1f)
        {
            time += Time.deltaTime;
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer renderer in renderers)
            {
                Color color = renderer.color;
                color.a = Mathf.Lerp(1, 0, time);
                renderer.color = color;
            }
            while (!GameData.isBattle)
                yield return null;
            yield return null;
        }
        enemySpawner.MonsterDead(1.5f);
        Destroy(gameObject);
    }

    void ShowDamage(int damage, TypeOfInputSkill type = TypeOfInputSkill.Damage)
    {
        GameObject damageUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/DamageText"), this.transform);
        Transform damageUIPos = damageUI.transform.Find("Root").GetComponent<Transform>();
        damageUIPos.position = Camera.main.WorldToScreenPoint(this.transform.position) + Vector3.up * 200;
        TMP_Text damageText = damageUI.transform.Find("Root/Damage").GetComponent<TMP_Text>();
        damageText.text = damage.ToString();
        damageUI.GetComponent<Canvas>().sortingOrder = 0;
        switch (type)
        {
            case TypeOfInputSkill.Damage:
                damageText.color = Color.red;
                break;
            case TypeOfInputSkill.Heal:
                damageText.color = Color.green;
                break;
            case TypeOfInputSkill.Reward:
                damageUI.GetComponent<Canvas>().sortingOrder = 1;
                damageText.color = Color.yellow;
                StartCoroutine(MoveDamage(damageUIPos,0.5f));
                return;
        }
        StartCoroutine(MoveDamage(damageUIPos));
    }

    IEnumerator MoveDamage(Transform damageUIPos,float waitTime = 0)
    {
        yield return new WaitForSeconds(waitTime);
        float time = 0;
        float originY = damageUIPos.position.y;
        float targetY = originY + 100f;
        while(time<1)
        {
            time += Time.deltaTime;
            float posX = damageUIPos.position.x;
            float posY = damageUIPos.position.y;
            posY = Mathf.Lerp(originY, targetY, time);
            damageUIPos.position = new Vector2( posX, posY);

            yield return null;
        }
        Destroy(damageUIPos.parent.gameObject);
    }
}
