using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShaderEffectControls : MonoBehaviour
{
    public static ShaderEffectControls instance;
    public Transform ShockWaveLocation;

    public GameObject background;
    public Coroutine ShockWaveCoroutine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
    }
    public IEnumerator Attack1ShaderEffect(float duration, float goal, float startingValue)
    {
        Disappear(PlayerCombat.instance.Attack1Data.dissolvingDuration, PlayerController.instance.gameObject, true);
        yield return StartCoroutine(SetShockWave(duration, goal, background, true));
        yield return new WaitForSeconds(PlayerCombat.instance.Attack1Data.intervalBetweenScreenWaves);
        Appear(PlayerCombat.instance.Attack1Data.appearingDuration, PlayerController.instance.gameObject, true);
        yield return StartCoroutine(SetShockWave(duration, startingValue, background, true));
    }
    public void Disappear(float duration, GameObject target, bool isPushing)
    {
        StartCoroutine(SetDissolve(duration, 1f, target, isPushing, true));
    }
    public void Appear(float duration, GameObject target, bool isPushing)
    {
        StartCoroutine(SetDissolve(duration, 0f, target, isPushing, false));
    }
    public IEnumerator SetShockWave(float duration, float goal, GameObject target, bool isPushing)
    {
        if (target.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            Vector2 ringCenter = WorldToSpriteUV(renderer, PlayerController.instance.transform.position);
            renderer.material.SetVector("_RingSpawnPosition", ringCenter);

            if (isPushing)
                yield return renderer.material.DOFloat(goal, "_WaveDistanceFromCenter", duration).SetEase(Ease.InOutQuart);
            else
                yield return renderer.material.DOFloat(goal, "_WaveDistanceFromCenter", duration).SetEase(Ease.OutQuart);
            yield return new WaitForSeconds(duration - 0.25f);
        }
    }
    public IEnumerator SetDissolve(float duration, float goal, GameObject target, bool isPushing, bool isDisolving)
    {
        if (isPushing)
            if (isDisolving)
                yield return new WaitForSeconds(0.25f);
            else
                yield return new WaitForSeconds(0.1f);

        if (target.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
            {
                yield return renderer.material.DOFloat(goal, "_DissolveAmount", duration).WaitForCompletion(true);
                if (renderer.material.GetFloat("_DissolveAmount") != goal)
                {
                    renderer.material.SetFloat("_DissolveAmount", goal);
                }
            }
    }
    private Vector2 WorldToSpriteUV(SpriteRenderer renderer, Vector3 worldPos)
    {
        Bounds bounds = renderer.bounds;
        float x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, worldPos.x);
        float y = Mathf.InverseLerp(bounds.min.y, bounds.max.y, worldPos.y);
        return new Vector2(x, y);
    }

}
