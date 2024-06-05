using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHP : MonoBehaviour
{
    [SerializeField] GameObject canvasUI;
    [SerializeField] HeartShard heartShard;
    [SerializeField] string instanceName;
    public int ConutEffect = 30;
    private ObjectPooling pool;
    bool used;

    private void Start()
    {
        if (GameManager.Instance.unlockShards.Contains(instanceName))
        {
            Destroy(gameObject);
        }
        pool = GetComponent<ObjectPooling>();
        if (PlayerController.Instance.maxHp >= PlayerController.Instance.maxTotalHealth)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.unlockShards.Contains(instanceName))
            {
                GameManager.Instance.unlockShards.Add(instanceName);
            }
            StartCoroutine(ShowAbility());
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            for (int i = 0; i < ConutEffect; i++)
            {
                GameObject obj = pool.GetPooledObject();
            }
        }
    }

    IEnumerator ShowAbility()
    {
        StartCoroutine(PlayerController.Instance.WaitForAwaken(2.5f));

        yield return new WaitForSeconds(3f);
        canvasUI.SetActive(true);
        heartShard.initialFillAmount = PlayerController.Instance.heartShards * 0.25f;
        PlayerController.Instance.heartShards++;
        heartShard.targetFillAmount = PlayerController.Instance.heartShards * 0.25f;
        StartCoroutine(heartShard.LerpFill());
        PlayerController.Instance.canControl = false;

        yield return new WaitForSeconds(2.5f);

        canvasUI.SetActive(false);
        PlayerController.Instance.canControl = true;
        GameManager.Instance.SaveProgress();
        Destroy(gameObject);
    }
}
