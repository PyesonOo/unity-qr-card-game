using UnityEngine;
using System.Collections.Generic;

public class PowerCardDaten : MonoBehaviour
{
    public static PowerCardDaten Instance;

    private Dictionary<string, PowerCard> cards;

    private void Awake()
    {
        Instance = this;

        cards = new Dictionary<string, PowerCard>();

        AddCard("PWR_MANA2", "Mana Boost", 1, PowerType.ManaBoost, 2);
        AddCard("PWR_ATK1", "Attack Boost", 1, PowerType.AttackBoost, 1);
    }

    private void AddCard(string id, string name, int manaCost, PowerType powerType, int powerValue)
    {
        PowerCard card = new PowerCard();

        card.id = id;
        card.name = name;
        card.manaCost = manaCost;
        card.powerType = powerType;
        card.powerValue = powerValue;

        cards.Add(id, card);
    }

    public PowerCard GetCard(string id)
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
}
