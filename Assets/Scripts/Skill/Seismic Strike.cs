using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;


[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Seismic Strike")]
public class SeismicStrike : Skills
{
    public float a;
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
            PlayerController.Instance.canControl = false;
            PlayerController.Instance.playerAnimationAndAudio.Quake(true);
            PlayerController.Instance.playerAnimationAndAudio.Run(false);
            PlayerController.Instance.StartCoroutine(HandleActivate());
        }
    }

    IEnumerator HandleActivate()
    {
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(a);
        PlayerController.Instance.RB.constraints = RigidbodyConstraints2D.None;

        while (PlayerController.Instance.LastOnGroundTime < 0)
        {
            PlayerController.Instance.RB.velocity = speed * Vector2.down;
            PlayerController.Instance.ImmuneDamage(true);
            Debug.Log(PlayerController.Instance.LastOnGroundTime);
            yield return null;
        }

        PlayerController.Instance.playerAnimationAndAudio.Quake(false);
        PlayerController.Instance.StartCoroutine(WaitToEnd());
        Attack();
        // PlayerController.Instance.playerAnimation.Fall(false);
		// PlayerController.Instance.playerAnimation.WallSlide(false);
        GameObject obj = Instantiate(skillEffect, PlayerController.Instance.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        obj.transform.localScale = new Vector3(scale, scale, 0);
    }

    IEnumerator WaitToEnd()
    {
        PlayerController.Instance.FreezePlayer(0);
        yield return new WaitForSeconds(castingTime);
        PlayerController.Instance.canControl = true;
        PlayerController.Instance.LastOnGroundTime = 1;
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
