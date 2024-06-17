using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkill : MonoBehaviour
{
    public float damage = 3;
    public float hitForce;
    public float speed;
    public float lifeTime = 1f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();
        Destroy(gameObject, lifeTime);
    }

}
