using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnlockAbilities : MonoBehaviour
{
    public AbilitiesUnlock abilitiesUnlock;
    public GameObject uiCanvas;
    public int ConutEffect = 30;
    private ObjectPooling pool;
    Collider2D cd;
    GameObject ui;
    public enum AbilitiesUnlock
    {
        Dashing,
        WallJump,
        DoubleJump,
        ShawdowDash,
        Lantern,
        Posion
    }

    private void Start()
    {
        pool = GetComponent<ObjectPooling>();
        cd = GetComponent<Collider2D>();

        switch (abilitiesUnlock)
        {
            case AbilitiesUnlock.Dashing:
                if (PlayerController.Instance.isUnlockDash)
                {
                    Destroy(gameObject);
                }
                break;

            case AbilitiesUnlock.WallJump:
                if (PlayerController.Instance.isUnlockWallJump)
                {
                    Destroy(gameObject);
                }
                break;
            case AbilitiesUnlock.DoubleJump:
                if (PlayerController.Instance.isUnlockDoubleJump)
                {
                    Destroy(gameObject);
                }
                break;
            case AbilitiesUnlock.ShawdowDash:
                if (PlayerController.Instance.isUnlockMedal)
                {
                    Destroy(gameObject);
                }
                break;
            case AbilitiesUnlock.Lantern:
                if (PlayerController.Instance.isUnlockLantern)
                {
                    Destroy(gameObject);
                }
                break;
            case AbilitiesUnlock.Posion:
                if (PlayerController.Instance.isUnlockPosion)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Update()
    {
        if (ui != null)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Destroy(ui);
                PlayerController.Instance.canControl = true;
                PlayerController.Instance.playerAnimationAndAudio.Kneel(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
                case AbilitiesUnlock.ShawdowDash:
                    PlayerController.Instance.isUnlockMedal = true;
                    break;
                case AbilitiesUnlock.Lantern:
                    PlayerController.Instance.isUnlockLantern = true;
                    break;
                case AbilitiesUnlock.Posion:
                    PlayerController.Instance.isUnlockPosion = true;
                    break;
            }
            StartCoroutine(ShowAbility());
            for (int i = 0; i < ConutEffect; i++)
            {
                GameObject obj = pool.GetPooledObject();
            }
            cd.enabled = false;
        }
    }

    IEnumerator ShowAbility()
    {
        PlayerController.Instance.playerAnimationAndAudio.Scream();
        PlayerController.Instance.isLie = true;
        PlayerController.Instance.Awaken(2.5f);
        yield return new WaitForSeconds(3f);
        PlayerController.Instance.FreezePlayer(0);
        ui = Instantiate(uiCanvas);
        PlayerController.Instance.canControl = false;
        GameManager.Instance.SaveProgress();
    }
}
