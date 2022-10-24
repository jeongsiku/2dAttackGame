using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystemMsg : MonoBehaviour
{
    TMP_Text msg;

    public void Init()
    {
        msg = transform.Find("Text").GetComponent<TMP_Text>();
        msg?.gameObject.SetActive(false);
    }

    public void ShowMsg(string text)
    {
        msg.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IEShowMsg(text));
    }

    IEnumerator IEShowMsg(string text)
    {
        msg.text = text;
        Color alpha = msg.color;
        alpha.a = 1;
        msg.color = alpha;
        yield return new WaitForSeconds(3);
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            alpha.a = Mathf.Lerp(1,0,time);
            msg.color = alpha;
            yield return null;
        }
        msg.gameObject.SetActive(false);
    }
}
