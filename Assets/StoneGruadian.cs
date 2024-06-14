using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StoneGruadian : Enemy
{
    public bool isFliped = false;
    public Vector2 sideAttackArea;
    public Vector2 upAttackArea;
    public float jumpForce;
    public Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] GameObject baseAttackSFX;
    [SerializeField] GameObject baseAttackSkill;
    [SerializeField] Transform sideAttackTransform;
    [SerializeField] Transform upAttackTransform;
    [SerializeField] Transform baseSkillTransform;
    [SerializeField] LayerMask playerLayer;
    List<Material> materials = new List<Material>();
    [SerializeField] LayerMask groundLayer;

    protected override void Start()
    {
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        pool = GetComponent<ObjectPooling>();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                materials.Add(child.GetComponent<SpriteRenderer>().material);
            }
        }
    }

    protected override void Update()
    {
        if (isDestroyed) { return; }
    }

    protected override IEnumerator DamageFlash()
    {
        SetFlashColor();
        float currentFlashAmount = 0;
        float elapsedTime = 0;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    protected override void SetFlashAmount(float amount)
    {
        // material.SetFloat("_FlashAmount", amount);
        foreach (Material m in materials)
        {
            m.SetFloat("_FlashAmount", amount);
        }
    }

    protected override void SetFlashColor()
    {
        // material.SetColor("_FlashColor", flashColor);
        foreach (Material m in materials)
        {
            m.SetColor("_FlashColor", flashColor);
        }
    }

    public override void EnemyHit(float _dameDone, Vector2 _hitDirection, float _hitForce)
    {
        if (hp > 0)
        {
            hp -= _dameDone;
            for (int i = 0; i < countManaSFX; i++)
            {
                GameObject obj = pool.GetPooledObject();
            }
            StartCoroutine(DamageFlash());
        }
        else
        {
            Death(1f);
        }
    }

    protected override void Death(float _destroyTime)
    {
        isDestroyed = true;
        GameObject effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        cd.enabled = false;
        gameObject.layer = 8;
        ani.SetTrigger("Death");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !PlayerController.Instance.isInvincible && PlayerController.Instance.Health > 0)
        {
            Attack();
            if (PlayerController.Instance.isAlive)
            {
                PlayerController.Instance.HitStopTime(0, 1, 1f);
            }
        }
    }

    protected override void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    public void BaseAttack()
    {
        GameObject attackEffect = Instantiate(baseAttackSFX);
        attackEffect.transform.position = sideAttackTransform.position;
        Destroy(attackEffect, 1f);
        Collider2D hit = Physics2D.OverlapBox(sideAttackTransform.position, sideAttackArea, 0, playerLayer);
        if (hit != null)
        {
            if (hit.gameObject.CompareTag("Player") && !PlayerController.Instance.isInvincible && hp > 0)
            {
                Attack();
                if (PlayerController.Instance.isAlive)
                {
                    PlayerController.Instance.HitStopTime(0, 1, 0.5f);
                    // PlayerController.Instance.isRecoilByAttack = true;
                }
            }
        }
    }

    public void BaseSkillAttack()
    {
        GameObject attackSkill = Instantiate(baseAttackSkill, baseSkillTransform.position, Quaternion.identity);
        if (!isFliped)
        {
            attackSkill.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            attackSkill.transform.eulerAngles = new Vector2(attackSkill.transform.eulerAngles.x, 180);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x < PlayerController.Instance.transform.position.x && isFliped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFliped = false;
        }
        else if (transform.position.x > PlayerController.Instance.transform.position.x && !isFliped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFliped = true;
        }
    }

    public bool isPlayerUp()
    {
        Collider2D hit = Physics2D.OverlapBox(upAttackTransform.position, upAttackArea, 0, playerLayer);
        if (hit != null)
        {
            return true;
        }
        return false;
    }

    public override void BeginAwaken()
    {
        ani.SetTrigger("Awake");
    }

    public bool GroundCheck()
    {
        return Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0, groundLayer);
    }

    public void DoneFallAttack(){
        StartCoroutine(WaitAndTransition());
        GameObject skillRight = Instantiate(baseAttackSkill, baseSkillTransform.position, Quaternion.identity);
        GameObject skillLeft = Instantiate(baseAttackSkill, baseSkillTransform.position, Quaternion.identity);
        skillRight.transform.eulerAngles = Vector3.zero;
        skillLeft.transform.eulerAngles = new Vector2(skillLeft.transform.eulerAngles.x, 180);
    }

    private IEnumerator WaitAndTransition()
    {
        // Đợi 2 giây
        yield return new WaitForSeconds(1f);

        // Chuyển đến state tiếp theo
        ani.SetTrigger("DoneFallAttack"); // Giả sử bạn sử dụng trigger để chuyển state

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.transform.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttackTransform.transform.position, upAttackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(baseSkillTransform.position, new Vector2(1, 1));
    }
}
