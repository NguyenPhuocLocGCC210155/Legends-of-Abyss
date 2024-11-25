using System;
using System.Collections;
using System.Data;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. If you don't want to use it
	//just paste in all the parameters, though you will need to manuly change all references in this script
	public PlayerData Data;
	public static PlayerController Instance;
	#region COMPONENTS
	public Rigidbody2D RB { get; private set; }
	[HideInInspector]
	public SkillManager skillManager;
	//Script to handle all player animations, all references can be safely removed if you're importing into your own project.
	[HideInInspector] public Animator animator;
	[HideInInspector] public PlayerAnimationAndAudioController playerAnimationAndAudio;
	SpriteRenderer sprite;
	#endregion

	#region STATE PARAMETERS
	//Variables control the various actions the player can perform at any time.
	//These are fields which can are public allowing for other sctipts to read them
	//but can only be privately written to.
	public bool IsFacingRight { get; private set; }
	public bool _isJumping { get; private set; }
	public bool IsDashing { get; private set; }
	public bool IsSliding { get; private set; }
	[HideInInspector] public bool isAlive;
	[HideInInspector] public bool canControl;
	[HideInInspector] public bool isCutScene = false;
	bool isShadowStyle;

	//Timers (also all fields, could be private and a method returning a bool could be used)
	[HideInInspector] public float LastOnGroundTime;
	public float LastOnWallTime { get; private set; }

	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;
	private int airJumpCounter = 0;
	[Range(0, 2)]
	[SerializeField] int maxAirJump;

	//Wall Jump
	public bool _isWallSliding { get; private set; }
	private bool _isWallJumping;
	private float wallJumpDirection;
	private float wallJumpDuration = 0.4f;

	//Dash
	private int _dashesLeft;
	private bool _dashRefilling;
	private Vector2 _lastDashDir;
	private bool _isDashAttacking;
	private float LastTimeShadowDash = 0;
	#endregion

	#region Effect
	[SerializeField] GameObject focusEffect;
	[SerializeField] GameObject focusEndEffect;
	[SerializeField] GameObject deathEffect;
	[SerializeField] GameObject dashEffect;
	[SerializeField] GameObject shadowDashEffect;
	[SerializeField] GameObject refillShadowDashEffect;
	[SerializeField] GameObject doubleJumpEffect;
	[SerializeField] GameObject slashEffect;
	[SerializeField] GameObject slashSecondEffect;
	[SerializeField] GameObject slashUpEffect;
	[SerializeField] GameObject slashDownEffect;
	#endregion

	#region INPUT PARAMETERS
	private Vector2 _moveInput;
	public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; private set; }
	[Space(5)]
	#endregion

	#region 
	public int keys;
	public int heartShards;
	public int chamberCount = 1;
	public bool isUnlockDash;
	public bool isUnlockWallJump;
	public bool isUnlockDoubleJump;
	public bool isUnlockLantern;
	public bool isUnlockMedal;
	public bool isUnlockPosion;
	#endregion

	#region CHECK PARAMETERS
	[Header("Checks settings")]
	[SerializeField] float fallSpeedThreshold = -10;
	[SerializeField] private Transform _groundCheckPoint;
	bool isGround = false;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
	[Space(5)]
	#endregion

	[Header("Attack settings")]
	[SerializeField] float dmg = 1;
	[SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform, backAttackTransform;
	public Transform SideAttackTransform { get => sideAttackTransform; private set => sideAttackTransform = value; }
	[SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
	[Range(0, 1)]
	[SerializeField] float timeSinceAttack;
	bool isAttackCombo;
	bool canAttack = false;
	[SerializeField] float timeBetweenAttack;
	bool isRestoreTime;
	float restoreTime;
	[Space(5)]

	[Header("Recoil settings")]
	[SerializeField] int recoilXStep = 5;
	[SerializeField] int recoilYStep = 5;
	[HideInInspector] public bool isRecoilingX, isRecoilingY, isRecoilByAttack;
	[SerializeField] float recoilYSpeed;
	[SerializeField] float recoilXSpeed;
	int stepsXRecoiled, stepsYRecoiled, stepRecoildAttack;


	[Header("Mana settings")]
	public float mana;
	[SerializeField] float manaDrainSpeed;
	[SerializeField] float manaGain;
	[SerializeField] Image manaStorage;

	[Header("Health settings")]
	[SerializeField] float hitFlashSpeed;
	[SerializeField] GameObject bloodSpurt;
	[SerializeField] float timeToHeal;
	[HideInInspector] public delegate void OnHealthChangeDelegate();
	[HideInInspector] public OnHealthChangeDelegate OnhealthChangeCallBack;
	[HideInInspector] public bool isLie;
	int hp;
	public int maxHp;
	public int maxTotalHealth = 10;
	public bool isInvincible;
	bool isHealing;
	bool isOpenMap;
	bool isOpenInventory;
	bool isLook;
	float healTimer;


	#region LAYERS & TAGS
	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] LayerMask attackableLayerMask;
	[SerializeField] LayerMask _wallLayer;
	#endregion


	private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		skillManager = GetComponent<SkillManager>();
		playerAnimationAndAudio = GetComponent<PlayerAnimationAndAudioController>();
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
		Health = maxHp;
		canControl = true;
		isAlive = true;
		ColliderCheck();
	}

	private void Update()
	{
		if (isAlive)
		{
			ColliderCheck();
			CheckGravity();
			if (canControl)
			{
				if (!isCutScene && !isRecoilByAttack)
				{

					Healing();
					if (!isHealing)
					{
						CheckInputAndParemeter();
						ToggleMap();
						ToggleInventory();
						ToggleLook();
						JumpCheck();
						DashCheck();
						Attack();
						WallSlide();
					}
				}
				else
				{
					return;
				}
			}
		}

		if (isCutScene)
		{
			ColliderCheck();
			return;
		}
		RestoreTimeSale();
		UpdateCameraYDampForPlayerFall();
		FlashWhileInvicible();
	}
	private void FixedUpdate()
	{
		RestoreTimeSale();
		if (!isAlive)
		{
			return;
		}
		if (canControl && !isHealing)
		{
			if (!isCutScene)
			{
				Recoil();
				RecoilByAttack();
				if (IsDashing)
				{
					return;
				}
				//Handle Run
				if (!IsDashing && !isRecoilingX && canControl && !isRecoilByAttack)
				{
					if (!_isWallJumping)
					{
						Run(1);
					}
				}
				else if (_isDashAttacking && canControl)
				{
					Run(Data.dashEndRunLerp);
				}

				//Handle Slide
				// if (IsSliding && canControl)
				// 	Slide();
			}
			else
			{
				return;
			}
		}
	}

	#region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
	public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut())
			_isJumpCut = true;
	}

	public void OnDashInput()
	{
		LastPressedDashTime = Data.dashInputBufferTime;
	}
	#endregion

	#region GENERAL METHODS
	public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	private void Sleep(float duration)
	{
		//Method used so we don't need to call StartCoroutine everywhere
		//nameof() notation means we don't need to input a string directly.
		//Removes chance of spelling mistakes and will improve error messages if any
		StartCoroutine(nameof(PerformSleep), duration);
	}

	private IEnumerator PerformSleep(float duration)
	{
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
		Time.timeScale = 1;
	}
	#endregion
	#region  ATTACK METHODS
	void Attack()
	{
		timeSinceAttack += Time.deltaTime;
		if (canAttack && timeSinceAttack >= timeBetweenAttack)
		{
			timeSinceAttack = 0;
			if (_moveInput.x != 0 && LastOnWallTime > 0)
			{
				playerAnimationAndAudio.WallSlash();
				int recoilLeftOrRight = IsFacingRight ? 1 : -1;
				hitBox(backAttackTransform, sideAttackArea, ref isRecoilingX, Vector2.right * recoilLeftOrRight, recoilXSpeed);
				SlashEffectAttacking(slashEffect, 135, backAttackTransform);
			}
			else if (_moveInput.y < 0 && LastOnGroundTime < -0.1f)
			{
				playerAnimationAndAudio.SlashDown();
				hitBox(downAttackTransform, downAttackArea, ref isRecoilingY, Vector2.down, recoilYSpeed);
				// SlashEffectAttacking(slashEffect, -90, downAttackTransform);
				Instantiate(slashDownEffect, downAttackTransform);
			}
			else if (_moveInput.y > 0)
			{
				playerAnimationAndAudio.SlashUp();
				hitBox(upAttackTransform, upAttackArea, ref isRecoilingY, Vector2.up, recoilYSpeed);
				// SlashEffectAttacking(slashEffect, 90, upAttackTransform);
				Instantiate(slashUpEffect, upAttackTransform);
			}
			else if (_moveInput.y == 0 || _moveInput.y < 0 && isGround)
			{
				int recoilLeftOrRight = IsFacingRight ? 1 : -1;
				hitBox(SideAttackTransform, sideAttackArea, ref isRecoilingX, Vector2.right * recoilLeftOrRight, recoilXSpeed);
				if (RB.velocity.y == 0)
				{
					if (isAttackCombo)
					{
						playerAnimationAndAudio.SlashSecond();
						isAttackCombo = false;
						Instantiate(slashSecondEffect, SideAttackTransform);
					}
					else
					{
						playerAnimationAndAudio.Slash();
						isAttackCombo = true;
						Instantiate(slashEffect, SideAttackTransform);
					}
				}
				else
				{
					playerAnimationAndAudio.Slash();
					Instantiate(slashEffect, SideAttackTransform);
				}
			}
		}
	}

	void hitBox(Transform attackTransform, Vector2 attackArea, ref bool _isRecoilDir, Vector2 _recoilDir, float _recoilStregth)
	{
		Collider2D[] objectToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, attackableLayerMask);
		if (objectToHit.Length > 0)
		{
			_isRecoilDir = true;
		}
		for (int i = 0; i < objectToHit.Length; i++)
		{
			if (objectToHit[i].GetComponent<Enemy>() != null)
			{
				// objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, ((transform.position - objectToHit[i].transform.position).normalized * -1), _recoilStregth);
				objectToHit[i].GetComponent<Enemy>().EnemyHit(dmg, _recoilDir, _recoilStregth);
				// if (objectToHit[i].CompareTag("Enemy"))
				// {
				// 	Mana += manaGain;
				// }
			}

			if (objectToHit[i].GetComponent<BreakWall>() != null)
			{
				objectToHit[i].GetComponent<BreakWall>().TakeDamage();
			}
		}
	}

	void SlashEffectAttacking(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
	{
		_slashEffect = Instantiate(_slashEffect, _attackTransform);
		_slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle * transform.localScale.x);
		_slashEffect.transform.localScale = new Vector2(_slashEffect.transform.localScale.x, _slashEffect.transform.localScale.y);
	}

	public void RecoilByAttack()
	{
		if (isRecoilByAttack)
		{
			if (IsFacingRight == true)
			{
				RB.velocity = new Vector2(-recoilXSpeed * 4, recoilYSpeed * 3);
			}
			else
			{
				RB.velocity = new Vector2(recoilXSpeed * 4, recoilYSpeed * 3);
			}

			if (stepRecoildAttack < recoilXStep)
			{
				stepRecoildAttack++;
			}
			else
			{
				StopRecoildByAttack();
			}
		}
	}

	void Recoil()
	{
		if (isRecoilingX)
		{
			if (IsFacingRight == true)
			{
				RB.velocity = new Vector2(-recoilXSpeed, 0);
			}
			else
			{
				RB.velocity = new Vector2(recoilXSpeed, 0);
			}
		}

		if (isRecoilingY)
		{
			// SetGravityScale(0);
			if (_moveInput.y < 0)
			{
				// RB.velocity = Vector2.zero;
				RB.velocity = new Vector2(RB.velocity.x, recoilYSpeed);
			}
			else
			{
				// RB.velocity = Vector2.zero;
				RB.velocity = new Vector2(RB.velocity.x, -recoilYSpeed);
			}
			airJumpCounter = 0;
		}
		//Stop recoil
		if (isRecoilingX && stepsXRecoiled < recoilXStep)
		{
			stepsXRecoiled++;
		}
		else
		{
			StopRecoilX();
		}
		if (isRecoilingY && stepsYRecoiled < recoilYStep)
		{
			stepsYRecoiled++;
		}
		else
		{
			StopRecoilY();
		}
		if (LastOnGroundTime > 0)
		{
			StopRecoilY();
		}
	}

	void StopRecoilX()
	{
		stepsXRecoiled = 0;
		isRecoilingX = false;
		if (!canControl)
		{
			RB.velocity = new Vector2(0, 0);
		}
	}

	void StopRecoilY()
	{
		stepsYRecoiled = 0;
		isRecoilingY = false;
	}

	void StopRecoildByAttack()
	{
		isRecoilByAttack = false;
		stepRecoildAttack = 0;
		RB.velocity = new Vector2(0, 0);
	}
	#endregion

	#region HEALTH METHODS
	void Healing()
	{
		if (Input.GetKey(KeyCode.S) && Health < maxHp && Mana > 0 && LastOnGroundTime > 0 && !IsDashing)
		{
			if (!isHealing)
			{
				focusEffect.SetActive(true);
			}
			playerAnimationAndAudio.Focus(true);
			isHealing = true;
			healTimer += Time.deltaTime;
			if (healTimer >= timeToHeal)
			{
				Health++;
				healTimer = 0;
				playerAnimationAndAudio.EndFocus();
				isHealing = false;
				focusEffect.SetActive(false);
				Instantiate(focusEndEffect, transform);
			}
			Mana -= Time.deltaTime * manaDrainSpeed;
		}
		else
		{
			isHealing = false;
			focusEffect.SetActive(false);
			playerAnimationAndAudio.Focus(false);
			healTimer = 0;
		}
	}

	public void TakeDamage(float _dmg, bool isSpike)
	{
		if(isShadowStyle){
			return;
		}
		if (isAlive)
		{
			healTimer = 0;
			Health -= Mathf.RoundToInt(_dmg);
			if (Health <= 0)
			{
				Health = 0;
				StartCoroutine(Death());
			}
			else
			{
				if (isSpike)
				{
					playerAnimationAndAudio.DeathBySpike();
				}
				else
				{
					playerAnimationAndAudio.Stun();
				}
				StartCoroutine(StopTakingDamaged());
			}
		}
	}

	public void TakeDamage(float _dmg)
	{
		if (isAlive)
		{
			healTimer = 0;
			Health -= Mathf.RoundToInt(_dmg);
			LastPressedJumpTime = 0;
			if (Health <= 0)
			{
				Health = 0;
				StartCoroutine(Death());
			}
			else
			{
				playerAnimationAndAudio.Stun();
				StartCoroutine(StopTakingDamaged());
			}
		}
	}

	void FlashWhileInvicible()
	{
		sprite.material.color = isInvincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1f)) : Color.white;
	}

	void RestoreTimeSale()
	{
		if (isRestoreTime)
		{
			if (Time.timeScale < 1)
			{
				Time.timeScale += Time.unscaledDeltaTime * restoreTime;
				if (Time.timeScale >= 1)
				{
					Time.timeScale = 1;
					isRestoreTime = false;
				}
			}
			else
			{
				Time.timeScale = 1;
				isRestoreTime = false;
			}
		}
	}

	public void HitStopTime(float _newTimeScale, float _restoreTime)
	{
		if (hp > 0)
		{
			restoreTime = _restoreTime;

			StopCoroutine(StartTimeAgain(0.2f));
			StartCoroutine(StartTimeAgain(0.2f));

			Time.timeScale = _newTimeScale;
		}
	}

	IEnumerator StartTimeAgain(float _delay)
	{
		yield return new WaitForSecondsRealtime(_delay);
		isRestoreTime = true; // Đảm bảo isRestoreTime được đặt lại
	}

	IEnumerator StopTakingDamaged()
	{
		isInvincible = true;
		GameObject _bloodSpurtParticle = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
		Destroy(_bloodSpurtParticle, 1.5f);
		yield return new WaitForSeconds(1f);
		isInvincible = false;
	}

	public int Health
	{
		get { return hp; }
		set
		{
			if (hp != value)
			{
				hp = Mathf.Clamp(value, 0, maxHp);

				if (OnhealthChangeCallBack != null)
				{
					OnhealthChangeCallBack.Invoke();
				}
			}
		}
	}
	#endregion

	#region MANA
	public float Mana
	{
		get { return mana; }
		set
		{
			if (mana != value)
			{
				mana = Mathf.Clamp(value, 0, 1);
			}
		}
	}

	#endregion

	//MOVEMENT METHODS
	#region RUN METHODS
	private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Nhận giá trị gia tốc dựa trên việc chúng ta đang tăng tốc (bao gồm cả việc rẽ) hay đang cố gắng giảm tốc (dừng). Cũng như áp dụng hệ số nhân nếu chúng ta bay trên không.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((_isJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		if (isOpenMap)
		{
			RB.velocity = new Vector2(movement / 100, RB.velocity.y);
		}
		else
		{
			RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
		}

		if (canControl)
		{
			playerAnimationAndAudio.Run(_moveInput.x != 0 && LastOnGroundTime > -1);
		}

		/*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		if (!IsDashing && canControl)
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
			playerAnimationAndAudio.Turn();
		}
	}
	#endregion

	#region JUMP METHODS
	private void Jump()
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = Data.jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		playerAnimationAndAudio.Jump(true);
		#endregion
	}

	private void DoubleJump()
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		RB.velocity = new Vector2(0, 0);
		RB.velocity = new Vector3(RB.velocity.x, Data.jumpForce / 1.3f);
		playerAnimationAndAudio.DoubleJump();
		Instantiate(doubleJumpEffect, transform);
	}

	private void WallJump()
	{
		playerAnimationAndAudio.WallJump();
		playerAnimationAndAudio.WallSlide(false);
		if (_isWallSliding)
		{
			_isWallJumping = false;
			CancelInvoke(nameof(StopWallJumping));
		}
		RB.velocity = new Vector2(wallJumpDirection * Data.wallJumpForce.x, Data.wallJumpForce.y);
		LastOnWallTime = 0;
		Invoke(nameof(StopWallJumping), wallJumpDuration);
		Turn();
	}

	private void StopWallJumping()
	{
		_isWallJumping = false;
	}
	#endregion

	#region DASH METHODS
	//Dash Coroutine
	private IEnumerator StartDash(Vector2 dir)
	{
		//Overall this method of dashing aims to mimic Celeste, if you're looking for
		// a more physics-based approach try a method similar to that used in the jump
		LastOnGroundTime = 0;
		LastPressedDashTime = 0;

		float startTime = Time.time;

		if (isUnlockMedal && LastTimeShadowDash <= 0)
		{
			playerAnimationAndAudio.DashShadow();
			isShadowStyle = true;
			ImmuneDamage(true);
			Instantiate(shadowDashEffect, backAttackTransform);
		}else{
			playerAnimationAndAudio.Dash();
			Instantiate(dashEffect, transform);
		}
		_dashesLeft--;
		_isDashAttacking = true;

		SetGravityScale(0);

		//We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
		while (Time.time - startTime <= Data.dashAttackTime)
		{
			RB.velocity = dir.normalized * Data.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 
			//This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
			yield return null;
		}

		startTime = Time.time;

		_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		SetGravityScale(Data.gravityScale);
		RB.velocity = Data.dashEndSpeed * dir.normalized * 0.5f;

		while (Time.time - startTime <= Data.dashEndTime)
		{
			yield return null;
		}

		//Dash over
		if (isShadowStyle)
		{
			ImmuneDamage(false);
			isShadowStyle = false;
			LastTimeShadowDash = 1;
			Instantiate(refillShadowDashEffect, transform);
		}
		IsDashing = false;
		playerAnimationAndAudio.Jump(false);
		playerAnimationAndAudio.Fall(false);
	}

	//Short period before the player is able to dash again
	private IEnumerator RefillDash(int amount)
	{
		//SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
		_dashRefilling = true;
		yield return new WaitForSeconds(Data.dashRefillTime);
		_dashRefilling = false;
		_dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
	}
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		if (_moveInput.x != 0)
		{
			//Chúng tôi loại bỏ các xung động còn lại để tránh trượt lên trên
			if (RB.velocity.y > 0)
			{
				RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
			}

			//Works the same as the Run but only in the y-axis
			//THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
			float speedDif = Data.slideSpeed - RB.velocity.y;
			float movement = speedDif * Data.slideAccel;
			//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
			//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
			movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
			RB.AddForce(movement * Vector2.up);
		}
	}
	#endregion


	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		return LastOnGroundTime > 0 && !_isJumping;
	}

	private bool CanJumpCut()
	{
		return _isJumping && RB.velocity.y > 0;
	}

	private void JumpCheck()
	{
		if (_isJumping && RB.velocity.y < 0)
		{
			_isJumping = false;

			_isJumpFalling = true;
		}

		if (!IsDashing)
		{
			//Jump
			if (CanJump() && LastPressedJumpTime > 0)
			{
				_isJumping = true;
				_isJumpCut = false;
				_isJumpFalling = false;
				_isWallJumping = false;
				Jump();
				playerAnimationAndAudio.PlayJump();
			}
			//Wall Jump
			else if (Input.GetKeyDown(KeyCode.Space) && LastOnGroundTime < 0 && LastOnWallTime >= 0 && isUnlockWallJump)
			{
				WallJump();
				_isWallJumping = true;
				_isJumping = true;
				_isJumpCut = false;
				_isJumpFalling = false;
			}
			//Double Jump
			else if (airJumpCounter < maxAirJump && Input.GetKeyDown(KeyCode.Space) && isUnlockDoubleJump)
			{
				_isJumping = true;
				_isJumpCut = false;
				_isJumpFalling = false;
				_isWallJumping = false;
				airJumpCounter++;
				if (Input.GetKeyDown(KeyCode.Space) && !isLie && !isOpenInventory)
				{
					DoubleJump();
					isOpenMap = false;
				}
				else if (Input.GetKeyUp(KeyCode.Space) && isLie)
				{
					WakeUp();
				}
			}
		}
	}

	private void DashCheck()
	{
		if (CanDash() && LastPressedDashTime > 0 && isUnlockDash)
		{
			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			//If not direction pressed, dash forward
			if (_moveInput.x != 0)
				_lastDashDir.x = _moveInput.x;
			else
				_lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

			if (LastOnWallTime >= 0)
			{
				Turn();
				_lastDashDir.x *= -1f;
			}

			IsDashing = true;
			_isJumping = false;
			_isJumpCut = false;

			StartCoroutine(nameof(StartDash), _lastDashDir);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
		{
			playerAnimationAndAudio.FallToLand();
			playerAnimationAndAudio.Jump(false);
			playerAnimationAndAudio.Fall(false);
			playerAnimationAndAudio.WallSlide(false);
			_isJumpCut = false;
			_isJumping = false;
			airJumpCounter = 0;
			LastOnGroundTime = Data.coyoteTime;
		}
	}

	private void ColliderCheck()
	{
		if (!IsDashing && !_isJumping)
		{
			//Ground Check
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
			{
				RB.velocity = new Vector2(RB.velocity.x, 0);
				if (LastOnGroundTime < -0.1f)
				{
					isGround = true;
					airJumpCounter = 0;
					playerAnimationAndAudio.Jump(false);
					playerAnimationAndAudio.Fall(false);
				}
				LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
			}
			else
			{
				isGround = false;
			}
		}
		//Wall check
		if (Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _wallLayer) && LastOnGroundTime < 0)
		{
			wallJumpDirection = -transform.localScale.x;
			LastOnWallTime = Data.coyoteTime;
			airJumpCounter = 0;
			playerAnimationAndAudio.WallSlide(true);
		}
	}

	private bool CanDash()
	{

		if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnWallTime > 0 && !_dashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		return _dashesLeft > 0;

	}

	// public bool CanSlide()
	// {
	// 	if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
	// 		return true;
	// 	else
	// 		return false;
	// }

	private void WallSlide()
	{
		if (LastOnWallTime >= 0 && LastOnGroundTime < 0 && RB.velocity.y <= 0 && isUnlockWallJump)
		{
			_isWallSliding = true;
			Slide();
		}
		else
		{
			_isWallSliding = false;
			playerAnimationAndAudio.WallSlide(false);
		}
	}

	public void CheckGravity()
	{
		if (RB.velocity.y < 0)
		{
			playerAnimationAndAudio.Jump(true);
			playerAnimationAndAudio.Fall(true);
		}
		if (!_isDashAttacking)
		{
			//Higher gravity if we've released the jump input or are falling
			if (IsSliding)
			{
				SetGravityScale(0);
			}
			else if (RB.velocity.y < 0 && _moveInput.y < 0)
			{
				//Much higher gravity if holding down
				SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
				playerAnimationAndAudio.Fall(true);
			}
			else if (_isJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
				playerAnimationAndAudio.Fall(true);
			}
			else if ((_isJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
			{
				SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
			}
			else if (RB.velocity.y < 0)
			{
				playerAnimationAndAudio.Fall(true);
				//Higher gravity if falling
				SetGravityScale(Data.gravityScale * Data.fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(Data.gravityScale);
			}
		}
		else
		{
			//No gravity when dashing (returns to normal once initial dashAttack phase over)
			SetGravityScale(0);
		}
	}

	private void CheckInputAndParemeter()
	{
		LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;
		LastTimeShadowDash -= Time.deltaTime;

		#region INPUT HANDLER
		_moveInput.y = Input.GetAxisRaw("Vertical");
		if (isLie && Input.GetAxisRaw("Horizontal") != 0 && !isOpenInventory)
		{
			WakeUp();
		}
		else
		{
			_moveInput.x = Input.GetAxisRaw("Horizontal");
		}

		if (!isLie && !isOpenInventory && !isOpenMap)
		{
			canAttack = Input.GetKeyDown(KeyCode.A);
		}

		if (_moveInput.x != 0)
			CheckDirectionToFace(_moveInput.x > 0);

		if (Input.GetKeyDown(KeyCode.Space) && !isLie && !isOpenInventory)
		{
			OnJumpInput();
			isOpenMap = false;
		}
		else if (Input.GetKeyUp(KeyCode.Space) && isLie)
		{
			WakeUp();
		}

		if (Input.GetKeyUp(KeyCode.Space) && !isLie && !isOpenInventory)
		{
			OnJumpUpInput();
			isOpenMap = false;
		}
		else if (Input.GetKeyUp(KeyCode.Space) && isLie)
		{
			WakeUp();
		}

		if (Input.GetKeyDown(KeyCode.D) && !isLie && !isOpenInventory)
		{
			OnDashInput();
			isOpenMap = false;
		}
		else if (Input.GetKeyDown(KeyCode.D) && isLie)
		{
			WakeUp();
		}

		if (Input.GetKeyDown(KeyCode.Q) && !isLie && !isOpenInventory)
		{
			if (skillManager.equippedSkills[0] != null)
			{
				skillManager.ActivateSkill(0, gameObject);
				isOpenMap = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.W) && !isLie && !isOpenInventory)
		{
			if (skillManager.equippedSkills[1] != null)
			{
				skillManager.ActivateSkill(1, gameObject);
				isOpenMap = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.E) && !isLie && !isOpenInventory)
		{
			if (skillManager.equippedSkills[2] != null)
			{
				skillManager.ActivateSkill(2, gameObject);
				isOpenMap = false;
			}
		}
		if (!isLie)
		{
			isLook = _moveInput.y != 0;
			isOpenMap = Input.GetKey(KeyCode.M);
			isOpenInventory = Input.GetKey(KeyCode.Tab);

			if (isOpenInventory)
			{
				isOpenMap = false;
			}
		}
		#endregion
	}

	void UpdateCameraYDampForPlayerFall()
	{
		if (RB.velocity.y < fallSpeedThreshold && !CameraManager.Instance.isLerpingYDamping && !CameraManager.Instance.hasLerpedYDamping)
		{
			StartCoroutine(CameraManager.Instance.LerpYDamping(true));
		}

		if (RB.velocity.y >= 0 && CameraManager.Instance.isLerpingYDamping && CameraManager.Instance.hasLerpedYDamping)
		{
			CameraManager.Instance.hasLerpedYDamping = false;
			StartCoroutine(CameraManager.Instance.LerpYDamping(false));
		}
	}

	#endregion

	public void ImmuneDamage(float time)
	{
		StartCoroutine(StartImmuneDamage(time));
	}

	public void ImmuneDamage(bool isActive)
	{
		if (isActive)
		{
			gameObject.layer = 8;
		}
		else
		{
			gameObject.layer = 10;
		}
	}

	public void LostControl(float time)
	{
		StartCoroutine(StartLostControl(time));
	}

	IEnumerator StartLostControl(float time)
	{
		canControl = false;
		yield return new WaitForSeconds(time);
		canControl = true;
	}

	IEnumerator StartImmuneDamage(float time)
	{
		int temp = gameObject.layer;
		gameObject.layer = 8;
		yield return new WaitForSeconds(time);
		gameObject.layer = temp;
	}

	public void FreezeXPlayer(float time)
	{
		StartCoroutine(StartFreezeXPlayer(time));
	}

	IEnumerator StartFreezeXPlayer(float time)
	{
		RB.constraints = RigidbodyConstraints2D.FreezePositionX;
		if (LastOnGroundTime > 0)
		{
			// AnimHandler.justLanded = true
			isGround = true;
			airJumpCounter = 0;
			// animator.SetBool("isJumping", false);
		}

		LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
		yield return new WaitForSeconds(time);
		if (LastOnGroundTime > 0)
		{
			// AnimHandler.justLanded = true
			isGround = true;
			airJumpCounter = 0;
			// animator.SetBool("isJumping", false);
		}
		RB.constraints = RigidbodyConstraints2D.None;
		RB.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void FreezePlayer(float time)
	{
		StartCoroutine(StartFreeze(time));
	}

	IEnumerator StartFreeze(float time)
	{
		RB.constraints = RigidbodyConstraints2D.FreezeAll;
		if (LastOnGroundTime > 0)
		{
			isGround = true;
			airJumpCounter = 0;
		}

		LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
		yield return new WaitForSeconds(time);
		if (LastOnGroundTime > 0)
		{
			isGround = true;
			airJumpCounter = 0;
		}
		RB.constraints = RigidbodyConstraints2D.None;
		RB.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void Awaken(float time)
	{
		StartCoroutine(WaitForAwaken(time));
	}

	IEnumerator WaitForAwaken(float time)
	{
		canControl = false;
		RB.constraints = RigidbodyConstraints2D.FreezePosition;
		yield return new WaitForSeconds(time);
		canControl = true;
		RB.constraints = RigidbodyConstraints2D.None;
		RB.constraints = RigidbodyConstraints2D.FreezeRotation;
		SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
	}

	IEnumerator InvincibilityTimer(float time)
	{
		isInvincible = true;
		yield return new WaitForSeconds(time);
		isInvincible = false;
	}

	public IEnumerator WalkIntoNewScene(Vector2 exitDir, float delay)
	{
		playerAnimationAndAudio.Fall(false);
		playerAnimationAndAudio.Jump(false);
		playerAnimationAndAudio.WallSlide(false);
		canControl = false;
		RB.velocity = Vector2.zero;
		if (exitDir.y > 0)
		{
			RB.velocity = new Vector2(Data.runMaxSpeed * 1.5f * exitDir.x, Data.jumpForce * exitDir.y);
			SetGravityScale(Data.gravityScale);
			playerAnimationAndAudio.Jump(true);
		}

		if (exitDir.x != 0)
		{
			_moveInput.x = exitDir.x > 0 ? 1 : -1;
			Run(1);
			playerAnimationAndAudio.Run(true);
		}
		CheckDirectionToFace(_moveInput.x > 0);
		yield return new WaitForSeconds(delay);
		LastPressedJumpTime = 0;
		playerAnimationAndAudio.Run(false);
		LastOnGroundTime = -1;
		isCutScene = false;
		canControl = true;
	}

	IEnumerator Death()
	{
		GameManager.Instance.audioSource.Stop();
		isAlive = false;
		canControl = false;
		isOpenMap = false;
		isOpenInventory = false;
		Time.timeScale = 1f;
		RB.velocity = Vector2.zero;
		playerAnimationAndAudio.Death();
		playerAnimationAndAudio.Run(false);
		playerAnimationAndAudio.Jump(false);
		playerAnimationAndAudio.Fall(false);
		yield return new WaitForSeconds(1f);
		GameManager.Instance.RespawnPlayer();
	}

	void DeathEffect()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
	}

	void WakeUp()
	{
		StartCoroutine(StartWakeUp());
	}

	IEnumerator StartWakeUp()
	{
		canControl = false;
		playerAnimationAndAudio.WakeUp();
		playerAnimationAndAudio.Kneel(false);
		RB.constraints = RigidbodyConstraints2D.FreezeAll;
		isLie = false;
		if (!playerAnimationAndAudio.isLie())
		{
			yield return new WaitForSeconds(0.4f);
		}
		else
		{
			yield return new WaitForSeconds(1.6f);
		}
		RB.constraints = RigidbodyConstraints2D.None;
		RB.constraints = RigidbodyConstraints2D.FreezeRotation;
		LastOnGroundTime = Data.coyoteTime;
		canControl = true;
	}

	public void ResetInput()
	{
		LastPressedJumpTime = -1;
		LastPressedDashTime = -1;
		_moveInput = Vector2.zero;
	}

	void ToggleMap()
	{
		if (isOpenMap && LastOnGroundTime > 0 && !isOpenInventory && !isLook)
		{
			UIManager.Instance.OpenMap(true);
			playerAnimationAndAudio.OpenMap(true);
		}
		else
		{
			UIManager.Instance.OpenMap(false);
			playerAnimationAndAudio.OpenMap(false);
		}
	}

	void ToggleInventory()
	{
		if (isOpenInventory && LastOnGroundTime > 0 && !isOpenMap && !isLook)
		{
			UIManager.Instance.OpenInventory(true);
		}
		else
		{
			UIManager.Instance.OpenInventory(false);
		}
	}

	void ToggleLook()
	{
		if (isLook && LastOnGroundTime > 0 && !isOpenMap && !isOpenInventory && hp == maxHp && _moveInput.x == 0)
		{
			if (_moveInput.y > 0)
			{
				playerAnimationAndAudio.LookUp(true);
				playerAnimationAndAudio.LookDown(false);
			}
			else if (_moveInput.y < 0)
			{
				playerAnimationAndAudio.LookDown(true);
				playerAnimationAndAudio.LookUp(false);
			}
			else
			{
				playerAnimationAndAudio.LookUp(false);
				playerAnimationAndAudio.LookDown(false);
			}
		}
		else
		{
			playerAnimationAndAudio.LookUp(false);
			playerAnimationAndAudio.LookDown(false);
		}
	}

	// #region EDITOR METHODS
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(SideAttackTransform.position, sideAttackArea);
		Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
		Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
		Gizmos.DrawWireCube(backAttackTransform.position, sideAttackArea);
	}
}

