using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        PlayerController.Instance.isCutScene = true;
        PlayerController.Instance.canControl = false;
        PlayerController.Instance.isInvincible = true;
        PlayerController.Instance.RB.velocity = Vector2.zero;
        PlayerController.Instance.TakeDamage(1, true);
        PlayerController.Instance.ImmuneDamage(1.5f);
        PlayerController.Instance.isLie = true;
        PlayerController.Instance.ResetInput();
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        yield return new WaitForSeconds(1.5f);
        PlayerController.Instance.playerAnimationAndAudio.Kneel(true);
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        PlayerController.Instance.transform.position = GameManager.Instance.respawnPoint;
        yield return new WaitForSeconds(0.2f);
        PlayerController.Instance.isCutScene = false;
        PlayerController.Instance.isInvincible = false;
        PlayerController.Instance.canControl = true;
    }
}
