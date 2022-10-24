using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    public CharacterState charState;

    public void SetTrigger(string triggerName)
    {
        charState.SetTrigger(triggerName);
    }


}
