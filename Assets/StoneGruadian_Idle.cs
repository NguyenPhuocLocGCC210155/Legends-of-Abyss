using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGruadian_Idle : StateMachineBehaviour
{
    StoneGruadian boss;
    Rigidbody2D rb;
    Animator ani;
    float cooldownBaseSkill;
    float cooldownJump;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<StoneGruadian>();
        ani = animator.GetComponent<Animator>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        MoveToPlayer();
        cooldownBaseSkill += Time.deltaTime;
        cooldownJump += Time.deltaTime;
        
        if(cooldownBaseSkill >= 2 && Vector2.Distance(PlayerController.Instance.transform.position, rb.position) >= boss.sideAttackArea.x + 1){
            cooldownBaseSkill = 0;
            ani.SetTrigger("SkillAttack");
        }
        else if(cooldownJump >= 3 && cooldownBaseSkill < 3 && Vector2.Distance(PlayerController.Instance.transform.position, rb.position) >= boss.sideAttackArea.x + 3){
            cooldownJump = 0;
            ani.SetTrigger("Jump");
        }
        else if (Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= boss.sideAttackArea.x + 1)
        {
            ani.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    void MoveToPlayer()
    {
        Vector2 target = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, boss.speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

}
