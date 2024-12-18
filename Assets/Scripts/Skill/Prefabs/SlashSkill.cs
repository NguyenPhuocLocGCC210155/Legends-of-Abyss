using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float hitForce;
    [SerializeField] GameObject exploseEffect;
    [SerializeField] Vector2 attackSize;
    [SerializeField] Vector2 scaleEffect = new Vector2(1, 1);
    [SerializeField] LayerMask layerMask;
    Material material;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        hitBox(transform, attackSize, hitForce);
    }

    void hitBox(Transform attackTransform, Vector2 attackArea, float _recoilStregth)
    {
        bool isDetect = true;
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, layerMask);
        if (isDetect)
        {
            for (int i = 0; i < objectToHit.Length; i++)
            {
                if (objectToHit[i].GetComponent<Enemy>() != null)
                {
                    int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                    // objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, ((transform.position - objectToHit[i].transform.position).normalized * -1), _recoilStregth);
                    objectToHit[i].GetComponent<Enemy>().EnemyHit(damage, dirRecoil * Vector2.right, _recoilStregth);
                    GameObject obj = Instantiate(exploseEffect, objectToHit[i].transform.position, Quaternion.identity);
                    obj.transform.localScale = scaleEffect;
                    Destroy(obj, 0.5f);
                }
            }
            isDetect = false;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackSize);
    }
}
