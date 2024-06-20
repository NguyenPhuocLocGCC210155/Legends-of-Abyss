using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Skill", menuName = "Shoot Skill/Crescendo Slash")]
public class CrescendoSlash : Skills
{
    [SerializeField] GameObject slash1;
    [SerializeField] GameObject slash2;
    [SerializeField] GameObject slash3;
    public override void Activate()
    {
        PlayerController.Instance.StartCoroutine(HandleCrescendoSlash());
    }

    IEnumerator HandleCrescendoSlash()
    {
        PlayerController.Instance.animator.SetTrigger("Attacking");
        PlayerController.Instance.LostControl(castingTime);
        PlayerController.Instance.FreezePlayer(castingTime);
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.StartCoroutine(StartSlash());
    }

    IEnumerator StartSlash()
    {
        yield return new WaitForSeconds(0.04f);
        Slash(slash1, damage);
        yield return new WaitForSeconds(0.2f);
        Slash(slash2, damage + 1);
        yield return new WaitForSeconds(0.2f);
        Slash(slash3, damage * 2);
    }

    void Slash(GameObject slash, float dmg)
    {
        GameObject obj = Instantiate(slash, PlayerController.Instance.SideAttackTransform.position, Quaternion.identity);
        if (PlayerController.Instance.IsFacingRight)
        {
            obj.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            obj.transform.eulerAngles = new Vector2(slash.transform.eulerAngles.x, 180);
        }
    }
}
