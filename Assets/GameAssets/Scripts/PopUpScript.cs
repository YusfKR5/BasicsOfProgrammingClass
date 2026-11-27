using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpScript : MonoBehaviour
{
    private void Start()
    {
        
    }
    public IEnumerator PopupBehaviour(GameObject popup, float popupFadeSpeed)
    {
        float xMove = Random.Range(-0.35f, 0.35f);
        float yMove = Random.Range(0.3f, 0.7f);
        popup.transform.DOMove(transform.position + new Vector3(xMove, yMove, 0f), 0.5f);
        while (popup.GetComponentInChildren<TMP_Text>().color.a > 0)
        {
            float a = popup.GetComponentInChildren<TMP_Text>().color.a;
            a -= Time.deltaTime * popupFadeSpeed;
            float scale = popup.transform.localScale.x;
            scale -= Time.deltaTime;
            scale = Mathf.Clamp(scale, 0f, 1f);
            transform.localScale = new Vector3(scale, scale, 1);
            popup.GetComponentInChildren<TMP_Text>().color = new Color(popup.GetComponentInChildren<TMP_Text>().color.r, popup.GetComponentInChildren<TMP_Text>().color.g, popup.GetComponentInChildren<TMP_Text>().color.b, a);
            yield return null;
        }
        yield return null;
        Destroy(gameObject, 0.2f);
    }
}
