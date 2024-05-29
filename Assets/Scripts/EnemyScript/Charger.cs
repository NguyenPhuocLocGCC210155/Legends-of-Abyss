using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    [SerializeField] float ledgeCheckX;
    [SerializeField] float ledgeCheckY;
    [SerializeField] float chargerSpeedMultiplier;
    [SerializeField] float jumpForce;
    [SerializeField] float chargeDuration;
    [SerializeField] LayerMask layerGroundToCheck;


    float timer;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12;
        ChangeState(EnemyStates.Idle);
    }


    protected override void UpdateState()
    {
        if (hp <= 0)
        {
            Death(0.5f);
        }
        Vector3 ledgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;
        switch (CurrentStates)
        {
            case EnemyStates.Idle:
                if (!Physics2D.Raycast(transform.position + ledgeCheckStartPoint, Vector2.down, ledgeCheckY, layerGroundToCheck)
                 || Physics2D.Raycast(transform.position, wallCheckDir, ledgeCheckX, layerGroundToCheck))
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position + ledgeCheckStartPoint, wallCheckDir, ledgeCheckX * 10);
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Suprised);
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

            case EnemyStates.Suprised:
                rb.velocity = new Vector2(0, jumpForce);
                ChangeState(EnemyStates.Charge);
                break;

            case EnemyStates.Charge:
                timer += Time.deltaTime;

                if (timer < chargeDuration)
                {
                    if (Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, layerGroundToCheck))
                    {
                        if (transform.localScale.x > 0)
                        {
                            rb.velocity = new Vector2(speed * chargerSpeedMultiplier, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed * chargerSpeedMultiplier, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Idle);
                }
                break;
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        if (CurrentStates == EnemyStates.Idle)
        {
            ani.speed = 1;
        }

        if (CurrentStates == EnemyStates.Charge)
        {
            ani.speed = chargerSpeedMultiplier;
        }
    }
}
