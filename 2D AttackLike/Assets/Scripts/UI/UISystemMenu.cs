using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemMenu : MonoBehaviour
{
    Button exit;
    Button continueGame;
    GameManager gameManager;
    public void Init()
    {
        exit = transform.Find("EXIT").GetComponent<Button>();
        exit?.onClick.AddListener(Exit);

        continueGame = transform.Find("CONTINUE").GetComponent<Button>();
        continueGame?.onClick.AddListener(Continue);

        gameManager = FindObjectOfType<GameManager>();
    }

    void Continue()
    {
        gameManager.CloseSystemMenu();
    }

    void Exit()
    {
        gameManager.ExitGame();
    }

    
}
