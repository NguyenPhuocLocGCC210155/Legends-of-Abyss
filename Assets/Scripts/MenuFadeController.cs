using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MonoBehaviour
{
    private FadeUI fadeUI;
    string scene;
    [SerializeField] ConfirmPopup confirmPopup;
    [SerializeField] float fadeTime;
    private string saveFilePath;
    private void Start()
    {
        fadeUI = GetComponent<FadeUI>();
        fadeUI.FadeUiOut(fadeTime);
    }

    public void CallFadeAndStartGame(string filename)
    {
        if (SaveFileManager.Instance != null)
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, filename + ".json");
            SaveFileManager.Instance.SaveFileName = saveFilePath;
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                GameData data = JsonUtility.FromJson<GameData>(json);
                scene = data.saveScene;
            }else{
                scene = "forest_1";
            }
            Debug.Log(scene);
            StartCoroutine(FadeAndStartGame(scene));
        }
        else
        {
            Debug.Log("Null save file manager");
        }
    }

    IEnumerator FadeAndStartGame(string sceneToLoad)
    {
        fadeUI.FadeUiIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ConfirmClear(string filename){
        confirmPopup.gameObject.SetActive(true);
        confirmPopup.saveFileName = filename;
    }
}
