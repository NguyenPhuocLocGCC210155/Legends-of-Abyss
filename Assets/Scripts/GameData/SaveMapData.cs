using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveMapData
{
    public List<string> unlockedMap;

    public SaveMapData(List<string> scenes)
    {
        unlockedMap = scenes;
    }
}
