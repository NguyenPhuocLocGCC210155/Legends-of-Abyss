using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSFX : MonoBehaviour
{
    [SerializeField] float speed = 5;
    Rigidbody2D rb;
    private Coroutine movementCoroutine;
    Vector2 randomDirection;
    float randomDuration;
    float startTime;
    void Start()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        randomDuration = Random.Range(0.2f, 0.5f);
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        movementCoroutine = StartCoroutine(MoveRandomThenChase());
    }

    IEnumerator MoveRandomThenChase()
    {
        while (Time.time < startTime + randomDuration)
        {
            rb.velocity = randomDirection * speed;
            yield return null;
        }

        rb.velocity = Vector2.zero;

        while (true)
        {
            Vector2 directionPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.velocity = directionPlayer * speed;
            yield return null;
        }
    }

    private void OnEnable() {
        Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > startTime + randomDuration)
        {
            gameObject.SetActive(false);
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }
            rb.velocity = Vector2.zero;
        }
    }
}
