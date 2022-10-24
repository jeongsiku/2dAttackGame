using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHeroStat : MonoBehaviour
{
    GameManager gameManager;
    
    Image heroFace;
    Image hpBar;
    TMP_Text hpText;
    Image expBar;
    TMP_Text expText;
    TMP_Text charName;
    TMP_Text charClass;
    TMP_Text level;
    TMP_Text round;

    TMP_Text atk;
    TMP_Text speed;
    TMP_Text def;
    TMP_Text health;
    TMP_Text regen;
    TMP_Text luck;

    Button atkButton;
    Button atkButtonOff;
    TMP_Text atkUpgadeCostOn;
    TMP_Text atkUpgadeCostOff;
    Button speedButton;
    Button speedButtonOff;
    TMP_Text speedUpgadeCostOn;
    TMP_Text speedUpgadeCostOff;
    Button defButton;
    Button defButtonOff;
    TMP_Text defUpgadeCostOn;
    TMP_Text defUpgadeCostOff;
    Button hpButton;
    Button hpButtonOff;
    TMP_Text hpUpgadeCostOn;
    TMP_Text hpUpgadeCostOff;
    Button regenButton;
    Button regenButtonOff;
    TMP_Text regenUpgadeCostOn;
    TMP_Text regenUpgadeCostOff;
    Button luckButton;
    Button luckButtonOff;
    TMP_Text luckUpgadeCostOn;
    TMP_Text luckUpgadeCostOff;

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();

        heroFace = transform.Find("Root/HeadBack/FaceBack/Face").GetComponent<Image>();
        hpBar = transform.Find("Root/HeadBack/HPBack/HPBar").GetComponent<Image>();
        hpText = transform.Find("Root/HeadBack/HPBack/HPText").GetComponent<TMP_Text>();
        expBar = transform.Find("Root/HeadBack/EXPBack/EXPBar").GetComponent<Image>();
        expText = transform.Find("Root/HeadBack/EXPBack/EXPText").GetComponent<TMP_Text>();
        charName = transform.Find("Root/HeadBack/CharName/Name").GetComponent<TMP_Text>();
        charClass = transform.Find("Root/HeadBack/CharName/Class").GetComponent<TMP_Text>();
        level = transform.Find("Root/HeadBack/LevelBack/Level").GetComponent<TMP_Text>();
        round = transform.Find("Root/HeadBack/LevelBack/Round").GetComponent<TMP_Text>();
        atk = transform.Find("Root/InfoBack/Stat/ATK/Value").GetComponent<TMP_Text>();
        def = transform.Find("Root/InfoBack/Stat/DEFENSE/Value").GetComponent<TMP_Text>();
        speed = transform.Find("Root/InfoBack/Stat/SPEED/Value").GetComponent<TMP_Text>();
        health = transform.Find("Root/InfoBack/Stat/HEALTH/Value").GetComponent<TMP_Text>();
        regen = transform.Find("Root/InfoBack/Stat/REGEN/Value").GetComponent<TMP_Text>();
        luck = transform.Find("Root/InfoBack/Stat/LUCK/Value").GetComponent<TMP_Text>();

        atkButton = transform.Find("Root/InfoBack/Stat/ATK/Grade/Up").GetComponent<Button>();
        atkButton?.onClick.AddListener(UpgrageATK);
        atkButtonOff = transform.Find("Root/InfoBack/Stat/ATK/Grade/Down").GetComponent<Button>();
        atkButtonOff?.onClick.AddListener(NotUpgradeable);  
        atkUpgadeCostOn = transform.Find("Root/InfoBack/Stat/ATK/Grade/Up/Text").GetComponent<TMP_Text>();
        atkUpgadeCostOff = transform.Find("Root/InfoBack/Stat/ATK/Grade/Down/Text").GetComponent<TMP_Text>();

        speedButton = transform.Find("Root/InfoBack/Stat/SPEED/Grade/Up").GetComponent<Button>();
        speedButton?.onClick.AddListener(UpgrageSPEED);
        speedButtonOff = transform.Find("Root/InfoBack/Stat/SPEED/Grade/Down").GetComponent<Button>();
        speedButtonOff?.onClick.AddListener(NotUpgradeable);
        speedUpgadeCostOn = transform.Find("Root/InfoBack/Stat/SPEED/Grade/Up/Text").GetComponent<TMP_Text>();
        speedUpgadeCostOff = transform.Find("Root/InfoBack/Stat/SPEED/Grade/Down/Text").GetComponent<TMP_Text>();

        defButton = transform.Find("Root/InfoBack/Stat/DEFENSE/Grade/Up").GetComponent<Button>();
        defButton?.onClick.AddListener(UpgrageDEF);
        defButtonOff = transform.Find("Root/InfoBack/Stat/DEFENSE/Grade/Down").GetComponent<Button>();
        defButtonOff?.onClick.AddListener(NotUpgradeable);
        defUpgadeCostOn = transform.Find("Root/InfoBack/Stat/DEFENSE/Grade/Up/Text").GetComponent<TMP_Text>();
        defUpgadeCostOff = transform.Find("Root/InfoBack/Stat/DEFENSE/Grade/Down/Text").GetComponent<TMP_Text>();

        hpButton = transform.Find("Root/InfoBack/Stat/HEALTH/Grade/Up").GetComponent<Button>();
        hpButton?.onClick.AddListener(UpgrageHP);
        hpButtonOff = transform.Find("Root/InfoBack/Stat/HEALTH/Grade/Down").GetComponent<Button>();
        hpButtonOff?.onClick.AddListener(NotUpgradeable);
        hpUpgadeCostOn = transform.Find("Root/InfoBack/Stat/HEALTH/Grade/Up/Text").GetComponent<TMP_Text>();
        hpUpgadeCostOff = transform.Find("Root/InfoBack/Stat/HEALTH/Grade/Down/Text").GetComponent<TMP_Text>();

        regenButton = transform.Find("Root/InfoBack/Stat/REGEN/Grade/Up").GetComponent<Button>();
        regenButton?.onClick.AddListener(UpgrageREGEN);
        regenButtonOff = transform.Find("Root/InfoBack/Stat/REGEN/Grade/Down").GetComponent<Button>();
        regenButtonOff?.onClick.AddListener(NotUpgradeable);
        regenUpgadeCostOn = transform.Find("Root/InfoBack/Stat/REGEN/Grade/Up/Text").GetComponent<TMP_Text>();
        regenUpgadeCostOff = transform.Find("Root/InfoBack/Stat/REGEN/Grade/Down/Text").GetComponent<TMP_Text>();

        luckButton = transform.Find("Root/InfoBack/Stat/LUCK/Grade/Up").GetComponent<Button>();
        luckButton?.onClick.AddListener(UpgrageLUCK);
        luckButtonOff = transform.Find("Root/InfoBack/Stat/LUCK/Grade/Down").GetComponent<Button>();
        luckButtonOff?.onClick.AddListener(NotUpgradeable);
        luckUpgadeCostOn = transform.Find("Root/InfoBack/Stat/LUCK/Grade/Up/Text").GetComponent<TMP_Text>();
        luckUpgadeCostOff = transform.Find("Root/InfoBack/Stat/LUCK/Grade/Down/Text").GetComponent<TMP_Text>();
    }

    public void Setting(HeroInfo info)
    {
        string sprite = DataManager.ToS(TableType.HeroTable, info.charID, "FACEIMAGE");
        heroFace.sprite = Resources.Load<Sprite>("Images/HeroFace/" + sprite);
        UpdateBar(info);
        charName.text = info.name;
        switch(info.charClassType)
        {
            case CharacterClassType.Knight:
                charClass.text = "장수";
                break;
            case CharacterClassType.Assassin:
                charClass.text = "산적";
                break;
            case CharacterClassType.Archer:
                charClass.text = "궁수";
                break;
            case CharacterClassType.Tactician:
                charClass.text = "책사";
                break;
        }
        level.text = "Level: " + info.level.ToString();
        round.text = "Round: 99";
        atk.text = info.attackPower.ToString();
        speed.text = info.attackInterval.ToString();
        def.text = info.defense.ToString();
        health.text = info.healthPower.ToString();
        regen.text = info.regen.ToString();
        luck.text = info.luck.ToString();

        RefreshAllCost(info);
    }

    public void RefreshAllCost(HeroInfo info)
    {
        RefreshATKCost(info);
        RefreshSPEEDCost(info);
        RefreshDefenseCost(info);
        RefreshHealthCost(info);
        RefreshRegenCost(info);
        RefreshLuckCost(info);
    }
    
    void RefreshATKCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.atkUpgrageLevel, "COST");
        atkUpgadeCostOn.text = string.Format($"{cost:N0}");
        atkUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) atkButton.gameObject.SetActive(true);
        else atkButton.gameObject.SetActive(false);
    }

    void RefreshSPEEDCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.speedUpgrageLevel, "COST");
        speedUpgadeCostOn.text = string.Format($"{cost:N0}");
        speedUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) speedButton.gameObject.SetActive(true);
        else speedButton.gameObject.SetActive(false);
    }

    void RefreshDefenseCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.defUpgrageLevel, "COST");
        defUpgadeCostOn.text = string.Format($"{cost:N0}");
        defUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) defButton.gameObject.SetActive(true);
        else defButton.gameObject.SetActive(false);
    }

    void RefreshHealthCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.hpUpgrageLevel, "COST");
        hpUpgadeCostOn.text = string.Format($"{cost:N0}");
        hpUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) hpButton.gameObject.SetActive(true);
        else hpButton.gameObject.SetActive(false);
    }

    void RefreshRegenCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.regenUpgrageLevel, "COST");
        regenUpgadeCostOn.text = string.Format($"{cost:N0}");
        regenUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) regenButton.gameObject.SetActive(true);
        else regenButton.gameObject.SetActive(false);
    }

    void RefreshLuckCost(HeroInfo info)
    {
        int cost = DataManager.ToI(TableType.UpgradeTable, info.luckUpgrageLevel, "COST");
        luckUpgadeCostOn.text = string.Format($"{cost:N0}");
        luckUpgadeCostOff.text = string.Format($"{cost:N0}");
        if (cost <= PlayerData.playerGold) luckButton.gameObject.SetActive(true);
        else luckButton.gameObject.SetActive(false);
    }

    public void UpdateBar(HeroInfo info)
    {
        hpText.text = string.Format($"{info.currentHP} / {info.healthPower}");
        expText.text = string.Format($"{info.currentEXP} / {info.nextEXP}");
        hpBar.fillAmount = (float)info.currentHP / (float)info.healthPower;
        expBar.fillAmount = (float)info.currentEXP / (float)info.nextEXP;
    }

    public void UpdateStat(HeroInfo info)
    {
        level.text = "Level: " + info.level.ToString();
        round.text = "Round: 99";
        atk.text = info.attackPower.ToString();
        speed.text = info.attackInterval.ToString();
        def.text = info.defense.ToString();
        health.text = info.healthPower.ToString();
        regen.text = info.regen.ToString();
        luck.text = info.luck.ToString();
    }

    void UpgrageATK()
    {
        gameManager.UpgrageATK(GameData.nowHeroStatPanel);
    }

    void UpgrageSPEED()
    {
        gameManager.UpgrageSPEED(GameData.nowHeroStatPanel);
    }

    void UpgrageDEF()
    {
        gameManager.UpgrageDEF(GameData.nowHeroStatPanel);
    }

    void UpgrageHP()
    {
        gameManager.UpgrageHP(GameData.nowHeroStatPanel);
    }

    void UpgrageREGEN()
    {
        gameManager.UpgrageREGEN(GameData.nowHeroStatPanel);
    }

    void UpgrageLUCK()
    {
        gameManager.UpgrageLUCK(GameData.nowHeroStatPanel);
    }

    void NotUpgradeable()
    {
        gameManager.ShowSystemMsg("금이 부족합니다");
        AudioMng.Instance.PlayUIEffect("sci-fi_code_fail_06");
    }

    public void RefreshButton()
    {

    }
}
