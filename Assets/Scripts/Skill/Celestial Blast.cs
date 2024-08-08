using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Celestial Blast")]
public class CelestialBlast : Skills
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] float distance;
    public override void Activate()
    {
        PlayerController.Instance.StartCoroutine(BeginSkill());
    }

    IEnumerator BeginSkill()
    {
        GameObject obj = Instantiate(skillEffect, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.GetComponent<TopDownLazerBeam>().damage = this.damage;
        obj.SetActive(false);
        if (PlayerController.Instance.LastOnGroundTime > 0f)
        {
            obj.transform.position += new Vector3(0, distance, 0);
        }
        PlayerController.Instance.playerAnimationAndAudio.ShadowScream(true);
        yield return new WaitForSeconds(castingTime);
        PlayerController.Instance.playerAnimationAndAudio.ShadowScream(false);
        obj.SetActive(true);
    }
}
