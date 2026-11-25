using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public string Name;
    public string Animation;

    [Header("Attack")]
    public float damage;

    [Header("Movement")]
    public float chargeDuration;
    public float fadeOutDuration;
    public float fadeInDuration;
    public float teleportDistance;

    [Header("Timings")]
    public float intervalBetweenScreenWaves;
    public float intervalBetweenPostProcessNShader;
    public float intervalBeforeResetingPostProcess;
    public float dissolvingDuration;
    public float appearingDuration;
    public float cloneDissolvingDuration;
    public float attackTriggerTime;
    public float teleportTriggerTime;
    public float shockWaveDuration;
}
