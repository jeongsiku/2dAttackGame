using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    Image black;

    public void Init()
    {
        black = GetComponentInChildren<Image>(true);
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator IEFade(Color start, Color end, float speed)
    {
        black.gameObject.SetActive (true);
        float elapsed = 0;
        while (true)
        {
            elapsed += Time.deltaTime /  speed;
            black.color = Color.Lerp(start, end, elapsed);
            if (elapsed >= 1.0f) break;
            yield return null;
        }
    }

    public void FadeIn(float speed = 0.5f)
    {
        StartCoroutine(IEFade(Color.black, new Color(0,0,0,0), speed));
        Invoke("DeactiveBlack", speed);
    }

    void DeactiveBlack()
    {
        black.gameObject.SetActive(false);
    }

    public void FadeOut(float speed = 0.5f)
    {
        StartCoroutine(IEFade(new Color(0, 0, 0, 0), Color.black, speed));
    }
}
