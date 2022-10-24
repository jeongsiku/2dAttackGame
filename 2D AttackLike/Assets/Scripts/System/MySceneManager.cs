using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    GameManager gameManager;
    UITitle uiTitle;
    Transform backgroundRoot;
    GameObject mapPrefab;
    EnemySpawner enemySpawner;
    MyHeroManager myHeroManager;

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScene":
                {
                    uiTitle = FindObjectOfType<UITitle>();
                    uiTitle.Init();
                }
                break;
            case "TownScene":
                {
                    GameData.nowScene = SceneList.Town;
                }
                break;

            case "GameScene":
                {
                    GameData.nowScene = SceneList.Game;

                    if (GameData.currStageIdx > 1000)
                        SetBackground(GameData.currStageIdx);

                    enemySpawner = FindObjectOfType<EnemySpawner>();
                    enemySpawner?.Init();

                    myHeroManager = FindObjectOfType<MyHeroManager>();
                    // test code. 게임씬에서 실행 시 테스트 영웅 생성되도록
                    if (myHeroManager?.GetHeroList().Count == 0)
                    {
                        PlayerData.AddHero(DataManager.GetHero(2001));
                        PlayerData.AddHero(DataManager.GetHero(2004));
                        PlayerData.AddHero(DataManager.GetHero(2010));
                        PlayerData.AddHero(DataManager.GetHero(2007));
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        gameManager?.SetHeroTown(2001);
                        gameManager?.SetHeroTown(2004);
                        gameManager?.SetHeroTown(2010);
                        gameManager?.SetHeroTown(2007);
                        GameData.countHeroInShop -= 4;
                    }
                    // test end
                }
                break;
        }
    }

    void SetBackground(int stageIdx)
    {
        backgroundRoot = GameObject.Find("BackgroundRoot").transform;
        string stageName = string.Empty;
        stageName = DataManager.ToS(TableType.StageTable, stageIdx, "SMAP");
        mapPrefab = Instantiate(Resources.Load<GameObject>(
            "Prefabs/Backgrounds/" + stageName), backgroundRoot);

    }

    private void OnGUI()
    {
        //if (GUI.Button(new Rect(50, 150, 100, 60), "go to title"))
        //{
        //    UIFade uiFade = FindObjectOfType<UIFade>();
        //    uiFade?.FadeOut();
        //    Invoke("LoadTitleScene", 0.6f);
        //}

        //if (GUI.Button(new Rect(200, 150, 100, 60), "go to town"))
        //{
        //    gameManager.SaveToPlayerData();
        //    UIFade uiFade = FindObjectOfType<UIFade>();
        //    uiFade?.FadeOut();
        //    Invoke("LoadTownScene", 0.6f);
        //}

        //if (GUI.Button(new Rect(350, 150, 100, 60), "go to game"))
        //{
        //    gameManager.SaveToPlayerData();
        //    UIFade uiFade = FindObjectOfType<UIFade>();
        //    uiFade?.FadeOut();
        //    Invoke("LoadGameScene", 0.6f);
        //}
        
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }
    public void LoadTownScene()
    {
        SceneManager.LoadSceneAsync("TownScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
