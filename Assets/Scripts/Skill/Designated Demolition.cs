using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Detect Skill", menuName = "Detect Skill/Designated Demolition")]
public class DesignatedDemolition : Skills
{
    [SerializeField] float sizeAttack;
    [SerializeField] GameObject skillEffect;
    [SerializeField] GameObject exploseEffect;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float hitForce;
    [SerializeField] float distanceSpamLighting;
    public override void Activate()
    {
        PlayerController.Instance.animator.SetTrigger("Attacking");
        PlayerController.Instance.animator.SetBool("isWalking", false);
        PlayerController.Instance.LostControl(castingTime);
        PlayerController.Instance.FreezePlayer(castingTime);
        Collider2D[] objectToHit = Physics2D.OverlapCircleAll(PlayerController.Instance.transform.position, sizeAttack, layerMask, 0);
        PlayerController.Instance.StartCoroutine(HandleActive(objectToHit));
        Debug.Log(objectToHit.Length);
    }

    IEnumerator HandleActive(Collider2D[] objectToHit)
    {
        yield return new WaitForSeconds(castingTime);
        for (int i = 0; i < objectToHit.Length; i++)
        {
            if (objectToHit[i].GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, ((transform.position - objectToHit[i].transform.position).normalized * -1), _recoilStregth);
                objectToHit[i].GetComponent<Enemy>().EnemyHit(damage, dirRecoil * Vector2.right, hitForce);
                GameObject obj = Instantiate(skillEffect, objectToHit[i].transform.position + new Vector3(0, distanceSpamLighting, 0), Quaternion.identity);
            }
        }
    }
}