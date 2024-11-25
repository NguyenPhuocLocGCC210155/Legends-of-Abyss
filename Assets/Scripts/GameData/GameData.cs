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
    public List<string> breakwalls;
    public List<string> unlockSkills;
    public List<string> npcName;
    public string[] equippedSkill;
    public string saveScene;
    public Vector2 savePoint;
    

    //Player Abilities
    public bool isUnlockDash;
    public bool isUnlockWallJump;
    public bool isUnlockDoubleJump;
    public bool isUnlockLantern;
    public bool isUnlockMedal;
    public bool isUnlockPosion;

    //Player count items
    public int playerMaxHP;
    public int shardsCount;
    public int chamberCount;

    public GameData(List<string> scenes)
    {
        unlockedMaps = scenes;
        this.unlockShards = GameManager.Instance.unlockShards;
        this.saveScene = GameManager.Instance.saveScene;
        this.savePoint = GameManager.Instance.savePoint;
        this.bossDefeated = GameManager.Instance.bossDefeated;
        this.breakwalls = GameManager.Instance.breakwalls;
        this.isUnlockDash = PlayerController.Instance.isUnlockDash;
        this.isUnlockWallJump = PlayerController.Instance.isUnlockWallJump;
        this.isUnlockDoubleJump = PlayerController.Instance.isUnlockDoubleJump;
        this.shardsCount = PlayerController.Instance.heartShards;
        this.playerMaxHP = PlayerController.Instance.maxHp;
        this.chamberCount = PlayerController.Instance.chamberCount;
        this.unlockSkills = PlayerController.Instance.skillManager.GetSkillUnlocked();
        this.equippedSkill = PlayerController.Instance.skillManager.GetEquippedSkill();
        this.npcName = GameManager.Instance.npcName;
    }
}
