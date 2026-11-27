using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Animation")]
    public float defaultFadeTime = 0.1f;
    private string currentAnim;

    [Header("ReadyComponents")]
    public Animator animator;
    public Rigidbody2D rb;
    public AudioSource audioSource;

    [Header("StateMachine")]
    public EnemyIdleState idleState;
    public EnemyHitState hitState;
    public EnemyDeathState deathState;
    public EnemyAttackState attackState; //For attackState i'm creating a new instance for every state transitions. Otherwise it doesnt variate the animations. Will be fixed. 
    //attackState transition is held on the TurnManager script. Checking the turn order just after increasing the currentTurn.
    public EnemyTransitionState transitionState;
    private StateMachine<EnemyController> stateMachine;

    [Header("Prefabs")]
    public GameObject BloodVFX;
    public GameObject BloodParticle;

    [Header("Porperties")]
    public float health;
    public float maxHealth;
    public int phase = 1; // phase = -1 means phase 2, it will be setten -1 for the control enum of attack animation names.

    public float delayedHP; //For UI impact
    public float delayTime;
    public float targetHP;

    [Header("Cheacks")]
    public bool isDead;

    [Header("UI")]
    public GameObject hpBarParent;
    public Image healthBar;
    public Image delayedHealthBar;
    public TMP_Text healthText;
    public Color VictoryImageColor;
    public Image VictoryImage;
    public GameObject GameOverPanel;

    public TurnManager turnManager;

    public static EnemyController instance;

    public GameObject damagePopup;

    public Transform popupTransformLeftBottom;
    public Transform popupTransformRightTop;

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

        phase = 1;

        idleState = new EnemyIdleState();
        hitState = new EnemyHitState();
        deathState = new EnemyDeathState();
        attackState = new EnemyAttackState();
        transitionState = new EnemyTransitionState();
        stateMachine = new StateMachine<EnemyController>();

        health = maxHealth;
        delayedHP = health;
        targetHP = health;

        VictoryImage.color = new Vector4(VictoryImage.color.r, VictoryImage.color.g, VictoryImage.color.b, 0f);
        GameOverPanel.SetActive(false);
    }
    private void Start()
    {
        stateMachine.Initialize(this, idleState);
    }
    private void Update()
    {
        #region UpdateHPBar
        healthBar.fillAmount = (health / maxHealth);
        healthText.text = ((int)targetHP).ToString();
        #endregion
        #region UpdateDelayedHPBar
        delayedHealthBar.fillAmount = (delayedHP / maxHealth);
        #endregion
        stateMachine.Update(this);

        Debug.Log(stateMachine.currentState);
        Debug.Log(turnManager.currentTurn);
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
    }
    public void ReturnToIdleState()
    {
        ChangeState(idleState);
        //Called on animation event. (After hit anim.)
    }
    public void SetPhase2()
    {
        phase = -1;
    }
    public IEnumerator TakeDamage(float damage)
    {
        Debug.Log("PopUpManager: " + PopUpManager.instance);
        Debug.Log("DamagePopup: " + damagePopup);
        Debug.Log("LeftBottom: " + popupTransformLeftBottom);
        Debug.Log("RightTop: " + popupTransformRightTop);
        PopUpManager.instance.SpawnPopUP(damagePopup, popupTransformLeftBottom, popupTransformRightTop, damage);
        targetHP = health - damage;
        ChangeState(hitState);
        Mathf.Clamp(targetHP, 0, maxHealth);
        yield return DOTween.To(() => health, x => health = x, targetHP, 0.1f);
        yield return hpBarParent.transform.DOShakePosition(delayTime, 60f, 150, 100f).WaitForCompletion();
        yield return DOTween.To(() => delayedHP, x => delayedHP = x, targetHP, 0.1f).WaitForCompletion();
    }
    public void Damage(float damage)
    {
        StartCoroutine(TakeDamage(damage));
    }
    public void ChangeState(IState<EnemyController> newState)
    {
        stateMachine.ChangeState(this, newState);
    }
    public IEnumerator SpawnBloodVFX()
    {
        yield return new WaitForSeconds(PlayerCombat.instance.hitFreezeDuration);
        GameObject spawnedBlood = Instantiate(BloodVFX, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.25f, transform.position.z), Quaternion.Euler(0f, 0f, -30f));
        Destroy(spawnedBlood, 0.25f);
        GameObject spawnedParticle = Instantiate(BloodParticle, transform.position, Quaternion.Euler(0f, 0f, -55f));
    }
    public void PlayAnim(string stateName, float fade = -1f)
    {
        Debug.Log("PlayAnim: '" + stateName + "'");

        if (currentAnim == stateName) return;
        currentAnim = stateName;

        float f = fade < 0f ? defaultFadeTime : fade;
        if (f <= 0f) animator.Play(stateName, 0);
        else animator.CrossFadeInFixedTime(stateName, f, 0);
    }
    public bool CheckAttackState()
    {
        return stateMachine.currentState == attackState;
    }
    public IEnumerator DisplayYouWonScreen()
    {
        GameOverPanel.SetActive(true);
        VictoryImageColor = VictoryImage.color;
        while (VictoryImageColor.a < 1)
        {
            VictoryImageColor.a += Time.deltaTime;
            VictoryImage.color = VictoryImageColor;
            yield return null;
        }
        turnManager.isGameOver = true;
    }
}
