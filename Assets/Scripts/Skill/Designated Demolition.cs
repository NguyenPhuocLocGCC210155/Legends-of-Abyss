using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Designated Demolition")]
public class DesignatedDemolition : Skills
{
    [SerializeField] float sizeAttack;
    [SerializeField] GameObject skillEffect;
    [SerializeField] GameObject exploseEffect;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float hitForce;
    [SerializeField] float distanceSpamLighting;
    [SerializeField] Vector2 exploseScale;
    public override void Activate()
    {
        PlayerController.Instance.playerAnimationAndAudio.ShadowScream(true);
        PlayerController.Instance.LostControl(castingTime / 2);
        PlayerController.Instance.FreezePlayer(castingTime / 2);
        Collider2D[] objectToHit = Physics2D.OverlapCircleAll(PlayerController.Instance.transform.position, sizeAttack, layerMask, 0);
        PlayerController.Instance.StartCoroutine(HandleActive(objectToHit));
    }


    IEnumerator HandleActive(Collider2D[] objectToHit)
    {
        yield return new WaitForSeconds(castingTime / 2);
        PlayerController.Instance.playerAnimationAndAudio.ShadowScream(false);
        yield return new WaitForSeconds(castingTime / 2);
        for (int i = 0; i < objectToHit.Length; i++)
        {
            if (objectToHit[i].GetComponent<Enemy>() != null)
            {
                int dirRecoil = PlayerController.Instance.IsFacingRight ? 1 : -1;
                // objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, ((transform.position - objectToHit[i].transform.position).normalized * -1), _recoilStregth);
                objectToHit[i].GetComponent<Enemy>().EnemyHit(damage, dirRecoil * Vector2.right, hitForce);
                GameObject lighting = Instantiate(skillEffect, objectToHit[i].transform.position + new Vector3(0, distanceSpamLighting, 0), Quaternion.identity);
                GameObject explose = Instantiate(exploseEffect, objectToHit[i].transform.position, Quaternion.identity);
                explose.transform.localScale = exploseScale;
            }
        }
    }
}
