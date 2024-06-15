using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDefeatedData 
{
    
    public string name;
    public Vector2 position;

    public BossDefeatedData(string name, Vector2 position)
    {
        this.name = name;
        this.position = position;
    }
}
