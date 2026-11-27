using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnPopUP(GameObject popup, Transform popupTransformLeftBottom, Transform popupTransformRightTop, float damage)
    {
        float x = Random.Range(popupTransformLeftBottom.position.x, popupTransformRightTop.position.x);
        float y = Random.Range(popupTransformLeftBottom.position.y, popupTransformRightTop.position.y);
        float z = Random.Range(popupTransformLeftBottom.position.z, popupTransformRightTop.position.z);
        Vector3 popupPosition = new Vector3(x, y, z);
        GameObject spawnedPopup = Instantiate(popup, popupPosition, Quaternion.identity);
        float scale = Random.Range(0.5f, 1f);
        spawnedPopup.transform.localScale = new Vector3 (scale, scale, 1);
        TMP_Text popupText = spawnedPopup.GetComponentInChildren<TMP_Text>();
        popupText.text = damage.ToString();
        PopUpScript popupScript = spawnedPopup.GetComponent<PopUpScript>();
        StartCoroutine(popupScript.PopupBehaviour(spawnedPopup, 0.1f));
    }
}

