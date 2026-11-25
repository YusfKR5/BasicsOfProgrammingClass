using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineCamera cinemachineCamera;
    public float defaultFov = 3.14f;
    public float runFov = 3.5f;
    public float zoonTime = 4f;

    private Coroutine zoomRoutine;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void ZoomTo(float target, float duration)
    {
        if (zoomRoutine != null) StopCoroutine(zoomRoutine); 
        zoomRoutine = StartCoroutine(ZoomRoutine(target, duration));
    }

    private IEnumerator ZoomRoutine(float target, float duration)
    {
        float start = cinemachineCamera.Lens.OrthographicSize; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);      

            duration += Time.deltaTime;

            float smooth = Mathf.SmoothStep(0f, 1f, t);        
            var lens = cinemachineCamera.Lens;
            lens.OrthographicSize = Mathf.Lerp(start, target, smooth);
            cinemachineCamera.Lens = lens;

            yield return null;
        }

        var final = cinemachineCamera.Lens;
        final.OrthographicSize = target;                
        cinemachineCamera.Lens = final;
        zoomRoutine = null;
    }
}
