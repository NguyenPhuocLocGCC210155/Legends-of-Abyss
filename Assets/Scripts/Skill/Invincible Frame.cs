using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Skill", menuName = "Buff Skill/Invincible Frame")]
public class InvincibleFrame : Skills
{
    [SerializeField] GameObject effectSkill;
    public override void Activate()
    {
        GameObject obj = Instantiate(effectSkill, PlayerController.Instance.transform.position + new Vector3(0,-0.3f,0), Quaternion.identity);
        obj.transform.SetParent(PlayerController.Instance.transform, true);
        Destroy(obj, damage + 0.5f);
        PlayerController.Instance.ImmuneDamage(damage);
    }
}
