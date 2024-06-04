using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHP : MonoBehaviour
{
    [SerializeField] GameObject canvasUI;
    [SerializeField] HeartShard heartShard;
    [SerializeField] ShardData data;
    public int ConutEffect = 30;
    private ObjectPooling pool;
    bool used;

    private void Start()
    {
        if (data.isUsed)
        {
            Destroy(gameObject);
        }
        else
        {
            pool = GetComponent<ObjectPooling>();
            if (PlayerController.Instance.maxHp >= PlayerController.Instance.maxTotalHealth)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            data.isUsed = true;
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

        heartShard.initialFillAmount = PlayerController.Instance.heartShards * 0.25f;
        canvasUI.SetActive(true);
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
