using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MonoBehaviour
{
    private FadeUI fadeUI;
    Vector2 pos;
    string scene;

    [SerializeField] float fadeTime;
    private string saveFilePath;
    private void Start()
    {
        fadeUI = GetComponent<FadeUI>();
        fadeUI.FadeUiOut(fadeTime);
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log(saveFilePath);
    }

    public void CallFadeAndStartGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            pos = data.savePoint;
            scene = data.saveScene;
        }
        StartCoroutine(FadeAndStartGame(scene, pos));
    }

    IEnumerator FadeAndStartGame(string sceneToLoad, Vector2 positionToLoad)
    {
        fadeUI.FadeUiIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
