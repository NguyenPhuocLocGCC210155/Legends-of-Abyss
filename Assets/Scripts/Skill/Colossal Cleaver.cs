using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slash Skill", menuName = "Slash Skill/Colossal Cleaver")]
public class ColossalCleaver : Skills
{
    [SerializeField] float hitForce;
    [SerializeField] Vector3 spawnTranform;
    [SerializeField] GameObject slash;
    public override void Activate()
    {
        PlayerController.Instance.playerAnimationAndAudio.SlashSuper();
        PlayerController.Instance.LostControl(castingTime);
        PlayerController.Instance.FreezePlayer(castingTime);
        GameObject obj = Instantiate(slash, PlayerController.Instance.SideAttackTransform.position, Quaternion.identity);
        obj.GetComponent<SlashSkill>().damage = this.damage;
        obj.GetComponent<SlashSkill>().hitForce = this.hitForce;
        if (PlayerController.Instance.IsFacingRight)
        {
            obj.transform.position += spawnTranform;
            obj.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            obj.transform.position += new Vector3(spawnTranform.x * -1, spawnTranform.y, spawnTranform.z);
            obj.transform.eulerAngles = new Vector2(slash.transform.eulerAngles.x, 180);
        }
    }
}
