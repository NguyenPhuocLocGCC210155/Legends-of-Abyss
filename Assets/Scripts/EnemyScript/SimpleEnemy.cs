using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    [SerializeField] float flipWaitTime;
    [SerializeField] float ledgeCheckX;
    [SerializeField] float ledgeCheckY;
    [SerializeField] LayerMask layerGroundToCheck;
    float timer;
    protected override void Start()
    {
        base.Start();
        CurrentStates = EnemyStates.Idle;
    }


    protected override void UpdateState()
    {
        if (hp <= 0)
        {
            Death(0.5f);
        }
        switch (CurrentStates)
        {
            case EnemyStates.Idle:
                Vector3 ledgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                if (!Physics2D.Raycast(transform.position + ledgeCheckStartPoint, Vector2.down, ledgeCheckY, layerGroundToCheck)
                 || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, layerGroundToCheck))
                {
                    ChangeState(EnemyStates.Flip);
                }

                if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                break;
            case EnemyStates.Flip:
                timer += Time.deltaTime;
                if (timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Idle);
                }
                break;
        }
    }
}
