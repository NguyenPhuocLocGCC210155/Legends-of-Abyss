using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeUiIn(float second){
        StartCoroutine(FadeIn(second));
    }

    public void FadeUiOut(float second){
        StartCoroutine(FadeOut(second));
    }

    IEnumerator FadeOut(float second){
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime/second;
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeIn(float second){
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime/second;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        yield return null;
    }
}
