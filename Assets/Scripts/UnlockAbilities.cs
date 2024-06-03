using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnlockAbilities : MonoBehaviour
{
    public AbilitiesUnlock abilitiesUnlock;
    public int ConutEffect = 30;
    bool used;
    private ObjectPooling pool;
    Collider2D cd;
    public enum AbilitiesUnlock
    {
        Dashing,
        WallJump,
        DoubleJump,
        ShawdowDash

    }

    private void Start() {
        pool = GetComponent<ObjectPooling>();
        cd = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { }
        {
            used = true;
            switch (abilitiesUnlock)
            {
                case AbilitiesUnlock.Dashing:
                    PlayerController.Instance.isUnlockDash = true;
                    break;

                case AbilitiesUnlock.WallJump:
                    PlayerController.Instance.isUnlockWallJump = true;
                    break;

                case AbilitiesUnlock.DoubleJump:
                    PlayerController.Instance.isUnlockDoubleJump = true;
                    break;
            }
            for (int i = 0; i < ConutEffect; i++)
            {
                GameObject obj = pool.GetPooledObject();
            }
            cd.enabled = false;
        }
    }
}
