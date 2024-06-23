using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TopDownLazerBeam : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float hitForce;
    public bool isUseDissolve = true;
    public bool isUseVertical = false;
    public Vector2 attackSize;
    public Transform attackTransform;
    public LayerMask attackableLayerMask;
    public LayerMask GroundLayer;
    Animator animator;
    Light2D light2D;
    float time;

    private void Start()
    {
        animator = GetComponent<Animator>();
        light2D = GetComponent<Light2D>();
        light2D.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (IsInLayerMask(other.gameObject, GroundLayer))
        {
            light2D.enabled = true;
            animator.SetTrigger("Begin");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(attackTransform.position, attackSize);
    }

    void DissloveAffterAnimation(){
        GetComponent<Disslove>().Vanish(isUseDissolve, isUseVertical);
        Destroy(gameObject, 1f);
    }

    void Attack()
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackSize, 0, attackableLayerMask);
        for (int i = 0; i < objectToHit.Length; i++)
        {
            if (objectToHit[i].GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, ((transform.position - objectToHit[i].transform.position).normalized * -1), _recoilStregth);
                objectToHit[i].GetComponent<Enemy>().EnemyHit(damage, dirRecoil * Vector2.right, hitForce);
            }
        }
    }



    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask == (mask | (1 << obj.layer)));
    }
}
