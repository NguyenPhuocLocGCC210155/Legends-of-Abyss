using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string transitionTo;
    [SerializeField] Transform startPoint;
    [SerializeField] Vector2 exitDirection;
    [SerializeField] float exitTime;
    
    private void Start() {
        if (transitionTo == GameManager.Instance.transitionFromScene && PlayerController.Instance.isAlive)
        {
            PlayerController.Instance.transform.position = startPoint.position;
            StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection,exitTime));
        }
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));   
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && PlayerController.Instance.isAlive)
        {
            GameManager.Instance.transitionFromScene = SceneManager.GetActiveScene().name;
            PlayerController.Instance.isCutScene = true;
            SceneManager.LoadScene(transitionTo);
            GameManager.Instance.currentMap = transitionTo;
            if (!GameManager.Instance.savedMap.Contains(transitionTo))
            {
                GameManager.Instance.unlockedMap.Add(transitionTo);
            }
            StartCoroutine(UIManager.Instance.sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, transitionTo));   
        }
    }
}
