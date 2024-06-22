using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Skill", menuName = "Shoot Skill/Quintuple Shot")]
public class QuintupleShot : Skills
{
    [SerializeField] float hitForce;
    [SerializeField] GameObject skillObject;
    public override void Activate()
    {
        PlayerController.Instance.StartCoroutine(HandleControlAndInvincibility());
    }

    private IEnumerator HandleControlAndInvincibility()
    {
        PlayerController.Instance.LostControl(castingTime);
        PlayerController.Instance.FreezePlayer(castingTime);

        // Đợi trước khi kích hoạt phép thuật
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.animator.SetBool("isCasting", true);
        PlayerController.Instance.StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.20f);
        PlayerController.Instance.StartCoroutine(StartShoot());
        PlayerController.Instance.isRecoilingX = true;
        PlayerController.Instance.animator.SetBool("isCasting", false);
    }

    IEnumerator StartShoot()
    {
        ActivateSkill();
        yield return new WaitForSeconds(0.1f);
        ActivateSkill();
        yield return new WaitForSeconds(0.1f);
        ActivateSkill();
        yield return new WaitForSeconds(0.1f);
        ActivateSkill();
        yield return new WaitForSeconds(0.1f);
        ActivateSkill();
    }

    void ActivateSkill()
    {
        GameObject obj = Instantiate(skillObject, PlayerController.Instance.SideAttackTransform.position + new Vector3(0, RanDomRange(), 0), Quaternion.identity);
        obj.GetComponent<ShootTwoPhasaSkill>().damage = this.damage;
        obj.transform.localScale *= 0.5f;
        if (PlayerController.Instance.IsFacingRight)
        {
            obj.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            obj.transform.eulerAngles = new Vector2(skillObject.transform.eulerAngles.x, 180);
        }
    }

    float RanDomRange()
    {
        return Random.Range(-0.2f, 1);
    }
}
