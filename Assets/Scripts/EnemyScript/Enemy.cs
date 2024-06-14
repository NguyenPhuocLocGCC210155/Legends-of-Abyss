using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float recoilLenght ;
    [SerializeField] protected float recoilFactor;
    public float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected GameObject bloodEffect;
    [SerializeField] protected float countManaSFX = 5;
    [ColorUsage(true, true )]
    [SerializeField] protected Color flashColor = Color.white;
    [SerializeField] protected float flashTime = 0.25f;
    protected Material material;
    protected bool isRecoiling = false;
    protected float recoilTimer;
    protected Rigidbody2D rb;   
    protected SpriteRenderer rd;
    protected Animator ani;
    protected Collider2D cd;
    protected ObjectPooling pool;
    protected bool isDestroyed = false;

    protected enum EnemyStates{
        Awake,
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
        pool = GetComponent<ObjectPooling>();
        material = rd.material;
    }

    protected virtual IEnumerator DamageFlash(){
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

    protected virtual void SetFlashAmount(float amount){
        material.SetFloat("_FlashAmount", amount);
    }

    protected virtual void SetFlashColor(){
        material.SetColor("_FlashColor", flashColor);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDestroyed) {return;}

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
            StartCoroutine(DamageFlash());
        }
    }

    protected virtual void Death(float _destroyTime){
        isDestroyed = true;
        GameObject effect = Instantiate(bloodEffect);
        effect.transform.position = transform.position;
        Destroy(effect, 3f);
        Destroy(gameObject, _destroyTime);
    }

    protected void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !PlayerController.Instance.isInvincible && hp > 0)
        {
            Attack();
            if (PlayerController.Instance.isAlive)
            {
                PlayerController.Instance.HitStopTime(0, 1f,0.5f);
            }
        }
    }

    protected virtual void Attack(){
        PlayerController.Instance.TakeDamage(damage);
    }

    protected virtual void UpdateState(){}

    protected void ChangeState(EnemyStates _newState){
        CurrentStates = _newState;
    }

    public virtual void BeginAwaken(){}
}
