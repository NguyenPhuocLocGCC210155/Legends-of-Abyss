using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGruadian_Jump : StateMachineBehaviour
{
    Rigidbody2D rb;
    StoneGruadian boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<StoneGruadian>();
        JumpToPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = 1;
        if (rb.velocity.y <= 0)
        {
            animator.SetBool("IsFall", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

    void JumpToPlayer()
    {
        boss.LookAtPlayer();
        Vector2 direction = (PlayerController.Instance.transform.position - rb.transform.position).normalized;

        // Tính toán lực nhảy
        Vector2 jumpVector = new Vector2((direction.x * boss.speed) / 2f, boss.jumpForce);

        boss.GetComponent<AudioSource>().PlayOneShot(boss.jumpSound);
        // Áp dụng lực nhảy
        rb.AddForce(jumpVector, ForceMode2D.Impulse);

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
