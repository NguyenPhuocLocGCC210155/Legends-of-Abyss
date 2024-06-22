using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D.IK;

public class ShootTwoPhasaSkill : MonoBehaviour
{
    public float lifeTime;
    public float initSpeed;
    public float maxSpeed;
    public float changePhaseTime;
    [HideInInspector] public float damage;
    [HideInInspector] public float hitForce;
    [SerializeField] Vector2 scaleEffect = Vector2.one;
    [SerializeField] GameObject exploseEffect;
    [SerializeField] Transform detectPoint;
    [SerializeField] LayerMask layerMask;
    float currentTime;
    bool hasCollider;
    bool isMaxSpeed;
    Rigidbody2D rb;
    Collider2D[] cd;
    Animator animator;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (hasCollider)
        {
            return;
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= changePhaseTime)
            {
                animator.SetTrigger("MaxSpeed");
                rb.velocity = maxSpeed * transform.right;
                if (!isMaxSpeed)
                {
                    isMaxSpeed = true;
                    damage *= 2;
                }
            }
            else
            {
                rb.velocity = initSpeed * transform.right;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasCollider)
        {
            return;
        }
        if (IsInLayerMask(other.gameObject, layerMask))
        {
            hasCollider = true;
            spriteRenderer.enabled = false;
            GameObject Obj = Instantiate(exploseEffect, detectPoint.position, Quaternion.identity);
            Obj.transform.localScale = scaleEffect;
            if (GetComponentInChildren<Light2D>())
            {
                GetComponentInChildren<Light2D>().enabled = false;
            }
            if (other.gameObject.GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (other.transform.position - transform.position).normalized, hitForce);
                other.gameObject.GetComponent<Enemy>().EnemyHit(damage, Vector2.right * dirRecoil, hitForce);
            }
            Destroy(gameObject, 1f);
            DisableAllCollider();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollider)
        {
            return;
        }
        if (IsInLayerMask(other.gameObject, layerMask))
        {
            hasCollider = true;
            spriteRenderer.enabled = false;
            GameObject Obj = Instantiate(exploseEffect, detectPoint.position, Quaternion.identity);
            Obj.transform.localScale = scaleEffect;
            if (GetComponentInChildren<Light2D>())
            {
                GetComponentInChildren<Light2D>().enabled = false;
            }
            if (other.gameObject.GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (other.transform.position - transform.position).normalized, hitForce);
                other.gameObject.GetComponent<Enemy>().EnemyHit(damage, Vector2.right * dirRecoil, hitForce);
            }
            Destroy(gameObject, 1f);
            DisableAllCollider();
        }
    }

    private void DisableAllCollider()
    {
        foreach (Collider2D c in cd)
        {
            c.enabled = false;
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask == (mask | (1 << obj.layer)));
    }
}
