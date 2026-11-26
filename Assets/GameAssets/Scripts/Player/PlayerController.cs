using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineCamera cinemachineCamera;
    public CameraController cameraController;
    public CinemachineImpulseSource impulseSource;

    [Header("Checks")]
    public bool facingRight = true;
    public bool isAttacking = false;
    public bool canAttack;
    public bool isDead;

    [Header("Animation")]
    public float defaultFadeTime = 0.1f;
    private string currentAnim;

    [Header("Transforms")]
    public Transform groundCheck;

    [Header("ReadyComponents")]
    public Animator animator;
    public Rigidbody2D rb;
    public AudioSource audioSource;

    [Header("StateMachine")]
    private StateMachine<PlayerController> stateMachine;
    public IdleState idleState;
    public Attack1State attack1State;
    public Attack2State attack2State;
    public Attack3State attack3State;

    [Header("SFX")]
    public AudioClip attack1SFX;

    [Header("Masks")]
    public LayerMask groundLayer;

    [Header("VFX")]
    public GameObject BloodVFX;
    public GameObject BloodParticle;

    [Header("Porps")]
    public float health;
    public float maxHealth;
    public float delayedHP;
    public float delayTime;
    public float targetHP;

    [Header("UI")]
    public GameObject hpBarParent;
    public Image healthBar;
    public Image delayedHealthBar;
    public TMP_Text healthText;
    public Color DeathImageColor;
    public Image DeathImage;
    public GameObject GameOverPanel;

    [HideInInspector] public static PlayerController instance { get; private set; }
    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        #endregion

        idleState = new IdleState();
        attack1State = new Attack1State();
        attack2State = new Attack2State();
        attack3State = new Attack3State();
        stateMachine = new StateMachine<PlayerController>();

        stateMachine.Initialize(this, idleState);

        health = maxHealth;
        targetHP = health;
        delayedHP = health;
    }
    private void Update()
    {
        stateMachine.Update(this);
        UpdateHealthBar();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }

    public IEnumerator SpawnBloodVFX()
    {
        yield return new WaitForSeconds(PlayerCombat.instance.hitFreezeDuration);
        GameObject spawnedBlood = Instantiate(BloodVFX, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.25f, transform.position.z), Quaternion.Euler(0f, 0f, -30f));
        Destroy(spawnedBlood, 0.25f);
        GameObject spawnedParticle = Instantiate(BloodParticle, transform.position, Quaternion.Euler(0f, 0f, -55f));
        //Called on animation event.
    }
    public IEnumerator DisplayYouLoseScreen()
    {
        GameOverPanel.SetActive(true);
        DeathImageColor = DeathImage.color;
        while (DeathImageColor.a < 1)
        {
            DeathImageColor.a += Time.deltaTime;
            DeathImage.color = DeathImageColor;
            yield return null;
        }
        TurnManager.instance.isGameOver = true;
    }
    public void UpdateHealthBar()
    {
        #region UpdateHPBar
        healthBar.fillAmount = (health / maxHealth);
        healthText.text = targetHP.ToString();
        #endregion
        #region UpdateDelayedHPBar
        delayedHealthBar.fillAmount = (delayedHP / maxHealth);
        #endregion
    }
    public IEnumerator TakeDamage(float damage)
    {
        targetHP = health - damage;
        if (targetHP <= 0f)
        {
            targetHP = 0f;
            Death();
        }
        else
        {
            PlayAnim("Hurt", 0.05f);
        }
            Mathf.Clamp(targetHP, 0, maxHealth);
        yield return DOTween.To(() => health, x => health = x, targetHP, 0.1f);
        yield return hpBarParent.transform.DOShakePosition(delayTime, 60f, 150, 100f).WaitForCompletion();
        yield return DOTween.To(() => delayedHP, x => delayedHP = x, targetHP, 0.1f).WaitForCompletion();
    }
    public void ReturnToIdle()
    {
        PlayAnim("Idle", 0.05f);
    }
    public void Death()
    {
        isDead = true;
        PlayAnim("Death", 0.1f);
        StartCoroutine(DisplayYouLoseScreen());
    }
    public void PlayAnim(string stateName, float fade = -1f)
    {
        if (currentAnim == stateName) return;
        currentAnim = stateName;

        float f = fade < 0f ? defaultFadeTime : fade;
        if (f <= 0f) animator.Play(stateName, 0);
        else animator.CrossFadeInFixedTime(stateName, f, 0);
    }
    public void ChangeState(IState<PlayerController> newState)
    {
        stateMachine.ChangeState(this, newState);
    }
    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    public void SetPlayerAbleToAttack()
    {
        canAttack = true;
        //Calling on animation event.
    }
    public void CancelAttack()
    {
        isAttacking = false;
        canAttack = true;
        //Calling on animation event.
    }
}