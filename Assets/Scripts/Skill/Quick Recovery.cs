using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Skill", menuName = "Buff Skill/Quick Recovery")]
public class QuickRecovery : Skills
{
    [SerializeField] GameObject effectSkill;
    public override void Activate()
    {
        GameObject obj = Instantiate(effectSkill, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.localScale = new Vector3(2, 2, 2);
        PlayerController.Instance.Health += Mathf.CeilToInt(damage);
    }
}
