using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Skill", menuName = "Shoot Skill/Inferno Orb")]
public class InfernoOrb : Skills
{
    [SerializeField] GameObject maigc;

    public override void Activate()
    {
        PlayerController.Instance.StartCoroutine(HandleControlAndInvincibility());
    }

    private IEnumerator HandleControlAndInvincibility()
    {
        PlayerController.Instance.LostControl(0.7f);
        PlayerController.Instance.FreezePlayer(0.5f);

        // Đợi trước khi kích hoạt phép thuật
        yield return new WaitForSeconds(0.1f);
         PlayerController.Instance.playerAnimation.CastShootSkill();
        PlayerController.Instance.StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.30f);
        GameObject magic = Instantiate(maigc, PlayerController.Instance.SideAttackTransform.position, Quaternion.identity);
        magic.GetComponent<ShootSkill>().damage = this.damage;
        if (PlayerController.Instance.IsFacingRight)
        {
            magic.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            magic.transform.eulerAngles = new Vector2(maigc.transform.eulerAngles.x, 180);
        }
    }
}
