using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public enum BattleState { Running, Won, Lost }
    public BattleState State { get; private set; } = BattleState.Running;
    public bool IsBattleOver => State != BattleState.Running;

    public bool playerTurn = true;
    public UnityEngine.UI.Button endTurnButton;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsPlayerTurn()
    {
        return !IsBattleOver && playerTurn;
    }

    public void EndBattle(BattleState result)
    {
        if (IsBattleOver || result == BattleState.Running)
            return;

        State = result;
        playerTurn = false;
        if (endTurnButton != null)
            endTurnButton.interactable = false;

        Debug.Log(result == BattleState.Won ? "Sieg!" : "Niederlage!");
    }

    //Spielzug endet
    public void EndPlayerTurn()
    {
        if (IsBattleOver || !playerTurn)
        {
            return;
        }

        Debug.Log("Spielerzug beendet.");

        playerTurn = false;

        if (endTurnButton != null)
{
    endTurnButton.interactable = false;
}

        if (Enemy.Instance != null && !Enemy.Instance.isDead)
        {
            Enemy.Instance.StartCoroutine(
                Enemy.Instance.AttackPlayer()
            );
        }
    }

    
    //Spielzug endet mit gespeicherten Mana
public void StartPlayerTurn()
{
    if (IsBattleOver || playerTurn)
        return;

    playerTurn = true; //Spieler ist am Zug

    if (endTurnButton != null)
{
    endTurnButton.interactable = true;
}

    if (Player.Instance != null)
    {
        Player.Instance.currentMana += Player.Instance.maxMana;
        Player.Instance.UpdateManaText(); // Wenn der Spielzug wieder anfängt, wird das gespeicherte Mana addiert

        Debug.Log(
            "Spielerzug beginnt. Mana: " +
            Player.Instance.currentMana
        );
    }
}
}
