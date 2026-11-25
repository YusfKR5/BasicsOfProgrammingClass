using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private ActionCard card;

    [Header("CardProps")]
    public Image cardImage;
    public int cardID;
    public TMP_Text cardName;
    public TMP_Text cardType;
    public TMP_Text cardDescription;
    public TMP_Text staminaCost;
    public TMP_Text damage;
    public TMP_Text defense;
    public void InitializeCard()
    {
        cardImage.sprite = card.cardImage;
        cardID = card.cardID;
        cardName.text = card.cardName;
        cardType.text = card.cardType;
        cardDescription.text = card.cardDescription;
        staminaCost.text = card.staminaCost.ToString();
        damage.text = card.damage.ToString();
        defense.text = card.defense.ToString();
    }
    void Awake()
    {
        InitializeCard();
    }

    void Update()
    {
        
    }
}
