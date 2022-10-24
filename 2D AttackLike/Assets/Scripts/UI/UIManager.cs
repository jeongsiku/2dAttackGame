using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UITownMenu uiTownMenu;
    UISelectStage uiSelectStage;
    UIHeroShop uiHeroShop;
    UITownDisplay uiTownDisplay;
    UISystemMsg uiSystemMsg;
    UIHeroStat uiHeroStat;

    public void Init()
    {
        uiTownMenu = GetComponentInChildren<UITownMenu>();
        uiTownMenu?.Init();

        uiHeroShop = GetComponentInChildren<UIHeroShop>(true);
        uiHeroShop?.Init();

        uiSelectStage = GetComponentInChildren<UISelectStage>(true);
        uiSelectStage?.Init();

        uiTownDisplay = GetComponentInChildren<UITownDisplay>(true);
        uiTownDisplay?.Init();

        uiSystemMsg = GetComponentInChildren<UISystemMsg>(true);
        uiSystemMsg?.Init();

        uiHeroStat = FindObjectOfType<UIHeroStat>(true);
        uiHeroStat?.Init();
    }

    public bool IsActiveSelectStage()
    {
        return uiSelectStage.isActiveAndEnabled;
    }

    public void ActiveUISelectStage(bool state)
    {
        uiSelectStage.gameObject.SetActive(state);
    }

    public bool IsActiveHeroShop()
    {
        return uiHeroShop.isActiveAndEnabled;
    }

    public void ActiveUIHeroShop(bool state)
    {
        uiHeroShop.gameObject.SetActive(state);
        uiHeroShop.ClearFocus();
    }

    public void SetGoldUI(int gold)
    {
        uiTownDisplay.SetGold(gold);
    }

    public void ShowSystemMsg(string text)
    {
        uiSystemMsg.ShowMsg(text);
    }

    public void RefreshGold()
    {
        uiTownDisplay.RefreshGold();
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

    public void ReloadHeroButton()
    {
        uiTownMenu.SetHeroButton();
    }

    public void RefreshHeroShop()
    {
        uiHeroShop.RefreshShop();
    }
}
