using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MonoBehaviour
{
    private FadeUI fadeUI;

    [SerializeField] float fadeTime;

    private void Start() {
        fadeUI = GetComponent<FadeUI>();
        fadeUI.FadeUiOut(fadeTime);
    }

    public void CallFadeAndStartGame(string scene){
        StartCoroutine(FadeAndStartGame(scene));
    }

    IEnumerator FadeAndStartGame(string sceneToLoad){
        fadeUI.FadeUiIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);

    }
}
