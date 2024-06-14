using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class TriggerAwakeBoss : MonoBehaviour
{
    [SerializeField] Enemy boss;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            boss.BeginAwaken();
            StartCoroutine(PlayerWaitForBossAwake());
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator PlayerWaitForBossAwake(){
        PlayerController.Instance.isCutScene = true;
        PlayerController.Instance.RB.velocity = Vector2.zero;
        PlayerController.Instance.animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.isCutScene = false;
    }
}
