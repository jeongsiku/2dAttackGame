using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectStage : MonoBehaviour
{
    StageButton stageButtonPrefab;
    VerticalLayoutGroup verticalLayoutGroup;

    List<StageButton> stageButtons = new List<StageButton>();
    Dictionary<int, StageButton> stageDic = new Dictionary<int, StageButton>();

    public void Init()
    {
        stageButtonPrefab = Resources.Load<StageButton>("Prefabs/UI/Stage");
        verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>(true);

        DataManager.SetStageDic(TableType.StageTable);
        CreateEmptySlot(DataManager.GetStageLength(TableType.StageTable));

        SetStageList(DataManager.GetStages());
    }

    

    public void CreateEmptySlot(int count)
    {
        for(int i = 0; i < count; i++)
        {
            StageButton stageButton = Instantiate(stageButtonPrefab, verticalLayoutGroup.transform);
            stageButton.Init();
            stageButtons.Add(stageButton);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < stageButtons.Count; i++)
            stageButtons[i].ActiveStageButton(false);
    }

    public void SetStageList(List<StageInfo> stageList)
    {
        stageDic.Clear();

        for(int i = 0; i < stageList.Count;i++)
        {
            if(i==0) stageList[i].isClear = true;
            stageButtons[i].SetInfo(stageList[i]);
            stageDic.Add(stageList[i].stageId, stageButtons[i]);
        }
    }
}
