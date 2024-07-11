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

    public void Jump(bool value){
        animator.SetBool("IsJump", value);
    }

    public void Fall(bool value){
        animator.SetBool("IsFall", value);
    }

    public void FallToLand(){
        animator.SetBool("IsFall", false);
        animator.SetBool("IsJump", false);
    }

    public void Turn(){
        animator.SetTrigger("Turn");
    }

    public void Run(bool value){
        animator.SetBool("IsWalk", value);
    }

    public void Dash(){
        animator.SetTrigger("Dash");
    }

    public void DoubleJump(){
        animator.SetTrigger("DoubleJump");
    }

    public void Slash(){
        animator.SetTrigger("Slash");
    }

    public void SlashUp(){
        animator.SetTrigger("SlashUp");
    }

    public void SlashDown(){
        animator.SetTrigger("SlashDown");
    }

    public void SlashSecond(){
        animator.SetTrigger("SlashSecond");
    }

    public void Kneel(bool value) {
        animator.SetBool("IsKneel", value);
    }

    public void Focus(){
        animator.SetBool("IsFoucs", true);
    }

    public void EndFocus(){
        animator.SetTrigger("EndFocus");
    }

    public void WallSlide(bool value){
        animator.SetBool("IsWallSlide", value );
    }

    public void WallJump(){
        animator.SetTrigger("WallJump");
    }

    public void WallSlash(){
        animator.SetTrigger("SlashWall");
    }

    public void Stun(){
        animator.SetTrigger("Stun");
    }

    public void Death(){
        animator.SetTrigger("Death");
    }

    public void DeathBySpike(){
        animator.SetTrigger("DeathBySpike");
    }
}
