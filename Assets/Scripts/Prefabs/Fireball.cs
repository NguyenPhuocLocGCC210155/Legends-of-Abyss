using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ShootSkill
{

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
