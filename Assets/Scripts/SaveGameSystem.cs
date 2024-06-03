using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveGameSystem : MonoBehaviour
{
    private string saveFilePath;

    private void Start() {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log(saveFilePath);
        LoadProgress();
    }

    public void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            GameManager.Instance.savedMap = data.unlockedMap;
            GameManager.Instance.saveScene = data.saveScene;
            GameManager.Instance.savePoint = data.savePoint;
        }
        else
        {
            Debug.Log("No save file found, starting new game.");
        }
    }

    public void SaveProgress()
    {
        foreach(string map in GameManager.Instance.unlockedMap){
            if (!GameManager.Instance.savedMap.Contains(map))
            {
                GameManager.Instance.savedMap.Add(map);
            }
        }
        GameData data = new GameData(GameManager.Instance.savedMap, GameManager.Instance.saveScene, GameManager.Instance.savePoint);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

}