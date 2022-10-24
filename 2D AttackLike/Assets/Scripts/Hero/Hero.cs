using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TypeOfInputSkill
{
    Damage,
    Heal,
    Reward
}

public enum TypeOfHUD
{
    Levelup
}

public class Hero : Character
{
    HeroInfo info;
    HeroState heroState;
    Vector3 originPos;
    GameObject miniBar;
    Transform miniBarPos;
    new TMP_Text name;
    Image hpBar;
    Image expBar;

    MyHeroManager myHeroManager;

    float regenTime = 0;

    public CharState CharState { get { return heroState.charState; } }
    public AttackType AttackType { get { return info.attackType; } }
    public CharacterClassType CharClassType { get { return info.charClassType; } }

    public int CurrentHP {  get { return info.currentHP; } }
    public int MaxHP {  get { return info.healthPower; } }

    private void Start()
    {
        charState = GetComponent<CharacterState>();
        heroState = GetComponent<HeroState>();
        myHeroManager = FindObjectOfType<MyHeroManager>();
        originPos = transform.position;
    }

    private void Update()
    {
        regenTime += Time.deltaTime;
        if(regenTime > 5)
        {
            int regen = (int)info.regen;
            Regen(regen);
            regenTime = 0;
        }
    }

    public void SetInfo(HeroInfo heroInfo)
    {
        this.info = heroInfo;
    }

    public HeroInfo GetInfo()
    {
        return info;
    }

    public void FillHp()
    {
        info.currentHP = info.healthPower;
    }


    public void StartBattle()
    {
        StartCoroutine(IEStartBattle());
        heroState.charState = CharState.Attack;
    }

    public void StopBattle()
    {
        StopCoroutine(IEStartBattle());
        heroState.charState = CharState.Idle;
    }

    IEnumerator IEStartBattle()
    {
        while(true)
        {
            float time = 0;
            float attckTime = 1 / info.attackInterval;
            while (time < attckTime)
            {
                time += Time.deltaTime;
                if(heroState.charState == CharState.Dead)
                {
                    yield break;
                }
                while (!GameData.isBattle)
                    yield return null;
                yield return null;
            }
            while(!GameData.IsLiveMonster())
                yield return null;
            SetTrigger("Attack");
        }
    }

    public void SetMiniBar(bool state)
    {
        if(transform.Find("MiniBar"))
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

        expBar = transform.Find("MiniBar(Clone)/Root/EXPBar").GetComponent<Image>();
        Image expBack = transform.Find("MiniBar(Clone)/Root/EXPBack").GetComponent<Image>();
        expBar.gameObject.SetActive(true);
        expBack.gameObject.SetActive(true);
        expBar.fillAmount = (float)info.currentEXP / (float)info.nextEXP;
    }

    void UpdateMinibar()
    {
        hpBar.fillAmount = (float)info.currentHP / (float)info.healthPower;
        expBar.fillAmount = (float)info.currentEXP / (float)info.nextEXP;
    }

    public void DamagedFromEnemy(int attackPower)
    {
        int result = CalcDamage(attackPower);

        ParticleSystem effect;
        if (result > 0)
        {
            info.currentHP -= result;
            AudioMng.Instance.PlayActionEffect("punch_general_body_impact_01");
            
            effect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Effect/hit-white-1"), this.transform);
            effect.transform.position = transform.position + Vector3.up * 0.5f;

            ShowDamage(result);
            
            UpdateMinibar();
        }

        if (info.currentHP > 0) // hit
        {
            StartCoroutine(IEAttackedReaction());
        }
        else // die
        {
            heroState.charState = CharState.Dead;
            heroState.SetTrigger("Dead");
            myHeroManager.CheckLiveHeroCount();
        }
    }

    public int CalcDamage(int enemyAttack)
    {
        int result = enemyAttack - info.defense;

        return result;
    }

    public void AttackEvent()
    {
        int power = CalcDamageWithLuck(info.attackPower);
        if(AttackType==AttackType.Magic)
        {
            myHeroManager.Heal(power);
        }
        else
        {
            myHeroManager.HitEnemy(power, info.attackType);
            // 테스트용으로 강력하게 설정
            //myHeroManager.HitEnemy(300, info.attackType);
        }
    }

    int CalcDamageWithLuck(int atk)
    {
        int luck = Random.Range(0, 101);
        if (luck <= info.luck)
            return Mathf.RoundToInt(atk * 1.5f);
        return atk;
    }

    public void ActiveBattle(bool state)
    {
        heroState.PlayAnimator(state);
    }

    public void ChangeAnimatorSpeed()
    {
        heroState?.SetAnimatorSpeed();
    }

    IEnumerator IEAttackedReaction()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + Vector3.left * 0.2f;

        float time = 0;

        while (time <= 1f)
        {
            time += Time.deltaTime * 4;
            float interpolation = (-Mathf.Pow(time, 2) + time) * 4;
            transform.position = Vector3.Lerp(originPos, targetPosition, interpolation);
            while (!GameData.isBattle)
                yield return null;
            yield return null;
        }
    }

    public void Healed(int attackPower)
    {
        int resultHeal = attackPower;
        if (info.healthPower - info.currentHP < attackPower)
            resultHeal = info.healthPower - info.currentHP;
        
        info.currentHP += resultHeal;

        //if (info.currentHP > info.healthPower)
        //    info.currentHP = info.healthPower;
        ParticleSystem effect = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Effect/FX_Heal_02"), this.transform);
        
        ShowDamage(resultHeal, TypeOfInputSkill.Heal);
        UpdateMinibar();
        AudioMng.Instance.PlayActionEffect("potion_flask_mana_collect_01");
    }

    public void Regen(int regenPower)
    {
        if (regenPower == 0) return;
        if (info.currentHP == info.healthPower) return;

        int resultHeal = regenPower;
        if (info.healthPower - info.currentHP < regenPower)
            resultHeal = info.healthPower - info.currentHP;

        info.currentHP += resultHeal;

        //if (info.currentHP > info.healthPower)
        //    info.currentHP = info.healthPower;

        ShowDamage(resultHeal, TypeOfInputSkill.Heal);
        UpdateMinibar();
    }

    void ShowDamage(int damage, TypeOfInputSkill type = TypeOfInputSkill.Damage)
    {
        GameObject damageUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/DamageText"), this.transform);
        Transform damageUIPos = damageUI.transform.Find("Root").GetComponent<Transform>();
        damageUIPos.position = Camera.main.WorldToScreenPoint(this.transform.position) + Vector3.up * 200;
        TMP_Text damageText = damageUI.transform.Find("Root/Damage").GetComponent<TMP_Text>();
        damageText.text = damage.ToString();
        switch(type)
        {
            case TypeOfInputSkill.Damage:
                damageText.color = Color.red;
                break;
            case TypeOfInputSkill.Heal:
                damageText.color = Color.green;
                break;
            case TypeOfInputSkill.Reward:
                damageText.color = Color.yellow;
                break;
        }
        

        StartCoroutine(MoveDamage(damageUIPos));
    }

    IEnumerator MoveDamage(Transform damageUIPos)
    {
        float time = 0;
        float originY = damageUIPos.position.y;
        float targetY = originY + 100f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float posX = damageUIPos.position.x;
            float posY = damageUIPos.position.y;
            posY = Mathf.Lerp(originY, targetY, time);
            damageUIPos.position = new Vector2(posX, posY);

            yield return null;
        }
        Destroy(damageUIPos.parent.gameObject);
    }

    IEnumerator ShowHUDText(TypeOfHUD typeHUD)
    {
        GameObject HUDText = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LevelUpText"), this.transform);
        Transform HUDUIPos = HUDText.transform.Find("Root").GetComponent<Transform>();
        HUDUIPos.position = Camera.main.WorldToScreenPoint(this.transform.position) + Vector3.up * 300;
        TMP_Text text = HUDText.transform.Find("Root/LevelUp").GetComponent<TMP_Text>();
        
        switch (typeHUD)
        {
            case TypeOfHUD.Levelup:
                text.text = "LEVEL UP!!";
                text.color = Color.yellow;
                break;
            
        }

        float time = 0;
        float originY = HUDUIPos.position.y;
        float targetY = originY + 100f;
        while (time < 2)
        {
            time += Time.deltaTime;
            float posX = HUDUIPos.position.x;
            float posY = HUDUIPos.position.y;
            posY = Mathf.Lerp(originY, targetY, time);
            HUDUIPos.position = new Vector2(posX, posY);

            yield return null;
        }
        yield return new WaitForSeconds(1);
        Destroy(HUDUIPos.parent.gameObject);
    }

    public void AddEXP(int exp)
    {
        if (heroState.charState == CharState.Dead) return;
        info.currentEXP += CalcExpWithLuck(exp);
        UpdateMinibar();
        myHeroManager.UpdateHeroStatCost();
        if (info.currentEXP >= info.nextEXP) // 레벨업
        {
            info.level++;
            info.currentEXP = info.currentEXP - info.nextEXP;
            info.nextEXP = DataManager.ToI(TableType.CharLevelTable, info.level, "EXP");
            LevelUp();
        }
    }

    public int CalcExpWithLuck(int exp)
    {
        int result = exp;
        int luck = Random.Range(0, 101);
        if (luck <= info.luck)
            result = exp * 2;
        return result;
    }

    void LevelUp()
    {
        StartCoroutine(ShowHUDText(TypeOfHUD.Levelup));
        switch (CharClassType)
        {
            case CharacterClassType.Knight:
                {
                    info.attackPower += DataManager.ToI(TableType.CharLevelTable, info.level, "K_ATK");
                    info.attackInterval += DataManager.ToF(TableType.CharLevelTable, info.level, "K_SPEED");
                    info.attackInterval = Mathf.Floor(info.attackInterval * 100) * 0.01f;
                    info.healthPower += DataManager.ToI(TableType.CharLevelTable, info.level, "K_HEALTH");
                    info.currentHP += DataManager.ToI(TableType.CharLevelTable, info.level, "K_HEALTH");
                    info.tacticPoint += DataManager.ToI(TableType.CharLevelTable, info.level, "K_TACTIC");
                }
                break;
            case CharacterClassType.Archer:
                {
                    info.attackPower += DataManager.ToI(TableType.CharLevelTable, info.level, "A_ATK");
                    info.attackInterval += DataManager.ToF(TableType.CharLevelTable, info.level, "A_SPEED");
                    info.attackInterval = Mathf.Floor(info.attackInterval * 100) * 0.01f;
                    info.healthPower += DataManager.ToI(TableType.CharLevelTable, info.level, "A_HEALTH");
                    info.currentHP += DataManager.ToI(TableType.CharLevelTable, info.level, "A_HEALTH");
                    info.tacticPoint += DataManager.ToI(TableType.CharLevelTable, info.level, "A_TACTIC");
                }
                break;
            case CharacterClassType.Tactician:
                {
                    info.attackPower += DataManager.ToI(TableType.CharLevelTable, info.level, "T_ATK");
                    info.attackInterval += DataManager.ToF(TableType.CharLevelTable, info.level, "T_SPEED");
                    info.attackInterval = Mathf.Floor(info.attackInterval * 100) * 0.01f;
                    info.healthPower += DataManager.ToI(TableType.CharLevelTable, info.level, "T_HEALTH");
                    info.currentHP += DataManager.ToI(TableType.CharLevelTable, info.level, "T_HEALTH");
                    info.tacticPoint += DataManager.ToI(TableType.CharLevelTable, info.level, "T_TACTIC");
                }
                break;
            case CharacterClassType.Assassin:
                {
                    info.attackPower += DataManager.ToI(TableType.CharLevelTable, info.level, "S_ATK");
                    info.attackInterval += DataManager.ToF(TableType.CharLevelTable, info.level, "S_SPEED");
                    info.attackInterval = Mathf.Floor(info.attackInterval * 100) * 0.01f;
                    info.healthPower += DataManager.ToI(TableType.CharLevelTable, info.level, "S_HEALTH");
                    info.currentHP += DataManager.ToI(TableType.CharLevelTable, info.level, "S_HEALTH");
                    info.tacticPoint += DataManager.ToI(TableType.CharLevelTable, info.level, "S_TACTIC");
                }
                break;
        }
        UpdateMinibar();
        myHeroManager.UpdateHeroStatPanel();
        AudioMng.Instance.PlayUIEffect("sci-fi_device_item_power_up_flash_02");
    }
}
