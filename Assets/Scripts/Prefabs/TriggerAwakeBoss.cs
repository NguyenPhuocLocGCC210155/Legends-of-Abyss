using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAwakeBoss : MonoBehaviour
{
    [SerializeField] Enemy boss;
    [SerializeField] List<LockBossRoom> lockDoor = new List<LockBossRoom>();
    private bool isComplete;

    private void Update() {
        if (boss.isDestroyed && !isComplete)
        {
            foreach (LockBossRoom l in lockDoor)
            {
                l.InActive();
            }
            isComplete = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !boss.isDestroyed)
        {
            boss.BeginAwaken();
            StartCoroutine(PlayerWaitForBossAwake());
            GetComponent<Collider2D>().enabled = false;
            foreach (LockBossRoom l in lockDoor)
            {
                l.Active();
            }
        }
    }

    IEnumerator PlayerWaitForBossAwake(){
        PlayerController.Instance.isCutScene = true;
        PlayerController.Instance.RB.velocity = Vector2.zero;
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.FreezePositionX;
        PlayerController.Instance.animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(2f);
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.None;
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerController.Instance.isCutScene = false;
    }
}
