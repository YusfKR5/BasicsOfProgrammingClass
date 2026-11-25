using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;

public class HoverEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Transform originalTransform;
    public float scalingFactor = 1.05f;

    public GameObject descriptionPanel;
    public GameObject OutLineCard;
    public GameObject OutLine;
    public Color outlineColor;

    public Vector3 HoverPosition;

    public float HoverMoveDuration;

    public bool isHover;

    public Coroutine alphaCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        originalTransform = transform;
        if(descriptionPanel != null)
            HideDescription();
        CreateOutline();
    }
    public void CreateOutline()
    {
        OutLineCard = new GameObject("OutlineCard");
        OutLine = new GameObject("Outline");
        OutLine.transform.SetParent(transform.Find("CardButton"));
        OutLine.transform.localPosition = Vector3.zero;
        OutLine.transform.localScale = new Vector3(1.02f, 1f, 1f);
        OutLineCard.AddComponent<Image>();
        OutLineCard.GetComponent<Image>().sprite = null;
        OutLineCard.transform.SetParent(OutLine.transform);
        OutLineCard.GetComponent<RectTransform>().sizeDelta = GetComponentInChildren<RectTransform>().sizeDelta;
        OutLineCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        OutLine.transform.SetParent(transform);
        OutLine.transform.SetAsFirstSibling();
        OutLineCard.GetComponent<RectTransform>().localScale = Vector3.one * 1.025f;
        OutLineCard.GetComponent<Image>().color = outlineColor;
        OutLine.SetActive(false);
    }
    public void DisplayDescription()
    {
        descriptionPanel.SetActive(true);
    }
    public IEnumerator SetOutlineAlpha(float goal, float duration)
    {
        yield return DOTween.To(() => outlineColor.a, x => outlineColor.a = x, goal, duration).SetEase(Ease.InOutExpo).WaitForCompletion();
        alphaCoroutine = null;
    }
    public IEnumerator DisplayOutline()
    {
        outlineColor.a = 1f;
        OutLine.SetActive(true);
        while (isHover)
        {
            yield return alphaCoroutine = StartCoroutine(SetOutlineAlpha(0f, 0.8f));
            yield return alphaCoroutine = StartCoroutine(SetOutlineAlpha(1f, 0.8f));
        }
    }
    public void HideOutline()
    {
        OutLine.SetActive(false);
        alphaCoroutine = null;
    }
    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
    public IEnumerator MovementAtHoverOn()
    {
        yield return DOTween.To(() => transform.rotation, x => transform.rotation = x, new Vector3(0f, 0f, 3f), 0.17f).WaitForCompletion();
    }

    public IEnumerator MovementAtHoverOff()
    {
        yield return DOTween.To(() => transform.rotation, x => transform.rotation = x, new Vector3(0f, 0f, 0f), 0.17f).WaitForCompletion();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        transform.localScale = originalScale * scalingFactor;
        if (descriptionPanel != null)
            DisplayDescription();
        StartCoroutine(DisplayOutline());
        StartCoroutine(MovementAtHoverOn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        transform.localScale = originalScale;
        if (descriptionPanel != null)
            HideDescription();
        HideOutline();
        StartCoroutine(MovementAtHoverOff());
    }
    private void FixedUpdate()
    {
        OutLineCard.GetComponent<Image>().color = new Vector4(OutLineCard.GetComponent<Image>().color.r, OutLineCard.GetComponent<Image>().color.g, OutLineCard.GetComponent<Image>().color.b, outlineColor.a);
    }
}
