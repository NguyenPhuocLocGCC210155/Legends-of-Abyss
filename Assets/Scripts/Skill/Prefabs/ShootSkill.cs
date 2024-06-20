using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkill : MonoBehaviour
{
    public float damage = 3;
    public float hitForce;
    public float speed;
    public float lifeTime = 1f;
    public GameObject exploseEffect;
    public Transform detectPoint;
    [SerializeField] bool useDissolve = true;
    [SerializeField] bool useVertical = false;
    [ColorUsage(true, true)]
    [SerializeField] Color color = Color.white;
    [SerializeField] Vector2 scaleEffect = new Vector2(1, 1);
    [SerializeField] LayerMask layerMask;
    Rigidbody2D rb;
    Collider2D[] cd;
    Material material;
    Disslove disslove;
    SpriteRenderer spriteRenderer;
    bool isDetect;
    private bool hasCollider;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        disslove = GetComponent<Disslove>();
        material = spriteRenderer.material;
        material.SetColor("_OutlikeColor", color);
        Destroy(gameObject, lifeTime);
    }
    protected virtual void FixedUpdate()
    {
        if (!isDetect)
        {
            rb.velocity = speed * transform.right;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (hasCollider)
        {
            return;
        }
        if (IsInLayerMask(other.gameObject, layerMask))
        {
            hasCollider = true;
            isDetect = true;
            DisableAllCollider();
            disslove.Vanish(useDissolve, useVertical);
            GameObject Obj = Instantiate(exploseEffect, detectPoint.position, Quaternion.identity);
            Obj.transform.localScale = scaleEffect;
            Destroy(Obj, 2f);
            Destroy(gameObject, disslove.dissloveTime + 0.25f);
            if (other.gameObject.GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (other.transform.position - transform.position).normalized, hitForce);
                other.gameObject.GetComponent<Enemy>().EnemyHit(damage, Vector2.right * dirRecoil, hitForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollider)
        {
            return;
        }
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            hasCollider = true;
            DisableAllCollider();
            isDetect = true;
            disslove.Vanish(useDissolve, useVertical);
            GameObject Obj = Instantiate(exploseEffect, detectPoint.position, Quaternion.identity);
            Obj.transform.localScale = scaleEffect;
            Destroy(Obj, 2f);
            Destroy(gameObject, disslove.dissloveTime + 0.25f);
            int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
            // other.gameObject.GetComponent<Enemy>().EnemyHit(damage, (other.transform.position - transform.position).normalized, hitForce);
            other.gameObject.GetComponent<Enemy>().EnemyHit(damage, Vector2.right * dirRecoil, hitForce);
        }
    }



    private void DisableAllCollider()
    {
        foreach (Collider2D c in cd)
        {
            c.enabled = false;
        }
    }

    protected virtual bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask == (mask | (1 << obj.layer)));
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detectPoint.position, new Vector2(0.1f, 0.1f));
    }
}
