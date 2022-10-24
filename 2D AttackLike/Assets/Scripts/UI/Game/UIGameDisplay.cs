using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameDisplay : MonoBehaviour
{
    TMP_Text playerGold;

    public void Init()
    {
        playerGold = transform.Find("PlayerGold/Gold").GetComponent<TMP_Text>();
        RefreshGold();
    }

    public void SetGold(int gold)
    {
        playerGold.text = string.Format($"{gold:N0}");
    }

    public void RefreshGold()
    {
        playerGold.text = string.Format($"{PlayerData.playerGold:N0}");
    }

}
