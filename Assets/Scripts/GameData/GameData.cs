using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> unlockedMap;
    public string saveScene;
    public Vector2 savePoint;
    

    //Player Abilities
    public bool isUnlockDash;
    public bool isUnlockWallJump;
    public bool isUnlockDoubleJump;
    public int playerMaxHP;
    public int shardsCount;

    public GameData(List<string> scenes, string savedScene, Vector2 savePoint)
    {
        unlockedMap = scenes;
        this.saveScene = savedScene;
        this.savePoint = savePoint;
        this.isUnlockDash = PlayerController.Instance.isUnlockDash;
        this.isUnlockWallJump = PlayerController.Instance.isUnlockWallJump;
        this.isUnlockDoubleJump = PlayerController.Instance.isUnlockDoubleJump;
        this.shardsCount = PlayerController.Instance.heartShards;
        this.playerMaxHP = PlayerController.Instance.maxHp;
    }
}
