using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationAndAudioController : MonoBehaviour
{
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip doubleJump;
    [SerializeField] AudioClip wallJump;
    [SerializeField] AudioClip wallSlide;
    [SerializeField] AudioClip slashFirst;
    [SerializeField] AudioClip slashSecond;
    [SerializeField] AudioClip shootSkill;
    [SerializeField] AudioClip takeDamage;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip shadowDash;
    [SerializeField] AudioClip refillShadowDashFirst;
    [SerializeField] AudioClip refillShadowDashSecond;
    [SerializeField] AudioClip fall;
    [SerializeField] AudioClip fallToLand;
    [SerializeField] AudioClip CompleteAwaken;
    [SerializeField] AudioClip quakePrepare;
    [SerializeField] AudioClip quakeImpact;
    [SerializeField] AudioClip runOnGrass;
    [SerializeField] AudioClip focusHealth;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioSource wallSildeaudioSource;
    [SerializeField] AudioSource focusAudioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Jump(bool value)
    {
        animator.SetBool("IsJump", value);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jump);
    }

    public void Fall(bool value)
    {
        animator.SetBool("IsFall", value);
    }

    public void FallToLand()
    {
        animator.SetBool("IsFall", false);
        animator.SetBool("IsJump", false);
        audioSource.PlayOneShot(fallToLand);
    }

    public void Turn()
    {
        animator.SetTrigger("Turn");
    }

    public void Run(bool value)
    {
        animator.SetBool("IsWalk", value);
        PlayRun(value, PlayerController.Instance.LastOnGroundTime > 0);
    }

    public void PlayRun(bool isInput, bool isGround)
    {
        if (isInput && isGround)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = runOnGrass;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.clip = null;
        }
    }

    public void Dash()
    {
        animator.SetTrigger("Dash");
        audioSource.PlayOneShot(dash);
    }

    public void DoubleJump()
    {
        animator.SetTrigger("DoubleJump");
        audioSource.PlayOneShot(doubleJump);
    }

    public void Slash()
    {
        animator.SetTrigger("Slash");
        audioSource.PlayOneShot(slashFirst);
    }

    public void SlashUp()
    {
        animator.SetTrigger("SlashUp");
        audioSource.PlayOneShot(slashFirst);
    }

    public void SlashDown()
    {
        animator.SetTrigger("SlashDown");
        audioSource.PlayOneShot(slashFirst);
    }

    public void SlashSecond()
    {
        animator.SetTrigger("SlashSecond");
        audioSource.PlayOneShot(slashSecond);
    }

    public void SlashSuper()
    {
        animator.SetTrigger("SlashSuper");
    }

    public void WallSlash()
    {
        animator.SetTrigger("SlashWall");
        audioSource.PlayOneShot(slashFirst);
    }

    public void Kneel(bool value)
    {
        animator.SetBool("IsKneel", value);
    }

    public void Focus(bool value)
    {
        animator.SetBool("IsFocus", value);
        if (value)
        {
            if (!focusAudioSource.isPlaying)
            {
                focusAudioSource.Play();
            }
        }
        else
        {
            focusAudioSource.Stop();
        }
    }

    public void EndFocus()
    {
        animator.SetTrigger("EndFocus");
        audioSource.PlayOneShot(focusHealth);
    }

    public void WallSlide(bool value)
    {
        animator.SetBool("IsWallSlide", value);
        PlayWallSlide(value, PlayerController.Instance._isWallSliding);
    }
    public void PlayWallSlide(bool isInput, bool isWall)
    {
        if (isInput && isWall)
        {
            if (!wallSildeaudioSource.isPlaying)
            {
                wallSildeaudioSource.clip = wallSlide;
                wallSildeaudioSource.Play();
            }
        }
        else
        {
            wallSildeaudioSource.Stop();
            wallSildeaudioSource.clip = null;
        }
    }

    public void WallJump()
    {
        animator.SetTrigger("WallJump");
        audioSource.PlayOneShot(wallJump);
    }

    public void Stun()
    {
        animator.SetTrigger("Stun");
        audioSource.PlayOneShot(takeDamage);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        audioSource.PlayOneShot(death);
    }

    public void DeathBySpike()
    {
        animator.SetTrigger("DeathBySpike");
        audioSource.PlayOneShot(takeDamage);
    }

    public void WakeUp()
    {
        animator.SetTrigger("WakeUp");
    }

    public bool isLie()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        // Nếu có clip đang phát trong layer này
        if (clipInfo.Length > 0)
        {
            string clipName = clipInfo[0].clip.name;
            return clipName.Equals("TheKnight_Lie");
        }
        else
        {
            return false;
        }
    }

    public void Respawn()
    {
        animator.SetTrigger("Respawn");
    }

    public void CastShootSkill()
    {
        animator.SetTrigger("CastShoot");
        audioSource.PlayOneShot(shootSkill);
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
            audioSource.PlayOneShot(quakePrepare);
        }
        else
        {
            animator.SetBool("IsQuake", value);
            audioSource.PlayOneShot(quakeImpact);
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
        }
        else
        {
            animator.SetBool("IsScreamShadow", value);
        }
    }

    public void OpenMap(bool value)
    {
        animator.SetBool("IsOpenMap", value);
    }

    public void LookUp(bool value)
    {
        animator.SetBool("IsLookUp", value);
    }

    public void LookDown(bool value)
    {
        animator.SetBool("IsLookDown", value);
    }
}
