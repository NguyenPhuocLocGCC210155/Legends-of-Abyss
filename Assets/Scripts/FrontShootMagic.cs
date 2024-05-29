using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Front Shoot Magic", menuName ="Shoot magic/Front")]
public class FrontMagicShoot : Skills
{
    [SerializeField] GameObject maigc;
    float time = 3f;

    public override void Active()
    {
        PlayerController.Instance.animator.SetBool("isCasting", true);
        PlayerController.Instance.StartCoroutine(Shoot());
    }

    IEnumerator Shoot(){
        yield return new WaitForSeconds(0.20f);
        yield return new WaitForFixedUpdate();
        GameObject _magic = Instantiate(maigc, PlayerController.Instance.SideAttackTransform.position, Quaternion.identity);
        if (PlayerController.Instance.IsFacingRight)
        {
            _magic.transform.eulerAngles = Vector3.zero;
        }else{
            _magic.transform.eulerAngles = new Vector2(_magic.transform.eulerAngles.x, 180);
        }
        PlayerController.Instance.isRecoilingX = true;
        PlayerController.Instance.animator.SetBool("isCasting", false);
    }
}
