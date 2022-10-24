using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITownMenu : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;
    Button selectStageButton;
    Button heroShopButton;

    Button hero1;
    Button hero2;
    Button hero3;
    Button hero4;

    TMP_Text hero1Name;
    TMP_Text hero2Name;
    TMP_Text hero3Name;
    TMP_Text hero4Name;

    Button systemButton;
    UISystemMenu systemMenu;

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = GetComponentInParent<UIManager>();

        selectStageButton = transform.Find("SelectStage").GetComponent<Button>();
        selectStageButton?.onClick.AddListener(OpenSelectStage);

        heroShopButton = transform.Find("HeroShop").GetComponent<Button>();
        heroShopButton?.onClick.AddListener(OpenHeroShop);

        hero1 = transform.Find("HeroButton/Hero1").GetComponent<Button>();
        hero1Name = transform.Find("HeroButton/Hero1/HeroName").GetComponent<TMP_Text>();
        hero1?.onClick.AddListener(OpenHero1Stat);
        hero2 = transform.Find("HeroButton/Hero2").GetComponent<Button>();
        hero2Name = transform.Find("HeroButton/Hero2/HeroName").GetComponent<TMP_Text>();
        hero2?.onClick.AddListener(OpenHero2Stat);
        hero3 = transform.Find("HeroButton/Hero3").GetComponent<Button>();
        hero3Name = transform.Find("HeroButton/Hero3/HeroName").GetComponent<TMP_Text>();
        hero3?.onClick.AddListener(OpenHero3Stat);
        hero4 = transform.Find("HeroButton/Hero4").GetComponent<Button>();
        hero4Name = transform.Find("HeroButton/Hero4/HeroName").GetComponent<TMP_Text>();
        hero4?.onClick.AddListener(OpenHero4Stat);

        systemButton = transform.Find("SystemMenu").GetComponent<Button>();
        systemButton?.onClick.AddListener(OpenSystemMenu);
        systemMenu = transform.Find("UISystemMenu").GetComponent<UISystemMenu>();
        systemMenu?.Init();

        SetHeroButton();
    }

    public void SetHeroButton()
    {
        hero1.gameObject.SetActive(false);
        hero2.gameObject.SetActive(false);
        hero3.gameObject.SetActive(false);
        hero4.gameObject.SetActive(false);
        List<Hero> heroList = gameManager.GetHeroList();
        for (int i = 0; i < heroList.Count; i++)
        {
            HeroInfo info = heroList[i].GetInfo();
            if (i == 0)
            {
                hero1Name.text = info.name;
                hero1.gameObject.SetActive(true);
            }
            if (i == 1)
            {
                hero2Name.text = info.name;
                hero2.gameObject.SetActive(true);
            }
            if (i == 2)
            {
                hero3Name.text = info.name;
                hero3.gameObject.SetActive(true);
            }
            if (i == 3)
            {
                hero4Name.text = info.name;
                hero4.gameObject.SetActive(true);
            }
        }
    }

    public void OpenHero1Stat() { gameManager.OpenHeroStat(1); }
    public void OpenHero2Stat() { gameManager.OpenHeroStat(2); }
    public void OpenHero3Stat() { gameManager.OpenHeroStat(3); }
    public void OpenHero4Stat() { gameManager.OpenHeroStat(4); }

    public void OpenSelectStage()
    {
        if(GameData.isStageUI == true)
        {
            uiManager.ActiveUISelectStage(false);
            GameData.isStageUI = false;
        }
        else
        {
            uiManager.ActiveUISelectStage(true);
            GameData.isStageUI = true;
        }
        AudioMng.Instance.PlayUIEffect("item_pickup_swipe_01");
    }

    public void OpenHeroShop()
    {
        if (GameData.isShopUI == true)
        {
            GameData.isShopUI = false;
            GameData.selectShopHeroID = -1;
            uiManager.ActiveUIHeroShop(false);
        }
        else
        {
            GameData.isShopUI = true;
            uiManager.ActiveUIHeroShop(true);
        }
        AudioMng.Instance.PlayUIEffect("item_pickup_swipe_01");
    }

    public void OpenSystemMenu()
    {
        if (GameData.isSystemUI == false)
        {
            GameData.isSystemUI = true;
            systemMenu.gameObject.SetActive(true);
        }
        else
        {
            GameData.isSystemUI = false;
            systemMenu.gameObject.SetActive(false);
        }
        AudioMng.Instance.PlayUIEffect("item_pickup_swipe_01");
    }
}
