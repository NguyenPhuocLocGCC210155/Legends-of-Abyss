using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Skill", menuName = "Buff Skill/Deflective Shield")]
public class DeflectiveShield : Skills
{
    [SerializeField] GameObject skillEffect; 
    public override void Activate()
    {
        PlayerController.Instance.FreezePlayer(castingTime);
        PlayerController.Instance.LostControl(castingTime);
        PlayerController.Instance.ImmuneDamage(castingTime);
        GameObject obj = Instantiate(skillEffect, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(PlayerController.Instance.transform, true);
    }

}
