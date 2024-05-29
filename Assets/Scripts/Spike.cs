using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn(){
        PlayerController.Instance.isCutScene = true;
        PlayerController.Instance.isInvincible = true;
        PlayerController.Instance.RB.velocity = Vector2.zero;
        Time.timeScale = 0;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        PlayerController.Instance.TakeDamage(1);

        yield return new WaitForSeconds(1);

        PlayerController.Instance.transform.position = GameManager.Instance.respawnPoint;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSeconds(UIManager.Instance.sceneFader.fadeTime);
        PlayerController.Instance.isCutScene = false;
        PlayerController.Instance.isInvincible = false;
        Time.timeScale = 1;

    }
}
