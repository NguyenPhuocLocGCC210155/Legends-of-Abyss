using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float hitForce;
    private bool hasRandomDirection = false;
    private Vector2 randomDirection;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void SetTarget(Transform targetTransform, float missileSpeed)
    {
        target = targetTransform;
        speed = missileSpeed;
    }

    void Update()
    {
        if (target == null)
        {
            if (!hasRandomDirection)
            {
                // Tạo một hướng ngẫu nhiên trong 2D
                randomDirection = Random.insideUnitCircle.normalized;
                hasRandomDirection = true;
            }

            // Di chuyển theo hướng ngẫu nhiên
            transform.position += (Vector3)randomDirection * speed * Time.deltaTime;

            // Optional: Add rotation towards the direction
            float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            // Di chuyển về phía mục tiêu
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optional: Add rotation towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            if (other.GetComponent<Enemy>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                other.GetComponent<Enemy>().EnemyHit(damage, dirRecoil * Vector2.right, hitForce);
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
