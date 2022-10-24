using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListner : MonoBehaviour
{
    public void CallEvent(string eventName)
    {
        transform.SendMessageUpwards(eventName, SendMessageOptions.DontRequireReceiver);
    }
}
