using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Desaparecer : MonoBehaviour
{
    [SerializeField] CanvasGroup myUI;
    bool fadeAway;

    void Update()
    {
        if (!fadeAway) StartCoroutine(Delay());
        if (myUI.alpha >= 0) myUI.alpha -= Time.deltaTime;
    }

    IEnumerator Delay()
    {
        fadeAway = true;
        yield return new WaitUntil(() => myUI.alpha == 0);
        gameObject.SetActive(false);
        myUI.alpha = 1;
        fadeAway = false;
    }
}
