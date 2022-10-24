using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroShop : MonoBehaviour
{
    UIHeroShopButton heroShopButtonPrefab;
    GridLayoutGroup gridLayoutGroup;

    List<UIHeroShopButton> heroButtons = new List<UIHeroShopButton>();
    Dictionary<int, UIHeroShopButton> heroDic = new Dictionary<int, UIHeroShopButton>();

    UIShopOrder uiShopOrder;
    MyHeroManager myHeroManager;

    public void Init()
    {
        myHeroManager = FindObjectOfType<MyHeroManager>();

        heroShopButtonPrefab = Resources.Load<UIHeroShopButton>("Prefabs/UI/HeroShop");
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>(true);

        DataManager.SetHeroDic(TableType.HeroTable);
        CreateEmptySlot(GameData.countHeroInShop);
        SetHeroList(DataManager.GetHeroes());

        uiShopOrder = GetComponentInChildren<UIShopOrder>(true);
        uiShopOrder?.Init();

    }

    public void CreateEmptySlot(int count)
    {
        heroButtons.Clear();
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            Destroy(gridLayoutGroup.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            UIHeroShopButton heroButton = Instantiate(heroShopButtonPrefab, gridLayoutGroup.transform);
            heroButton.Init();
            heroButtons.Add(heroButton);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < heroButtons.Count; i++)
            heroButtons[i].ActiveHeroButton(false);
    }

    public void SetHeroList(List<HeroInfo> heroList)
    {
        heroDic.Clear();
        for (int i = 0; i < heroList.Count; i++)
        {
            if (myHeroManager.CheckOwnHero(heroList[i].charID))
                heroList.Remove(heroList[i]);
            heroButtons[i].SetInfo(heroList[i]);
            heroDic.Add(heroList[i].charID, heroButtons[i]);
        }
    }

    public void ClearFocus()
    {
        foreach(var button in heroButtons)
            button.ActiveFocus(false);
    }

    public void RefreshShop()
    {
        ClearFocus();
        CreateEmptySlot(GameData.countHeroInShop);
        SetHeroList(DataManager.GetHeroes());
    }
}
