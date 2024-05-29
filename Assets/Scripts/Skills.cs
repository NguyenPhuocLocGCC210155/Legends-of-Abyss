using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills : ScriptableObject
{
    [SerializeField] float manaCosumed;
    [SerializeField] float damage;

    public virtual void Active(){
    }
}
