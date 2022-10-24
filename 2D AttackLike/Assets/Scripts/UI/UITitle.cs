using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitle : MonoBehaviour
{
    Button start;
    Button continueGame;
    Button exit;

    public void Init()
    {
        GameData.nowScene = SceneList.Title;
        start = transform.Find("ButtonGroup/Start").GetComponent<Button>();
        start.onClick.AddListener(StartTownScene);
        continueGame = transform.Find("ButtonGroup/Continue").GetComponent<Button>();
        continueGame.onClick.AddListener(StartWithLoad);
        exit = transform.Find("ButtonGroup/Exit").GetComponent<Button>();
        exit.onClick.AddListener(ExitGame);
    }

    void StartTownScene()
    {
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
        UIFade uiFade = FindObjectOfType<UIFade>();
        uiFade?.FadeOut();
        Invoke("LoadTownScene", 0.6f);
    }

    void LoadTownScene()
    {
        SceneManager.LoadSceneAsync("TownScene");
    }

    void StartWithLoad()
    {
        GameData.hasLoadData = true;
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
        UIFade uiFade = FindObjectOfType<UIFade>();
        uiFade?.FadeOut();
        Invoke("LoadTownScene", 0.6f);
    }

    void ExitGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.ExitGame();
    }
}
