using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;
    UIGameMenu uiGameMenu;
    UITownMenu uiTownMenu;
    UIHeroStat uiHeroStat;
    UIChangeSpeed uiChangeSpeed;

    bool escDown = false;
    bool barDown = false;
    
    bool oneDown = false;
    bool twoDown = false;
    bool threeDown = false;

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (GameData.nowScene == SceneList.Town)
        {
            uiTownMenu = FindObjectOfType<UITownMenu>();
        }

        if (GameData.nowScene == SceneList.Game)
        {
            uiGameMenu = FindObjectOfType<UIGameMenu>();
        }

        uiHeroStat = FindObjectOfType<UIHeroStat>(true);
        uiChangeSpeed = FindObjectOfType<UIChangeSpeed>();

    }

    void GetInputInTown()
    {
        escDown = Input.GetKeyDown(KeyCode.Escape);
    }

    void ESC()
    {
        if(escDown)
        {
            if(GameData.isSystemUI)
            {
                uiTownMenu?.OpenSystemMenu();
                return;
            }
            if(GameData.isStatUI)
            {
                uiHeroStat.gameObject.SetActive(false);
                GameData.isStatUI = false;
                return;
            }
            if (GameData.isShopUI)
            {
                uiTownMenu.OpenHeroShop();
                return;
            }

            if(GameData.isStageUI)
                uiTownMenu.OpenSelectStage();
        }
    }

    void ESCInGame()
    {
        if (escDown)
        {
            if (GameData.isSystemUI)
            {
                uiGameMenu?.OpenSystemMenu();
                return;
            }
            if (GameData.isStatUI)
            {
                uiHeroStat.gameObject.SetActive(false);
                GameData.isStatUI = false;
                return;
            }
        }
    }

    void GetInputInGame()
    {
        escDown = Input.GetKeyDown(KeyCode.Escape);
        barDown = Input.GetKeyDown(KeyCode.Space);
        oneDown = Input.GetKeyDown(KeyCode.Alpha1);
        twoDown = Input.GetKeyDown(KeyCode.Alpha2);
        threeDown = Input.GetKeyDown(KeyCode.Alpha3);
    }

    void SpaceBar()
    {
        if(barDown)
        {
            uiGameMenu.SwitchPlay();
        }
    }

    void AlphaDown()
    {
        if (GameData.isBattle)
        {
            if (oneDown)
            {
                uiChangeSpeed.ChangeSpeedOne();
            }
            else if (twoDown)
            {
                uiChangeSpeed.ChangeSpeedTwo();
            }

            else if (threeDown)
            {
                uiChangeSpeed.ChangeSpeedMax();
            }
            
        }
        
        
    }

    void Update()
    {
        if(GameData.nowScene == SceneList.Game)
        {
            GetInputInGame();
            SpaceBar();
            AlphaDown();
            ESCInGame();
        }

        if (GameData.nowScene == SceneList.Town)
        {
            GetInputInTown();
            ESC();
        }

        
    }
}
