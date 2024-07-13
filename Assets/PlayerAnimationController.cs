using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Jump(bool value)
    {
        animator.SetBool("IsJump", value);
    }

    public void Fall(bool value)
    {
        animator.SetBool("IsFall", value);
    }

    public void FallToLand()
    {
        animator.SetBool("IsFall", false);
        animator.SetBool("IsJump", false);
    }

    public void Turn()
    {
        animator.SetTrigger("Turn");
    }

    public void Run(bool value)
    {
        animator.SetBool("IsWalk", value);
    }

    public void Dash()
    {
        animator.SetTrigger("Dash");
    }

    public void DoubleJump()
    {
        animator.SetTrigger("DoubleJump");
    }

    public void Slash()
    {
        animator.SetTrigger("Slash");
    }

    public void SlashUp()
    {
        animator.SetTrigger("SlashUp");
    }

    public void SlashDown()
    {
        animator.SetTrigger("SlashDown");
    }

    public void SlashSecond()
    {
        animator.SetTrigger("SlashSecond");
    }

    public void SlashSuper()
    {
        animator.SetTrigger("SlashSuper");
    }

    public void Kneel(bool value)
    {
        animator.SetBool("IsKneel", value);
    }

    public void Focus(bool value)
    {
        animator.SetBool("IsFocus", value);
    }

    public void EndFocus()
    {
        animator.SetTrigger("EndFocus");
    }

    public void WallSlide(bool value)
    {
        animator.SetBool("IsWallSlide", value);
    }

    public void WallJump()
    {
        animator.SetTrigger("WallJump");
    }

    public void WallSlash()
    {
        animator.SetTrigger("SlashWall");
    }

    public void Stun()
    {
        animator.SetTrigger("Stun");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    public void DeathBySpike()
    {
        animator.SetTrigger("DeathBySpike");
    }

    public void WakeUp()
    {
        animator.SetTrigger("WakeUp");
    }

    public void Respawn()
    {
        animator.SetTrigger("Respawn");
    }

    public void CastShootSkill()
    {
        animator.SetTrigger("CastShoot");
    }

    public void Fear(bool value)
    {
        if (value)
        {
            animator.SetBool("IsFear", value);
            animator.SetTrigger("Fear");
        }
        else
        {
            animator.SetBool("IsFear", value);
        }
    }

    public void Quake(bool value)
    {
        if (value)
        {
            animator.SetBool("IsQuake", value);
            animator.SetTrigger("Quake");
        }
        else
        {
            animator.SetBool("IsQuake", value);
        }
    }

    public void Scream()
    {
        animator.SetTrigger("Scream");
        animator.SetBool("IsScreamShadow", false);
    }

    public void ShadowScream(bool value)
    {
        if (value)
        {
            animator.SetTrigger("Scream");
            animator.SetBool("IsScreamShadow", value);
        }else{
            animator.SetBool("IsScreamShadow", value);
        }
    }
}
