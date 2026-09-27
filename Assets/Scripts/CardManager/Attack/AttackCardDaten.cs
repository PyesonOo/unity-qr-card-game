using UnityEngine;
using System.Collections.Generic;

public class AttackCardDaten : MonoBehaviour
{
    public static AttackCardDaten Instance;

    private Dictionary<string, AttackCard> cards;

    private void Awake()
    {
        Instance = this;

        cards = new Dictionary<string, AttackCard>();

        AddCard("ATK2", "Kleiner Angriff", 2,1);
        AddCard("ATK5", "Mittlerer Angriff", 5,1);
        AddCard("ATK10", "Großer Angriff", 10,2);
    }

    private void AddCard(string id, string name, int attackValue, int manaCost)
    {
        AttackCard card = new AttackCard();

        card.id = id;
        card.name = name;
        card.attackValue = attackValue;
        card.manaCost = manaCost;

        cards.Add(id, card);
    }

   public AttackCard GetCard(string id)
{
    Debug.Log("Suche Karte: " + id);

    if (cards.ContainsKey(id))
    {
        Debug.Log("Karte gefunden!");
        return cards[id];
    }

    Debug.Log("Karte NICHT gefunden!");
    return null;
}
};