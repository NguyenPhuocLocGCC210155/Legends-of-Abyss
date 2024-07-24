using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAwakeBoss : MonoBehaviour
{
    [SerializeField] Enemy boss;
    [SerializeField] AudioClip themeBossFight;
    [SerializeField] List<LockBossRoom> lockDoor = new List<LockBossRoom>();
    private bool isComplete;

    private void Update()
    {
        if (boss.isDestroyed && !isComplete)
        {
            foreach (LockBossRoom l in lockDoor)
            {
                l.InActive();
            }
            isComplete = true;
            GameManager.Instance.audioSource.clip = GameManager.Instance.baseAudioClip;
            GameManager.Instance.audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !boss.isDestroyed)
        {
            boss.BeginAwaken();
            StartCoroutine(LockPlayerWhenRoar());
            GetComponent<Collider2D>().enabled = false;
            foreach (LockBossRoom l in lockDoor)
            {
                l.Active();
            }
        }
    }

    IEnumerator LockPlayerWhenRoar()
    {
        GameManager.Instance.audioSource.Stop();
        PlayerController.Instance.FreezeXPlayer(2f);
        PlayerController.Instance.LostControl(2f);
        PlayerController.Instance.playerAnimation.Fear(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.audioSource.clip = themeBossFight;
        GameManager.Instance.audioSource.Play();
        PlayerController.Instance.playerAnimation.Fear(false);
    }
}
