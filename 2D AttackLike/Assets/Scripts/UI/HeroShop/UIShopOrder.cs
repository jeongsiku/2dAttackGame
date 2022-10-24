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
            gameManager.ShowSystemMsg("장수를 선택 해주세요");
            return;
        }
        
        int currHeroCost = DataManager.ToI(TableType.HeroTable, GameData.selectShopHeroID, "COST");
        if(currHeroCost > PlayerData.playerGold)
        {
            gameManager.ShowSystemMsg("금이 부족합니다");
            return;
        }

        if(PlayerData.CheckEmptySlot() == 4)
        {
            gameManager.ShowSystemMsg("빈자리가 없습니다");
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
        gameManager.ShowSystemMsg($"{DataManager.ToS(TableType.HeroTable, GameData.selectShopHeroID, "NAME")}이(가) 합류했습니다");
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
        gameManager.ReloadHeroButton();
        GameData.countHeroInShop--;
        gameManager.RefreshHeroShop();
    }
}
