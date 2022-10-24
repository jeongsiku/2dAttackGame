using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopOrder : MonoBehaviour
{
    Button button;
    GameManager gameManager;
    public void Init()
    {
        button = GetComponentInChildren<Button>(true);
        button?.onClick.AddListener(OrderHero);

        gameManager = FindObjectOfType<GameManager>(true);
    }

    void OrderHero()
    {
        if (GameData.selectShopHeroID == -1)
        {
            gameManager.ShowSystemMsg("����� ���� ���ּ���");
            return;
        }
        
        int currHeroCost = DataManager.ToI(TableType.HeroTable, GameData.selectShopHeroID, "COST");
        if(currHeroCost > PlayerData.playerGold)
        {
            gameManager.ShowSystemMsg("���� �����մϴ�");
            return;
        }

        if(PlayerData.CheckEmptySlot() == 4)
        {
            gameManager.ShowSystemMsg("���ڸ��� �����ϴ�");
            return;
        }

        BuyHero();
    }

    void BuyHero()
    {
        PlayerData.playerGold -= DataManager.ToI(TableType.HeroTable, GameData.selectShopHeroID, "COST");
        gameManager.RefreshGoldUI();
        PlayerData.AddHero(DataManager.GetHero(GameData.selectShopHeroID));
        gameManager.SetHeroTown(GameData.selectShopHeroID);
        gameManager.ShowSystemMsg($"{DataManager.ToS(TableType.HeroTable, GameData.selectShopHeroID, "NAME")}��(��) �շ��߽��ϴ�");
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
        gameManager.ReloadHeroButton();
        GameData.countHeroInShop--;
        gameManager.RefreshHeroShop();
    }
}
