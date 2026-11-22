using UnityEngine;

public class DefenseCardManager : MonoBehaviour
{
    public static DefenseCardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ReceiveDefenseCard(string cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId))
        {
            Debug.Log("Verteidigungskarte ignoriert: Leere Karten-ID.");
            return;
        }

        if (BattleManager.Instance == null)
        {
            Debug.LogError("Verteidigungskarte ignoriert: BattleManager fehlt.");
            return;
        }

        if (BattleManager.Instance.IsBattleOver)
        {
            Debug.Log("Verteidigungskarte ignoriert: Der Kampf ist beendet.");
            return;
        }

        if (!BattleManager.Instance.IsPlayerTurn())
        {
            Debug.Log("Verteidigungskarte ignoriert: Gegner ist am Zug.");
            return;
        }

        if (DefenseCardDaten.Instance == null || Player.Instance == null)
        {
            Debug.LogError("Verteidigungskarte ignoriert: Kartendatenbank oder Player fehlt.");
            return;
        }

        if (Player.Instance.hasActiveShield)
        {
            Debug.Log("Verteidigungskarte ignoriert: Ein Schild ist bereits aktiv. Kein Mana verbraucht.");
            return;
        }

        DefenseCard card = DefenseCardDaten.Instance.GetCard(cardId);
        if (card == null)
        {
            Debug.Log("Verteidigungskarte nicht in der Datenbank gefunden: " + cardId);
            return;
        }

        if (card.manaCost < 0)
        {
            Debug.LogError("Verteidigungskarte hat ungültige Mana-Kosten: " + card.name);
            return;
        }

        if (Player.Instance.currentMana < card.manaCost)
        {
            Debug.Log("Nicht genug Mana für Verteidigungskarte: " + card.name);
            return;
        }

        Player.Instance.currentMana -= card.manaCost;
        Player.Instance.hasActiveShield = true;
        Player.Instance.shieldWeakBlockPercent = card.weakBlockPercent;
        Player.Instance.shieldStrongBlockPercent = card.strongBlockPercent;

        Player.Instance.UpdateManaText();

        Debug.Log("Schild aktiviert: " + card.name +
            ". Blockierung schwacher Angriffe: " + card.weakBlockPercent +
            " %, starker Angriffe: " + card.strongBlockPercent +
            " %. Mana verbraucht: " + card.manaCost +
            ". Verbleibendes Mana: " + Player.Instance.currentMana);

        // Den Schild setzen, bevor der Gegnerzug beginnen kann.
        if (Player.Instance.currentMana == 0 && !BattleManager.Instance.IsBattleOver)
        {
            Debug.Log("Mana ist auf 0. Spielerzug wird beendet.");
            BattleManager.Instance.EndPlayerTurn();
        }
    }
}
