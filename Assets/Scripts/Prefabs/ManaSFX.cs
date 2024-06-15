using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSFX : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float beginDuration = 0.2f;
    [SerializeField] protected float endDuration = 0.5f;
    protected Rigidbody2D rb;
    protected Coroutine movementCoroutine;
    protected Vector2 randomDirection;
    protected float randomDuration;
    protected float startTime;
    protected virtual void Start()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        randomDuration = Random.Range(beginDuration, endDuration);
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        movementCoroutine = StartCoroutine(MoveRandomThenChase());
    }

    protected virtual IEnumerator MoveRandomThenChase()
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

    protected virtual void OnTriggerEnter2D(Collider2D other)
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
