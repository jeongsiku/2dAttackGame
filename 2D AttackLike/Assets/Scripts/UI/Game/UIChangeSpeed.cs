using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChangeSpeed : MonoBehaviour
{
    GameManager gameManager;

    Button speedOne;
    Button speedTwo;
    Button speedMax;

    Sprite speedOneOn;
    Sprite speedOneOff;
    Sprite speedTwoOn;
    Sprite speedTwoOff;
    Sprite speedMaxOn;
    Sprite speedMaxOff;

    TMP_Text keyOne;
    TMP_Text keyTwo;
    TMP_Text keyMax;

    Color onColor = new Color(0.72f, 0.72f, 0, 1);
    Color offColor = new Color(0.74f, 0.74f, 0.74f, 1);

    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();

        speedOne = transform.Find("X1").GetComponent<Button>();
        speedOne?.onClick.AddListener(ChangeSpeedOne);
        speedTwo = transform.Find("X2").GetComponent<Button>();
        speedTwo?.onClick.AddListener(ChangeSpeedTwo);
        speedMax = transform.Find("X4").GetComponent<Button>();
        speedMax?.onClick.AddListener(ChangeSpeedMax);

        speedOneOn = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Play_Yellow");
        speedOneOff = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Play_Gray");
        speedTwoOn = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Fast-forward_Yellow");
        speedTwoOff = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Fast-forward_Gray");
        speedMaxOn = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Plus_Yellow");
        speedMaxOff = Resources.Load<Sprite>("Images/Game/Simple_ICON_BTN_Plus_Gray");

        keyOne = transform.Find("X1/key").GetComponent<TMP_Text>();
        keyTwo = transform.Find("X2/key").GetComponent<TMP_Text>();
        keyMax = transform.Find("X4/key").GetComponent<TMP_Text>();

        ChangeSpeedOne();
    }

    public void ChangeSpeedOne()
    {
        speedOne.image.sprite = speedOneOn;
        speedTwo.image.sprite = speedTwoOff;
        speedMax.image.sprite = speedMaxOff;

        keyOne.color = onColor;
        keyTwo.color = offColor;
        keyMax.color = offColor;

        GameData.gameSpeed = 1;
        gameManager.ChangeGameSpeed();
    }

    public void ChangeSpeedTwo()
    {
        speedOne.image.sprite = speedOneOff;
        speedTwo.image.sprite = speedTwoOn;
        speedMax.image.sprite = speedMaxOff;

        keyOne.color = offColor;
        keyTwo.color = onColor;
        keyMax.color = offColor;

        GameData.gameSpeed = 2;
        gameManager.ChangeGameSpeed();
    }

    public void ChangeSpeedMax()
    {
        speedOne.image.sprite = speedOneOff;
        speedTwo.image.sprite = speedTwoOff;
        speedMax.image.sprite = speedMaxOn;

        keyOne.color = offColor;
        keyTwo.color = offColor;
        keyMax.color = onColor;

        GameData.gameSpeed = 4;
        gameManager.ChangeGameSpeed();
    }

}
