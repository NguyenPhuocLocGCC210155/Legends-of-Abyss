using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disslove : MonoBehaviour
{
    public float dissloveTime = 0.75f;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private int dissloveAmount = Shader.PropertyToID("_DissloveAmount");
    private int verticalDissloveAmount = Shader.PropertyToID("_VerticalDisslove");

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void Vanish(bool useDissolve, bool useVertical){
        StartCoroutine(StartVanish(useDissolve, useVertical));
    }

    public void Appear(bool useDissolve, bool useVertical){
        StartCoroutine(StartAppear(useDissolve, useVertical));
    }

    private IEnumerator StartVanish(bool useDissolve, bool useVertical){
        float elapsedTime = 0f;
        while (elapsedTime < dissloveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime/dissloveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime/dissloveTime));

            if (useDissolve)
            {
                material.SetFloat(dissloveAmount, lerpedDissolve);
            }

            if (useVertical)
            {
                material.SetFloat(verticalDissloveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }

    private IEnumerator StartAppear(bool useDissolve, bool useVertical){
        float elapsedTime = 0f;
        while (elapsedTime < dissloveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime/dissloveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0, (elapsedTime/dissloveTime));

            if (useDissolve)
            {
                material.SetFloat(dissloveAmount, lerpedDissolve);
            }

            if (useVertical)
            {
                material.SetFloat(verticalDissloveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }
}
