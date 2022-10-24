using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHeroShopButton : MonoBehaviour
{
    HeroInfo heroInfo;

    Button button;

    Image background;
    Image face;
    Image focus;

    new TMP_Text name;
    TMP_Text charClass;
    TMP_Text level;
    TMP_Text atk;
    TMP_Text hp;
    TMP_Text atkSpeed;
    TMP_Text heroCost;

    public void Init()
    {
        background = GetComponent<Image>();
        button = GetComponent<Button>();
        button?.onClick.AddListener(ActiveOrderUI);
        face = transform.Find("Face").GetComponent<Image>();
        name = transform.Find("Name").GetComponent<TMP_Text>();
        charClass = transform.Find("Class").GetComponent<TMP_Text>();
        level = transform.Find("Level").GetComponent<TMP_Text>();
        atk = transform.Find("ATK").GetComponent<TMP_Text>();
        atkSpeed = transform.Find("ATKSPEED").GetComponent<TMP_Text>();
        hp = transform.Find("HP").GetComponent<TMP_Text>();
        focus = transform.Find("Focus").GetComponent<Image>();
        focus.gameObject.SetActive(false);
        heroCost = transform.Find("CostRoot/Cost").GetComponent<TMP_Text>();
    }

    public void SetInfo(HeroInfo info)
    {
        heroInfo = info;
        string sprite = DataManager.ToS(TableType.HeroTable, heroInfo.charID, "FACEIMAGE");
        face.sprite = Resources.Load<Sprite>("Images/HeroFace/" + sprite);
        name.text = heroInfo.name;
        switch(heroInfo.charClassType)
        {
            case CharacterClassType.Knight:
                background.sprite = Resources.Load<Sprite>("Images/HeroBg/knight");
                charClass.text = "장수";
                break;
            case CharacterClassType.Assassin:
                background.sprite = Resources.Load<Sprite>("Images/HeroBg/assassin");
                charClass.text = "산적";
                break;
            case CharacterClassType.Archer:
                background.sprite = Resources.Load<Sprite>("Images/HeroBg/archer");
                charClass.text = "궁수";
                break;
            case CharacterClassType.Tactician:
                background.sprite = Resources.Load<Sprite>("Images/HeroBg/tactician");
                charClass.text = "책사";
                break;
        }
        level.text = "LV " + heroInfo.level;
        atk.text = "ATK : " + heroInfo.attackPower;
        hp.text = "HP : " + heroInfo.healthPower;
        atkSpeed.text = "SP : " + heroInfo.attackInterval;
        heroCost.text = string.Format($"{heroInfo.heroCost:N0}");


    }

    public void ActiveHeroButton(bool state)
    {
        background.gameObject.SetActive(state);
        face.gameObject.SetActive(state);
        charClass.gameObject.SetActive(state);
        name.gameObject.SetActive(state);
        level.gameObject.SetActive(state);
        atk.gameObject.SetActive(state);
        hp.gameObject.SetActive(state);
        atkSpeed.gameObject.SetActive(state);
        heroCost.gameObject.SetActive(state);
    }

    public void ActiveFocus(bool state)
    {
        focus.gameObject.SetActive(state);
    }

    void ActiveOrderUI()
    {
        UIHeroShop uiHeroShop = FindObjectOfType<UIHeroShop>();
        uiHeroShop?.ClearFocus();
        ActiveFocus(true);
        GameData.selectShopHeroID = heroInfo.charID;
    }
}
