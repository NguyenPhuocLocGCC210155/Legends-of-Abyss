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
            GameManager.Instance.savedMap = data.unlockedMaps;
            GameManager.Instance.saveScene = data.saveScene;
            GameManager.Instance.savePoint = data.savePoint;
            GameManager.Instance.unlockShards = data.unlockShards;
            if (data.playerMaxHP > 0)
            {
                PlayerController.Instance.maxHp = data.playerMaxHP;
                PlayerController.Instance.Health = PlayerController.Instance.maxHp;
            }
            PlayerController.Instance.isUnlockDash = data.isUnlockDash;
            PlayerController.Instance.isUnlockWallJump = data.isUnlockWallJump;
            PlayerController.Instance.isUnlockDoubleJump = data.isUnlockDoubleJump;
            PlayerController.Instance.heartShards = data.shardsCount;
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
        GameData data = new GameData(GameManager.Instance.savedMap);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

}
