using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ActionCard", menuName = "Scriptable Objects/ActionCard")]
public class ActionCard : ScriptableObject
{
    public Sprite cardImage;
    public int cardID;
    public string cardName;
    public string cardType;
    public string cardDescription;
    public int staminaCost;
    public int damage;
    public int defense;
}
