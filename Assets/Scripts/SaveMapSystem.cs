using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveMapSystem : MonoBehaviour
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
            SaveMapData data = JsonUtility.FromJson<SaveMapData>(json);
            GameManager.Instance.savedMap = data.unlockedMap;
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
        SaveMapData data = new SaveMapData(GameManager.Instance.savedMap);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

}
