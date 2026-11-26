using DG.Tweening;
using System.Collections;
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
    public void AttackPlayer()
    {
        StartCoroutine(Run());
    }
    public IEnumerator Run()
    {
        #region Pushing and Attacking Phase
        EnemyController.instance.PlayAnim("Run", 0.1f);
        while (!shouldStop)
        {
            rb.MovePosition(new Vector2(transform.position.x - speed, transform.position.y));
            yield return null;
        }
        yield return new WaitUntil(() => shouldStop == true);
        EnemyController.instance.PlayAnim("Attack", 0.05f);
        yield return new WaitUntil(() => isAttacking == false);
        #endregion

        #region Returning Phase
        ShaderEffectControls.instance.Disappear(0.4f, gameObject, false);
        yield return new WaitForSeconds(0.6f);
        transform.position = OriginalPosition.position;
        ShaderEffectControls.instance.Appear(0.5f, gameObject, false);
        yield return new WaitForSeconds(0.15f);
        TurnManager.instance.IncreaseTurnByOne();
        #endregion
    }
    public void HurtPlayer(float damage)
    {
        StartCoroutine(PlayerController.instance.TakeDamage(damage));
        //Called on animation event.
    }
    public void ReturnToIdle()
    {
        EnemyController.instance.PlayAnim("Idle", 0.1f);
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
    public void StartAttack()
    {
        if (TurnManager.instance.currentTurn % 2 == 0 && !isAttacking && !TurnManager.instance.isGameOver)
        {
            AttackPlayer();
            isAttacking = true;
        }
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
