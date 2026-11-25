using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PostProcessEffects : MonoBehaviour
{
    public Volume postProcessVolume;
    public VolumeProfile volumeProfile;
    public ChromaticAberration chromaticAberration;
    public Vignette vignette;
    public LensDistortion lensDistortion;
    public float abberationChangeSpeedFactor = 1f;
    public float vignetteChangeSpeedFactor = 1f;
    public float lensDistortionChangeSpeedFactor = 1f;

    public Coroutine AbberationCoroutine;
    public Coroutine VignetteCoroutine;
    public Coroutine LensDistortionCoroutine;

    public static PostProcessEffects instance;
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

            volumeProfile = postProcessVolume.profile;

        volumeProfile.TryGet(out chromaticAberration);
        volumeProfile.TryGet(out vignette);
        volumeProfile.TryGet(out lensDistortion);
    }
    private void Start()
    {
    }
    public void Attack1PostProcess()
    {
        AbberationCoroutine = null;
        VignetteCoroutine = null;
        LensDistortionCoroutine = null;

        SetLensDistortion(0.4f, 0.8f);
        SetChoromaticAberration(0.55f, 0.85f);
        SetVignette(0.4f, 0.9f);
    }
    public void SetAllToZero(float duration)
    {
        AbberationCoroutine = null;
        VignetteCoroutine = null;
        LensDistortionCoroutine = null;

        SetLensDistortion(0f, duration);
        SetChoromaticAberration(0f, duration);
        SetVignette(0f, duration);
    }
    public void SetChoromaticAberration(float goalIntensity, float duration)
    {
        volumeProfile.TryGet(out chromaticAberration);
        AbberationCoroutine = StartCoroutine(SetIntensity(chromaticAberration.intensity, goalIntensity, duration, AbberationCoroutine));
    }
    public void SetVignette(float goalIntensity, float duration)
    {
        volumeProfile.TryGet(out vignette);
        VignetteCoroutine = StartCoroutine(SetIntensity(vignette.intensity, goalIntensity, duration, VignetteCoroutine));
    }
    public void SetLensDistortion(float goalIntensity, float duration)
    {
        volumeProfile.TryGet(out lensDistortion);
        LensDistortionCoroutine = StartCoroutine(SetIntensity(lensDistortion.intensity, goalIntensity, duration, LensDistortionCoroutine));
    }
    public IEnumerator SetIntensity(VolumeParameter<float> parameter, float goalIntensity, float duration, Coroutine currentCoroutine)
    {
        parameter.overrideState = true;

        yield return DOTween.To(() => parameter.value, x => parameter.value = x, goalIntensity, duration).WaitForCompletion();
    }
}
