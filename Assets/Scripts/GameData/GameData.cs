using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> unlockedMap;
    public string saveScene;
    public Vector2 savePoint;

    public GameData(List<string> scenes)
    {
        unlockedMap = scenes;
    }

    public GameData(List<string> scenes, string savedScene, Vector2 savePoint)
    {
        unlockedMap = scenes;
        this.saveScene = savedScene;
        this.savePoint = savePoint;
    }
}
