using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    UIManager uiManager;
    UIMngInGame uiMngInGame;
    UIFade uiFade;
    MySceneManager mySceneManager;
    MyHeroManager myHeroManager;
    EnemySpawner enemySpawner;
    InputManager inputManager;

    void Start()
    {
        DataManager.Load(TableType.StageTable);
        DataManager.Load(TableType.HeroTable);
        DataManager.Load(TableType.MonsterTable);
        DataManager.Load(TableType.NewEnemySpawnListTable);
        DataManager.Load(TableType.CharLevelTable);
        DataManager.Load(TableType.UpgradeTable);

        AudioMng.Instance.Init();

        if (GameData.hasLoadData)
        {
            Load();
            GameData.hasLoadData = false;
        }

        PlayBGM();

        if (myHeroManager != null)
        {

        }
        else
        {
            myHeroManager = FindObjectOfType<MyHeroManager>();
            myHeroManager?.Init();
        }
        
        enemySpawner = FindObjectOfType<EnemySpawner>();

        mySceneManager = FindObjectOfType<MySceneManager>();
        mySceneManager?.Init();

        uiManager = FindObjectOfType<UIManager>(); 
        uiManager?.Init();

        uiMngInGame = FindObjectOfType<UIMngInGame>();
        uiMngInGame?.Init();

        inputManager = FindObjectOfType<InputManager>();
        inputManager?.Init();

        
        uiFade = GameObject.FindObjectOfType<UIFade>();
        if(uiFade == null)
        {
            uiFade = Instantiate(Resources.Load<UIFade>("Prefabs/UI/UIFade"));
            uiFade?.Init();
            DontDestroyOnLoad(uiFade);
        }
        else
        {
            uiFade.FadeIn();
        }
    }

    private void Update()
    {
        if(GameData.nowHeroStatPanel != 0)
        {
            UpdateHeroBar();
        }
    }

    public void MoveToStage(int stageIdx)
    {
        GameData.currStageIdx = stageIdx;
        uiManager.ActiveUISelectStage(false);
        uiFade.FadeOut();
        Invoke("LoadGameScene", 0.6f);
    }

    public void FadeIn()
    {

    }

    public void FadeOut()
    {
        uiFade.FadeOut();
    }

    public void ExitGame()
    {
        Save();

        if(SceneManager.GetActiveScene().name != "TitleScene")
            SaveToPlayerData();
        FadeOut();
        Invoke("Quit", 0.6f);
    }

    public void Save()
    {
        SaveManager.Save("GameSave.json");
    }

    public void Load()
    {
        SaveManager.Load("GameSave.json");
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MoveToScene(string sceneName)
    {
        uiFade.FadeOut();
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName, float time = 1)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void ShowSystemMsg(string text)
    {
        uiManager?.ShowSystemMsg(text);
        uiMngInGame?.ShowSystemMsg(text);
    }

    public void RefreshGoldUI()
    {
        uiManager?.RefreshGold();
        uiMngInGame?.RefreshGold();
    }

    public void SetHeroTown(int charID)
    {
        if(myHeroManager == null)
            myHeroManager = FindObjectOfType<MyHeroManager>();
        myHeroManager.AddHero(charID);
    }

    public void StartBattle()
    {
        myHeroManager.StartBattle();
        enemySpawner.SpawnMonster();
    }

    public void ActiveBattle(bool state)
    {
        myHeroManager.ActiveBattle(state);
        enemySpawner.ActiveBattle(state);
    }

    void PlayBGM()
    {
        AudioMng.Instance.CheckPlayBGM();
    }

    public void OpenClearPanel(bool state)
    {
        uiMngInGame.OpenClearPanel(state);
    }

    public void OpenFailPanel(bool state)
    {
        uiMngInGame.OpenFailPanel(true);
    }
    

    public List<Hero> GetHeroList()
    {
        return myHeroManager.GetHeroList();
    }

    public void OpenHeroStat(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        GameData.nowHeroStatPanel = heroNum;
        uiMngInGame?.OpenHeroStat(heroList[heroNum-1].GetInfo());
        uiManager?.OpenHeroStat(heroList[heroNum-1].GetInfo());
    }

    public void UpdateHeroStatAll()
    {
        UpdateHeroStatPanel();
        UpdateHeroStatCost();
    }

    public void UpdateHeroStatPanel()
    {
        UpdateHeroBar();
        UpdateHeroStat();
    }

    public void UpdateHeroStatCost()
    {
        UpdateHeroCost();
    }

    public void UpdateHeroBar()
    {
        if (GameData.nowHeroStatPanel == 0) return;
        List<Hero> heroList = myHeroManager.GetHeroList();
        int heroNum= GameData.nowHeroStatPanel;
        uiMngInGame?.UpdateHeroBar(heroList[heroNum - 1].GetInfo());
        uiManager?.UpdateHeroBar(heroList[heroNum - 1].GetInfo());
    }

    public void UpdateHeroStat()
    {
        if (GameData.nowHeroStatPanel == 0) return;
        List<Hero> heroList = myHeroManager.GetHeroList();
        int heroNum = GameData.nowHeroStatPanel;
        uiMngInGame?.UpdateHeroStat(heroList[heroNum - 1].GetInfo());
        uiManager?.UpdateHeroStat(heroList[heroNum - 1].GetInfo());
    }

    void UpdateHeroCost()
    {
        if (GameData.nowHeroStatPanel == 0) return;
        List<Hero> heroList = myHeroManager.GetHeroList();
        int heroNum = GameData.nowHeroStatPanel;
        uiMngInGame?.UpdateHeroCost(heroList[heroNum - 1].GetInfo());
        uiManager?.UpdateHeroCost(heroList[heroNum - 1].GetInfo());
    }

    public void SaveToPlayerData()
    {
        myHeroManager.SaveToPlayerData();
    }

    public void ReloadHeroButton()
    {
        uiManager.ReloadHeroButton();
    }

    public void UpgrageATK(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());
        info.atkUpgrageLevel++;

        int cost = DataManager.ToI(TableType.UpgradeTable, info.atkUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        int atkPoint = DataManager.ToI(TableType.UpgradeTable, info.atkUpgrageLevel, "ATK");
        info.attackPower += atkPoint;

        FollowUpUpgrage();
    }

    public void UpgrageSPEED(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());

        int cost = DataManager.ToI(TableType.UpgradeTable, info.speedUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        float speedPoint = DataManager.ToF(TableType.UpgradeTable, info.speedUpgrageLevel, "SPEED");
        info.attackInterval += speedPoint;
        info.attackInterval = Mathf.Floor(info.attackInterval * 100) * 0.01f;
        info.speedUpgrageLevel++;
        FollowUpUpgrage();
    }

    public void UpgrageDEF(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());

        int cost = DataManager.ToI(TableType.UpgradeTable, info.defUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        int defPoint = DataManager.ToI(TableType.UpgradeTable, info.defUpgrageLevel, "DEF");
        info.defense += defPoint;

        info.defUpgrageLevel++;
        FollowUpUpgrage();
    }

    public void UpgrageHP(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());
        int cost = DataManager.ToI(TableType.UpgradeTable, info.hpUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        int hpPoint = DataManager.ToI(TableType.UpgradeTable, info.hpUpgrageLevel, "HEALTH");
        info.healthPower += hpPoint;
        info.hpUpgrageLevel++;

        FollowUpUpgrage();
    }

    public void UpgrageREGEN(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());
        int cost = DataManager.ToI(TableType.UpgradeTable, info.regenUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        int regenPoint = DataManager.ToI(TableType.UpgradeTable, info.regenUpgrageLevel, "REGEN");
        info.regen += regenPoint;
        info.regenUpgrageLevel++;

        FollowUpUpgrage();
    }

    public void UpgrageLUCK(int heroNum)
    {
        List<Hero> heroList = myHeroManager.GetHeroList();
        HeroInfo info = (heroList[heroNum - 1].GetInfo());
        int cost = DataManager.ToI(TableType.UpgradeTable, info.luckUpgrageLevel, "COST");
        PlayerData.playerGold -= cost;

        int luckPoint = DataManager.ToI(TableType.UpgradeTable, info.luckUpgrageLevel, "LUCK");
        info.luck += luckPoint;
        info.luckUpgrageLevel++;

        FollowUpUpgrage();
    }

    void FollowUpUpgrage()
    {
        AudioMng.Instance.PlayUIEffect("collect_item_jingle_02");
        UpdateHeroStatAll();
        RefreshGoldUI();
    }

    public void UpdateISClearStage(int currStageIndex)
    {
        if (GameData.clearStageNum >= currStageIndex) return;
        else
        {
            GameData.clearStageNum++;
        }
    }

    public void CloseSystemMenu()
    {
        uiMngInGame.CloseSystemMenu();
    }

    public void RefreshHeroShop()
    {
        uiManager.RefreshHeroShop();
    }

    public void ChangeGameSpeed()
    {
        myHeroManager.ChangeAnimatorSpeed();
        enemySpawner.ChangeAnimatorSpeed();
        AudioMng.Instance.PlayUIEffect("ui_button_simple_click_07");
    }
}
