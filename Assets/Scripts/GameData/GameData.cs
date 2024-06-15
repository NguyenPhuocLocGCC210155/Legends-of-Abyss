using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> unlockedMaps;
    public List<string> unlockShards;
    public List<BossDefeatedData> bossDefeated;
    public string saveScene;
    public Vector2 savePoint;
    

    //Player Abilities
    public bool isUnlockDash;
    public bool isUnlockWallJump;
    public bool isUnlockDoubleJump;
    public int playerMaxHP;
    public int shardsCount;

    public GameData(List<string> scenes)
    {
        unlockedMaps = scenes;
        this.unlockShards = GameManager.Instance.unlockShards;
        this.saveScene = GameManager.Instance.saveScene;
        this.savePoint = GameManager.Instance.savePoint;
        this.bossDefeated = GameManager.Instance.bossDefeated;
        this.isUnlockDash = PlayerController.Instance.isUnlockDash;
        this.isUnlockWallJump = PlayerController.Instance.isUnlockWallJump;
        this.isUnlockDoubleJump = PlayerController.Instance.isUnlockDoubleJump;
        this.shardsCount = PlayerController.Instance.heartShards;
        this.playerMaxHP = PlayerController.Instance.maxHp;
    }
}
