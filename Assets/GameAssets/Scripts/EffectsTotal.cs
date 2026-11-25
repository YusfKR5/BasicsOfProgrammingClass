using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EffectsTotal : MonoBehaviour
{
    public static EffectsTotal instance;
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
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
    }
    public IEnumerator AttackEffect(AttackData data)
    {
        PlayerController.instance.PlayAnim("AttackCharge", 0.25f);
        PostProcessEffects.instance.Attack1PostProcess();
        yield return new WaitForSeconds(data.intervalBetweenPostProcessNShader);
        yield return StartCoroutine(ShaderEffectControls.instance.Attack1ShaderEffect(data.shockWaveDuration, -0.4f, 1.7f));
        yield return new WaitForSeconds(data.intervalBeforeResetingPostProcess);
    }
    public void AttackReturnEffect(AttackData data)
    {
        GameObject clone = new GameObject();
        clone.name = "clone";
        clone.transform.parent = PlayerCombat.instance.transform;
        clone.transform.localEulerAngles = Vector3.zero;
        clone.transform.localScale = Vector3.one;
        clone.transform.localRotation = Quaternion.identity;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.parent = null;
        clone.AddComponent<SpriteRenderer>();
        clone.GetComponent<SpriteRenderer>().material = PlayerController.instance.GetComponent<SpriteRenderer>().material;
        clone.GetComponent<SpriteRenderer>().sprite = PlayerController.instance.GetComponent<SpriteRenderer>().sprite;

        StartCoroutine(DissolveClone(data.cloneDissolvingDuration, clone));

        PostProcessEffects.instance.SetAllToZero(0.3f);
    }
    public IEnumerator DissolveClone(float duration, GameObject clone)
    {
        yield return StartCoroutine(ShaderEffectControls.instance.SetDissolve(duration, 1f, clone, false, false));
        Destroy(clone, 0.1f);
    }
}
