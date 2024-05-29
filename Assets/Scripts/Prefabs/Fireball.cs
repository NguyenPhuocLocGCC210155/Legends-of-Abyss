using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float damage = 3;
    [SerializeField] float hitForce;
    [SerializeField] float speed;
    [SerializeField] float lifeTime = 1f;
    Rigidbody2D rb;

    public float Damage { get => damage; set => damage = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        // transform.position += speed * transform.right;
        rb.velocity = speed * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
            // other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (other.transform.position - transform.position).normalized, hitForce);
            other.gameObject.GetComponent<Enemy>().EnemyHit(damage, Vector2.right * dirRecoil, hitForce);
        }
    }
}
