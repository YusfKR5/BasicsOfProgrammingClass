using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

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
    }
    private void Update()
    {
        stateMachine.Update(this);
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate(this);
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