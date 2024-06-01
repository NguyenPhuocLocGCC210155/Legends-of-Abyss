using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float recoilLenght ;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected float speed;
    [SerializeField] protected float _damage;
    [SerializeField] protected GameObject bloodEffect;
    [SerializeField] protected float countManaSFX = 5;
    protected bool isRecoiling = false;
    protected float recoilTimer;
    protected Rigidbody2D rb;   
    protected SpriteRenderer rd;
    protected Animator ani;
    protected Collider2D cd;
    protected ManaObjectPooling pool;

    protected enum EnemyStates{
        Idle,
        Flip,
        Chase,
        Stunned,
        Suprised,
        Charge,
        Death
    }
    protected EnemyStates currentStates;

    protected virtual EnemyStates CurrentStates {
        get => currentStates; 
        set {
            if (currentStates != value) 
            {
                currentStates = value;
                ChangeCurrentAnimation();
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        pool = GetComponent<ManaObjectPooling>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!PlayerController.Instance.isAlive)
        {
            ChangeState(EnemyStates.Idle);
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLenght)
            {
                recoilTimer += Time.deltaTime;
            }else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }else{
            UpdateState();
        }
    }

    protected virtual void ChangeCurrentAnimation(){}

    public virtual void EnemyHit(float _dameDone, Vector2 _hitDirection, float _hitForce){
        hp -= _dameDone;    
        if (!isRecoiling)
        {
            // GameObject _blood = Instantiate(bloodEffect, transform.position,Quaternion.identity);
            // Destroy(_blood, 2);
            for (int i = 0; i < countManaSFX; i++)
            {
                GameObject obj = pool.GetPooledObject();
            }
            rb.velocity = Vector2.zero;
            rb.velocity = _hitForce * recoilFactor * _hitDirection;
        }
    }

    protected virtual void Death(float _destroyTime){
        Destroy(gameObject, _destroyTime);
    }

    protected void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !PlayerController.Instance.isInvincible && hp > 0)
        {
            Attack();
            if (PlayerController.Instance.isAlive)
            {
                PlayerController.Instance.HitStopTime(0,5,0.5f);
            }
        }
    }

    protected virtual void Attack(){
        PlayerController.Instance.TakeDamage(_damage);
    }

    protected virtual void UpdateState(){}

    protected void ChangeState(EnemyStates _newState){
        CurrentStates = _newState;
    }
}
