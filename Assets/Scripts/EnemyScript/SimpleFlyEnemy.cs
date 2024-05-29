using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleFlyEnemy : Enemy
{
    [SerializeField] float stunDuration;
    public float chaseDistance;
    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Idle);
    }

    protected override void UpdateState(){
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        switch (CurrentStates)
        {
            case EnemyStates.Idle:
            if (_dist < chaseDistance)
            {
                ChangeState(EnemyStates.Chase);
            }
            break;

            case EnemyStates.Chase:
            rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
            Flip();
            break;

            case EnemyStates.Stunned:
            timer += Time.deltaTime;
            
            if (timer > stunDuration)
            {
                ChangeState(EnemyStates.Idle);
                timer = 0;
            }
            break;

            case EnemyStates.Death:
            Death(Random.Range(5,10));
            break;    
        }
    }

    public override void EnemyHit(float _dameDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_dameDone, _hitDirection, _hitForce);

        if (hp > 0)
        {
            ChangeState(EnemyStates.Stunned);
        }else{
            ChangeState(EnemyStates.Death);
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        ani.SetBool("Idle", CurrentStates == EnemyStates.Idle);
        ani.SetBool("Chase", CurrentStates == EnemyStates.Chase);
        ani.SetBool("Stun", CurrentStates == EnemyStates.Stunned);

        if (CurrentStates == EnemyStates.Death)
        {
            ani.SetTrigger("Death");
        }
    }

    protected override void Death(float _destroyTime)
    {

        rb.gravityScale = 12;
        this.gameObject.layer = 8;
        rd.color = new Color(130, 130, 130);
        base.Death(_destroyTime);
    }

    void Flip(){
        rd.flipX = PlayerController.Instance.transform.position.x < transform.position.x;
    }
}
