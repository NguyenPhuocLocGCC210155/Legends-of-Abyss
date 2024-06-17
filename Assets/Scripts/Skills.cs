using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public float manaCosumed;
    public float damage;
    public float castingTime = 1f;

    public abstract void Activate();
}
