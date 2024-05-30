using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public string saveScene;
    public Vector2 respawnPoint;
    public Vector2 savePoint;

    public string transitionFromScene;
    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else{
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void RespawnPlayer(){
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn(){
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.Health = PlayerController.Instance.maxHp;
        SceneManager.LoadScene(saveScene);
        PlayerController.Instance.transform.position = savePoint;
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.isAlive = true;
        PlayerController.Instance.animator.SetTrigger("Alive");
    }
}
