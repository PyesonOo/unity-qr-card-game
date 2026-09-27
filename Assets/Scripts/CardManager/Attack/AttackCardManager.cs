using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AttackCardManager : MonoBehaviour
{
    public static AttackCardManager Instance;

    public string currentAttackCard;
    public int attackValue;

    public List<string> collectedCards = new List<string>();
    private string lastScannedCard = "";

    public TMP_Text attackCardText;
    public bool isDead = false;

    private void Awake()
    {
        Instance = this;
        Debug.Log("AttackCardManager gestartet");
    }

    public void ReceiveCard(string cardId)
    {
        if (BattleManager.Instance != null && BattleManager.Instance.IsBattleOver)
            return;

        //verhindert, dass im gegnerischen Zug Karten gespielt werden
        if (BattleManager.Instance != null &&
    !BattleManager.Instance.IsPlayerTurn())
{
    Debug.Log("Karte ignoriert: Gegner ist am Zug.");
    return;
}
        // Doppelte Scans verhindern
        if (cardId == lastScannedCard)
        {
            return;
        }

        // Nur Angriffskarten verarbeiten
        if (!cardId.StartsWith("ATK"))
        {
            Debug.Log("Keine Angriffskarte: " + cardId);
            return;
        }

        if (Enemy.Instance != null && Enemy.Instance.isDead)
{
    Debug.Log("Angriff ignoriert: Enemy ist bereits besiegt.");
    return;
}

        lastScannedCard = cardId;

        currentAttackCard = cardId;
        collectedCards.Add(cardId);

        Debug.Log("Gescannter Text: [" + cardId + "]");

        // Karte aus der Datenbank holen
        AttackCard card = AttackCardDaten.Instance.GetCard(cardId);

        if (card == null)
        {
            Debug.Log("Karte nicht in der Datenbank gefunden.");
            return;
        }

        Debug.Log("Gefundene Karte: " + card.name);

        attackValue = card.attackValue;

        //Wenn wir kein Mana mehr haben
        if (Player.Instance.currentMana < card.manaCost)
{
    Debug.Log("Nicht genug Mana!");
    return;
}
        //Mana abzug
       Player.Instance.currentMana -= card.manaCost;

    Player.Instance.UpdateManaText();
Debug.Log(
    "Mana verbraucht: " + card.manaCost +
    ". Verbleibendes Mana: " + Player.Instance.currentMana
); 

       if (Enemy.Instance != null)
{
    Enemy.Instance.TakeDamage(attackValue);
  
}
else
{
    Debug.LogError("Enemy.Instance ist NULL!");
}

        // Player informieren
        if (Player.Instance == null)
        {
            Debug.LogError("Player.Instance ist NULL!");
        }
        else
        {
            Debug.Log("Player gefunden.");
            Debug.Log("Rufe Player.SetCurrentCard auf...");

Player.Instance.SetCurrentCard(card.name);

Debug.Log("Player.SetCurrentCard wurde aufgerufen.");
        }

        attackCardText.text =
            "Karte: " + currentAttackCard +
            "\nAngriff: " + attackValue +
            "\nGesammelt: " + collectedCards.Count;

        Debug.Log("Angriffskarte gespeichert: " + cardId);

        // Erst Schaden und Kampfende auswerten, dann den Gegnerzug starten.
        if (Player.Instance.currentMana == 0 &&
            BattleManager.Instance != null && !BattleManager.Instance.IsBattleOver)
        {
            Debug.Log("Mana ist auf 0. Spielerzug wird beendet.");
            BattleManager.Instance.EndPlayerTurn();
        }
    }
    public void ResetLastScannedCard()
{
    lastScannedCard = "";
}

    public string GetCurrentCard()
    {
        return currentAttackCard;
    }
}
