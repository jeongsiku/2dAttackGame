using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameMenu : MonoBehaviour
{
    GameManager gameManager;
    
    Button startButton;
    Button playButton;
    Sprite playButtonImage;
    Sprite stopButtonImage;

    GameObject clearPanel;
    GameObject failPanel;
    Button goTown;
    Button goTownWithFail;

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

    UIChangeSpeed uiChangeSpeed;

    Button retreat;

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();

        startButton = transform.Find("Start").GetComponent<Button>();
        startButton?.onClick.AddListener(StartBattle);
        startButton.gameObject.SetActive(true);

        playButton = transform.Find("Play").GetComponent<Button>();
        playButton?.onClick.AddListener(SwitchPlay);

        playButtonImage = Resources.Load<Sprite>("Images/Game/TextBTN_Play");
        stopButtonImage = Resources.Load<Sprite>("Images/Game/TextBTN_Pause");

        clearPanel = transform.Find("End").gameObject;
        goTown = transform.Find("End/GoTown").GetComponent<Button>();
        goTown?.onClick.AddListener(MoveToTown);

        failPanel = transform.Find("EndwithFail").gameObject;
        goTownWithFail = transform.Find("EndwithFail/GoTown").GetComponent<Button>();
        goTownWithFail?.onClick.AddListener(MoveToTownWithFail);

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

        uiChangeSpeed = transform.Find("SpeedGroup").GetComponent<UIChangeSpeed>();
        uiChangeSpeed?.Init();

        retreat = transform.Find("Retreat").GetComponent<Button>();
        retreat?.onClick.AddListener(Retreat);

        SetHeroButton();
    }

    void SetHeroButton()
    {
        List<Hero> heroList = gameManager.GetHeroList();
        for(int i=0; i< heroList.Count; i++)
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

    public void OpenHero1Stat(){ gameManager.OpenHeroStat(1); }
    public void OpenHero2Stat(){ gameManager.OpenHeroStat(2); }
    public void OpenHero3Stat(){ gameManager.OpenHeroStat(3); }
    public void OpenHero4Stat(){ gameManager.OpenHeroStat(4); }

    void StartBattle()
    {
        AudioMng.Instance.PlayUIEffect("ui_button_simple_click_07");
        GameData.isBattle = true;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager?.StartBattle();
        startButton.gameObject.SetActive(false);
    }


    public void SwitchPlay()
    {
        if(GameData.isBattle)
        {
            GameData.isBattle = false;
            playButton.image.sprite = playButtonImage;
            gameManager.ActiveBattle(false);
        }
        else
        {
            GameData.isBattle = true;
            playButton.image.sprite = stopButtonImage;
            gameManager.ActiveBattle(true);
        }
        AudioMng.Instance.PlayUIEffect("ui_button_simple_click_07");
    }

    public void OpenClearPanel(bool state)
    {
        clearPanel.gameObject.SetActive(state);
    }
    
    public void OpenFailPanel(bool state)
    {
        failPanel.gameObject.SetActive(state);
    }

    void MoveToTown()
    {
        gameManager.UpdateISClearStage(GameData.currStageIdx);
        AudioMng.Instance.PlayUIEffect("collect_item_jingle_02");
        gameManager.SaveToPlayerData();
        gameManager.MoveToScene("TownScene");
    }

    void MoveToTownWithFail()
    {
        AudioMng.Instance.PlayUIEffect("collect_item_jingle_02");
        gameManager.SaveToPlayerData();
        gameManager.MoveToScene("TownScene");
    }

    public void OpenSystemMenu()
    {
        SwitchPlay();
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

    void Retreat()
    {
        AudioMng.Instance.PlayUIEffect("collect_item_jingle_02");
        gameManager.SaveToPlayerData();
        gameManager.MoveToScene("TownScene");
    }


}
