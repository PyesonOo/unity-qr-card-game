using UnityEngine;
using System.Collections.Generic;

public class DefenseCardDaten : MonoBehaviour
{
    public static DefenseCardDaten Instance;

    private Dictionary<string, DefenseCard> cards;

    private void Awake()
    {
        Instance = this;

        cards = new Dictionary<string, DefenseCard>();

        AddCard("DEF_SHIELD", "Energieschild", 1, 100, 50);
    }

    private void AddCard(string id, string name, int manaCost, int weakBlockPercent, int strongBlockPercent)
    {
        DefenseCard card = new DefenseCard();

        card.id = id;
        card.name = name;
        card.manaCost = manaCost;
        card.weakBlockPercent = weakBlockPercent;
        card.strongBlockPercent = strongBlockPercent;

        cards.Add(id, card);
    }

    public DefenseCard GetCard(string id)
    {
        Debug.Log("Suche Karte: " + id);

        if (id != null && cards.ContainsKey(id))
        {
            Debug.Log("Karte gefunden!");
            return cards[id];
        }

        Debug.Log("Karte NICHT gefunden!");
        return null;
    }
}
