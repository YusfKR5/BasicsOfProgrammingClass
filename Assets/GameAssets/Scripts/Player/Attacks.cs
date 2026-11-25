using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Attacks : MonoBehaviour
{
    public AttackData Attack1Data;
    public AttackData Attack2Data;

    public GameObject TeleportVFX;

    public Transform AttackVFXTransform;

    public Coroutine AttackCoroutine;

    public GameObject CardPanel;

    [SerializeField]private float teleportDelay;

    public TurnManager turnManager;

    public virtual IEnumerator Attack1()
    {
        PlayerController.instance.isAttacking = true;

        PlayerController.instance.audioSource.PlayOneShot(PlayerController.instance.attack1SFX);

        StartCoroutine(EffectsTotal.instance.AttackEffect(Attack1Data));
        yield return new WaitForSeconds(Attack1Data.teleportTriggerTime - teleportDelay);
        SpawnVFX(TeleportVFX, transform);
        PlayerController.instance.impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(teleportDelay);
        gameObject.transform.position = new Vector3(PlayerCombat.instance.playerAttackTransform.position.x, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(Attack1Data.attackTriggerTime-Attack1Data.teleportTriggerTime);
        PlayerController.instance.ChangeState(PlayerController.instance.attack1State);
        yield return null;
    }
    public virtual IEnumerator Attack2()
    {
        PlayerController.instance.isAttacking = true;

        PlayerController.instance.audioSource.PlayOneShot(PlayerController.instance.attack1SFX);

        StartCoroutine(EffectsTotal.instance.AttackEffect(Attack2Data));
        yield return new WaitForSeconds(Attack2Data.teleportTriggerTime - teleportDelay);
        SpawnVFX(TeleportVFX, transform);
        PlayerController.instance.impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(teleportDelay);
        gameObject.transform.position = new Vector3(PlayerCombat.instance.playerAttackTransform.position.x, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(Attack2Data.attackTriggerTime - Attack2Data.teleportTriggerTime);
        PlayerController.instance.ChangeState(PlayerController.instance.attack2State);
        yield return null;
    }
    public virtual IEnumerator AttackReturn(AttackData data)
    {
        EffectsTotal.instance.AttackReturnEffect(data);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        SpawnVFX(TeleportVFX, transform);
        transform.position = new Vector3(PlayerCombat.instance.playerRegularTransform.position.x, transform.position.y, transform.position.z);

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", 1f);
        ShaderEffectControls.instance.Appear(Attack1Data.appearingDuration, gameObject, false);

        yield return new WaitForSeconds(0.4f);

        AttackCoroutine = null;

        PlayerController.instance.isAttacking = false;

        turnManager.currentTurn++;
    }
    public void StartAttack1()
    {
        if (AttackCoroutine == null)
            AttackCoroutine = StartCoroutine(Attack1());
    }
    public void StartAttack2()
    {
        if (AttackCoroutine == null)
            AttackCoroutine = StartCoroutine(Attack2());
    }
    public void SpawnAttackVFX(GameObject vfx)
    {
        GameObject SpawnedVFX = Instantiate(vfx, AttackVFXTransform.position, Quaternion.identity);
        Destroy(SpawnedVFX, 0.5f);
    }
    public void SpawnVFX(GameObject vfx, Transform spawnPoint)
    {
        GameObject SpawnedVFX = Instantiate(vfx, spawnPoint.position, Quaternion.identity);
        Destroy(SpawnedVFX, 0.5f);
    }
}
