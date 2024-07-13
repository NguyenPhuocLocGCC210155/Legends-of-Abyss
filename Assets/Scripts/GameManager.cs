using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] FadeUI fadeUI;
    [SerializeField] float fadeTime;
    public Vector2 respawnPoint;
    public string saveScene;
    public Vector2 savePoint;
    public List<string> unlockedMap = new List<string>();
    public List<string> savedMap = new List<string>();
    public List<string> unlockShards = new List<string>();
    public List<BossDefeatedData> bossDefeated = new List<BossDefeatedData>();
    public string currentMap;
    public SaveGameSystem saveGameSystem;
    public string transitionFromScene;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        saveGameSystem = GetComponent<SaveGameSystem>();
        if (!savedMap.Contains(saveScene))
        {
            unlockedMap.Add(saveScene);
        }
        LoadSceneEnterGame();
        currentMap = SceneManager.GetActiveScene().name;
        if (!savedMap.Contains(currentMap))
        {
            unlockedMap.Add(currentMap);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            PlayerController.Instance.canControl = false;
            fadeUI.FadeUiIn(fadeTime);
            Time.timeScale = 0;
        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PlayerController.Instance.canControl = true;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn()
    {
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        PlayerController.Instance.playerAnimation.Respawn();
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        SceneManager.LoadScene(saveScene);
        PlayerController.Instance.transform.position = savePoint;
        PlayerController.Instance.Health = PlayerController.Instance.maxHp;
        PlayerController.Instance.RB.gravityScale = 18;
        PlayerController.Instance.isAlive = true;
        PlayerController.Instance.canControl = true;
        PlayerController.Instance.isLie = true;
    }

    void LoadSceneEnterGame()
    {
        PlayerController.Instance.transform.position = savePoint;
        PlayerController.Instance.LastOnGroundTime = -1;
        SceneManager.LoadScene(saveScene);
    }

    public void SaveProgress()
    {
        saveGameSystem.SaveProgress();
    }

    public void IncreaseShard(string shard)
    {
        if (!unlockShards.Contains(shard))
        {
            unlockShards.Add(shard);
        }
    }

    public void AddDefeatedBoss(string name, Vector2 pos){
        BossDefeatedData data = new BossDefeatedData(name,pos);
        if (!bossDefeated.Contains(data) && bossDefeated != null)
        {
            bossDefeated.Add(data);
        }
    }
}
