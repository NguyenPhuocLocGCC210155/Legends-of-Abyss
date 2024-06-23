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
            PlayerController.Instance.FreezePlayer(2f);
            PlayerController.Instance.LostControl(2f);
            GetComponent<Collider2D>().enabled = false;
            foreach (LockBossRoom l in lockDoor)
            {
                l.Active();
            }
        }
    }

}
