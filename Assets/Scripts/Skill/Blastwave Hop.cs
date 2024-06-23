using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Blastwave Hop")]
public class BlastwaveHop : Skills
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] float sizeAttack;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float hitForce;
    public override void Activate()
    {
        PlayerController.Instance.FreezePlayer(castingTime);
        PlayerController.Instance.LostControl(castingTime);
        Collider2D[] objectToHit = Physics2D.OverlapCircleAll(PlayerController.Instance.transform.position, sizeAttack, layerMask, 0);
        PlayerController.Instance.StartCoroutine(HandleActivate(objectToHit));
    }

    IEnumerator HandleActivate(Collider2D[] objectToHit)
    {
        yield return new WaitForSeconds(castingTime / 2);
        GameObject obj = Instantiate(skillEffect, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(PlayerController.Instance.transform, true);
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
