using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonSlash : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float initSpeed;
    [SerializeField] float fullSpeed;
    [SerializeField] float lifeTime;
    [SerializeField] LayerMask layerDetected;
    [SerializeField] Transform checkFront;
    [SerializeField] Vector2 checkSize;
    [SerializeField] GameObject exploseEffect;
    [SerializeField] LayerMask layerMask;
    float animationDuration;
    float elapseTime;
    float currentSpeed;
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = initSpeed;
        animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        elapseTime = 0;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        elapseTime += Time.deltaTime;
        currentSpeed = Mathf.Lerp(initSpeed, fullSpeed, elapseTime / animationDuration);

        // Cập nhật velocity cho Rigidbody2D
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y) * transform.right;

        // Khi animation đầu tiên kết thúc, chuyển sang animation thứ hai
        if (elapseTime >= animationDuration)
        {
            animator.SetTrigger("MaxSpeed");
        }

        if (Physics2D.OverlapBox(checkFront.position, checkSize, 0, layerDetected))
        {
            GameObject Obj = Instantiate(exploseEffect, checkFront.position, Quaternion.identity);
            Destroy(Obj, 2f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, layerMask) && !PlayerController.Instance.isInvincible && PlayerController.Instance.Health > 0)
        {
            Attack();
            if (PlayerController.Instance.isAlive)
            {
                PlayerController.Instance.HitStopTime(0, 1);
                GameObject Obj = Instantiate(exploseEffect, checkFront.position, Quaternion.identity);
                Destroy(Obj, 2f);
                Destroy(gameObject);
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask == (mask | (1 << obj.layer)));
    }

    private void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(checkFront.position, checkSize);
    }
}
