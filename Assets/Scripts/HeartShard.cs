using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartShard : MonoBehaviour
{
    public Image fill;
    public float targetFillAmount;
    public float lerpDuration = 1.5f;
    public float initialFillAmount;
    
    public IEnumerator LerpFill(){
        float elapsedTime = 0f;
        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime/lerpDuration);
            float lerpedFillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, 1);
            fill.fillAmount = lerpedFillAmount;
            yield return null;   
        }
        fill.fillAmount = targetFillAmount;
        if (fill.fillAmount == 1)   
        {
            PlayerController.Instance.maxHp++;
            PlayerController.Instance.OnhealthChangeCallBack();
            PlayerController.Instance.heartShards = 0;  
            PlayerController.Instance.Health = PlayerController.Instance.maxHp;
        }
    }
}
