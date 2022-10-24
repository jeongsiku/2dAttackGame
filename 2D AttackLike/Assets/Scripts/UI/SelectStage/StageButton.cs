using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageButton : MonoBehaviour
{
    StageInfo stageInfo;

    new TMP_Text name;
    TMP_Text level;
    Button button;
    
    public void Init()
    {
        name = transform.Find("Name").GetComponent<TMP_Text>();
        level = transform.Find("Level").GetComponent<TMP_Text>();
        button = transform.Find("Button").GetComponent<Button>();
        button?.onClick.AddListener(MoveToStage);
    }

    public void SetInfo(StageInfo info)
    {
        stageInfo = info;
        name.text = stageInfo.stageName;
        level.text = string.Format($"Lv {stageInfo.level} +");
        ActiveStageButton(true);
        
        if (stageInfo.stageId <= GameData.clearStageNum+1)
            button.gameObject.SetActive(true);
        else
            button.gameObject.SetActive(false);

    }

    public void ActiveStageButton(bool state)
    {
        name?.gameObject.SetActive(state);
        level?.gameObject.SetActive(state);
        button?.gameObject.SetActive(state);
    }

    void MoveToStage()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        if(PlayerData.myHeroList.Count == 0)
        {
            gameManager.ShowSystemMsg("출진할 장수가 없습니다.");
            AudioMng.Instance.PlayUIEffect("sci-fi_code_fail_06"); 
            return;
        }    

        AudioMng.Instance.PlayUIEffect("collect_item_jingle_02");
        gameManager?.MoveToStage(stageInfo.stageId);
    }
}
