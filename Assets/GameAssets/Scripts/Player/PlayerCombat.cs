using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerCombat : Attacks
{
    [Header("Transforms")]
    public Transform playerRegularTransform;
    public Transform playerAttackTransform;

    public LayerMask enemyLayer;
    public float hitFreezeDuration;

    public static PlayerCombat instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }
    private void Update()
    {
        CardPanel.SetActive(AttackCoroutine == null && turnManager.currentTurn % 2 == 1 && !turnManager.isGameOver);
    }
    public void ChangeTimeSpeed(float goal)
    {
        AudioSource playerAudio = PlayerController.instance.audioSource;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, goal, 0.05f).WaitForCompletion();
        DOTween.To(() => playerAudio.pitch, x => playerAudio.pitch = x, goal, 0.05f).WaitForCompletion();
    }
    public void DamageEnemy(float damage)
    {
        RaycastHit2D hit;
        if (hit = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0, Vector2.right, 2f, enemyLayer))
        {
            if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(damage);
                ChangeTimeSpeed(0.45f);
            }
        }
        //Called on animation event.
    }
    public IEnumerator HitFreeze()
    {
        float previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitFreezeDuration);
        Time.timeScale = previousTimeScale;
        //Called on animation event.
    }
}
