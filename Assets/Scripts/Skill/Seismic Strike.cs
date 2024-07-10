using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Seismic Strike")]
public class SeismicStrike : Skills
{
    [SerializeField] float speed;
    [SerializeField] float hitForce;
    [SerializeField] GameObject skillEffect;
    [SerializeField] Vector2 sizeAttack;
    [SerializeField] float scale;
    [SerializeField] LayerMask layerMask;
    public override void Activate()
    {
        if (PlayerController.Instance.LastOnGroundTime < 0)
        {
            PlayerController.Instance.StartCoroutine(HandleActivate());
        }
    }

    IEnumerator HandleActivate()
    {
        PlayerController.Instance.canControl = false;
        PlayerController.Instance.animator.SetBool("IsSkillGround", true);

        while (PlayerController.Instance.LastOnGroundTime < 0)
        {
            PlayerController.Instance.RB.velocity = speed * Vector2.down;
            PlayerController.Instance.ImmuneDamage(true);
            yield return null;
        }

        PlayerController.Instance.StartCoroutine(WaitToEnd());
        Attack();
        GameObject obj = Instantiate(skillEffect, PlayerController.Instance.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        obj.transform.localScale = new Vector3(scale, scale, 0);
    }

    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(castingTime);
        PlayerController.Instance.animator.SetBool("IsSkillGround", false);
        PlayerController.Instance.canControl = true;
        yield return new WaitForSeconds(castingTime);
        PlayerController.Instance.ImmuneDamage(false);
    }

    void Attack()
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(PlayerController.Instance.transform.position, sizeAttack, 0, layerMask);
        if (objectToHit != null)
        {
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
    }
}
