using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenSFX : ManaSFX
{
    [SerializeField] float stopTime = 1f;
    protected override void Start()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        randomDuration = Random.Range(0.2f, 0.5f);
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        movementCoroutine = StartCoroutine(MoveRandomThenChase());
    }

    protected override IEnumerator MoveRandomThenChase()
    {
        while (Time.time < startTime + randomDuration)
        {
            rb.velocity = randomDirection * speed;
            yield return null;
        }

        while(Time.time < startTime + stopTime){
            rb.velocity = Vector2.zero;
            yield return null;
        }

        rb.velocity = Vector2.zero;

        while (true)
        {
            Vector2 directionPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.velocity = directionPlayer * speed * 2;
            yield return null;
        }
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
