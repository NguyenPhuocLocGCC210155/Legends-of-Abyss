using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject keys;
    public GameObject shards;
    public GameObject lantern;
    public GameObject cloak;
    public GameObject belt;
    public GameObject wing;
    public GameObject medal;
    public GameObject posion;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (PlayerController.Instance.isUnlockDash)
        {
            cloak.SetActive(true);
        }

        if (PlayerController.Instance.isUnlockDoubleJump)
        {
            wing.SetActive(true);
        }

        if (PlayerController.Instance.isUnlockWallJump)
        {
            belt.SetActive(true);
        }

        if (PlayerController.Instance.isUnlockLantern)
        {
            lantern.SetActive(true);
        }

        if (PlayerController.Instance.isUnlockMedal)
        {
            medal.SetActive(true);
        }

        if (PlayerController.Instance.isUnlockPosion)
        {
            posion.SetActive(true);
        }

        if (PlayerController.Instance.heartShards > 0)
        {
            shards.SetActive(true);
            shards.GetComponentInChildren<TextMeshProUGUI>().text = PlayerController.Instance.heartShards.ToString();
        }

        if (PlayerController.Instance.keys > 0)
        {
            shards.SetActive(true);
            shards.GetComponentInChildren<TextMeshProUGUI>().text = PlayerController.Instance.heartShards.ToString();
        }
    }
}
