using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMngInGame : MonoBehaviour
{
    UIGameDisplay uiGameDisplay;
    UISystemMsg uiSystemMsg;
    UIGameMenu uiGameMenu;
    UIHeroStat uiHeroStat;

    public void Init()
    {
        uiGameDisplay = FindObjectOfType<UIGameDisplay>();
        uiGameDisplay?.Init();

        uiSystemMsg = FindObjectOfType<UISystemMsg>();
        uiSystemMsg?.Init();

        uiGameMenu = FindObjectOfType<UIGameMenu>();
        uiGameMenu?.Init();

        uiHeroStat = FindObjectOfType<UIHeroStat>(true);
        uiHeroStat?.Init();
    }

    public void SetGoldUI(int gold)
    {
        uiGameDisplay.SetGold(gold);
    }

    public void ShowSystemMsg(string text)
    {
        uiSystemMsg.ShowMsg(text);
    }

    public void RefreshGold()
    {
        uiGameDisplay.RefreshGold();
    }

    public void OpenClearPanel(bool state)
    {
        uiGameMenu.OpenClearPanel(state);
    }

    public void OpenFailPanel(bool state)
    {
        uiGameMenu.OpenFailPanel(state);
    }

    public void OpenHeroStat(HeroInfo hero)
    {
        if (!GameData.isStatUI)
        {
            GameData.isStatUI = true;
            uiHeroStat.Setting(hero);
            uiHeroStat.gameObject.SetActive(true);
        }
        else
        {
            GameData.isStatUI = false;
            GameData.nowHeroStatPanel = 0;
            uiHeroStat.gameObject.SetActive(false);
        }
    }

    public void UpdateHeroStat(HeroInfo hero)
    {
        uiHeroStat.UpdateStat(hero);
    }

    public void UpdateHeroBar(HeroInfo hero)
    {
        uiHeroStat.UpdateBar(hero);
    }

    public void UpdateHeroCost(HeroInfo hero)
    {
        uiHeroStat.RefreshAllCost(hero);
    }

    public void CloseSystemMenu()
    {
        uiGameMenu.OpenSystemMenu();
    }
}
