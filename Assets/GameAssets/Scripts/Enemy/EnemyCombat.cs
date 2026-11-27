using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public bool shouldStop = false;
    public Rigidbody2D rb;
    public Transform AttackingTransform;
    public float speed;
    public bool isAttacking;
    public Transform OriginalPosition;

    public GameObject AttackVFX;
    public Transform AttackVFXTransform;

    public static EnemyCombat instance;

    public AttackData currentAttackData;

    public List<AttackData> attackDatas;
    public List<AttackData> attackDatasPhase2;

    public bool isTransitionGoingOn = false;
    public enum Attacks
    {
        Attack2Phase2 = -2,
        Attack1Phase2 = -1,
        Attack1 = 1,
        Attack2 = 2
    }
    public Attacks attacksEnum;

    private void Awake()
    {
        #region Sigleton
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        #endregion
    }
    public void AttackPlayer(string animationName, int attackDataIndex)
    {
        StartCoroutine(Run(animationName, attackDataIndex));
    }
    public IEnumerator Attack(string animationName, int attackDataIndex)
    {
        isAttacking = true;
        if (EnemyController.instance.phase == 1)
            currentAttackData = EnemyCombat.instance.attackDatas[attackDataIndex];
        else
            currentAttackData = EnemyCombat.instance.attackDatasPhase2[attackDataIndex];
        EnemyController.instance.PlayAnim(animationName, 0.05f);
        yield return new WaitUntil(() => !isAttacking);
        yield return StartCoroutine(Return());
    }
    public IEnumerator Run(string animationName, int attackDataIndex)
    {
        yield return new WaitUntil(() => !isTransitionGoingOn);
        if (EnemyController.instance.phase == 1)
            EnemyController.instance.PlayAnim("Run", 0.1f);
        else
            EnemyController.instance.PlayAnim("RunPhase2", 0.1f);
        while (!shouldStop)
            {
                rb.MovePosition(new Vector2(transform.position.x - speed, transform.position.y));
                yield return null;
            }
        yield return new WaitUntil(() => shouldStop);
        yield return StartCoroutine(Attack(animationName, attackDataIndex));
    }
    public IEnumerator Return()
    {
        if (EnemyController.instance.phase == 1)
            EnemyController.instance.PlayAnim("Idle", 0.1f);
        else
            EnemyController.instance.PlayAnim("IdlePhase2", 0.1f);

        ShaderEffectControls.instance.Disappear(0.4f, gameObject, false);
        yield return new WaitForSeconds(0.6f);
        transform.position = OriginalPosition.position;
        ShaderEffectControls.instance.Appear(0.5f, gameObject, false);
        yield return new WaitForSeconds(0.2f);
        TurnManager.instance.IncreaseTurnByOne();
        EnemyController.instance.ChangeState(EnemyController.instance.idleState);
    }
    public void SetIsTransitionStart()
    {
        isTransitionGoingOn = true;
        //Called on animation event.
    }
    public void SetIsTransitionEnd()
    {
        isTransitionGoingOn = false;
        //Called on animation event.
    }
    public IEnumerator HealUp()
    {
        while (EnemyController.instance.health < 100)
        {
            EnemyController.instance.health += Time.deltaTime * 100f;
            EnemyController.instance.delayedHP += Time.deltaTime * 100f;
            EnemyController.instance.targetHP += Time.deltaTime * 100f;
            yield return null;
        }
        EnemyController.instance.health = 100;
        EnemyController.instance.delayedHP = 100;
        EnemyController.instance.targetHP = 100;
        //Called on animation event.
    }
    public void HurtPlayer()
    {
        PlayerController.instance.Hurt();
        //Called on animation event.
    }
    public void SetIsAttackingFalse()
    {
        isAttacking = false;
        //Called on animation event.
    }
    public void SpawnAttackVFX()
    {
        GameObject SpawnedVFX = Instantiate(AttackVFX, AttackVFXTransform.position, Quaternion.identity);
        Destroy(SpawnedVFX, 0.5f);
        //Called on animation event.
    }
    public IEnumerator HitFreeze()
    {
        float previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(PlayerCombat.instance.hitFreezeDuration);
        Time.timeScale = previousTimeScale;
        //Called on animation event.
    }
    public void StartAttack(string animationName, int attackDataIndex)
    {
        StartCoroutine(Run(animationName, attackDataIndex));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stop"))
        {
            shouldStop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stop"))
        {
            shouldStop = false;
        }
    }
}
