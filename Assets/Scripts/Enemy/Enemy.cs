using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    public int maxLife = 50;
    public int currentLife = 50;
    public bool isDead = false;

    public int attackDamage = 5;

    public Slider lifeBar;
    public TMP_Text enemyNameText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentLife = maxLife;

        lifeBar.maxValue = maxLife;
        lifeBar.value = currentLife;

        enemyNameText.text = "Trainingspuppe";

        
    }

public void TakeDamage(int damage)
{
    if (BattleManager.Instance != null && BattleManager.Instance.IsBattleOver)
        return;

    if (isDead)
{
    Debug.Log("Enemy ist bereits besiegt.");
    return;
}
    currentLife -= damage;

    if (currentLife < 0)
    {
        currentLife = 0;
    }

    lifeBar.value = currentLife;

    if (currentLife == 0)
{
    isDead = true;
    if (BattleManager.Instance != null)
        BattleManager.Instance.EndBattle(BattleManager.BattleState.Won);
    Debug.Log("Enemy besiegt!");
}

    Debug.Log(
        "Enemy bekommt " + damage +
        " Schaden. Leben: " + currentLife
    );
}

public IEnumerator AttackPlayer()
{
    if (!CanAttack())
    {
        yield break;
    }

    yield return new WaitForSeconds(1f);

    if (!CanAttack())
    {
        yield break;
    }

    Player.Instance.TakeDamage(attackDamage);

    Debug.Log(
        "Enemy greift Player mit " +
        attackDamage +
        " Schaden an."
    );

    if (BattleManager.Instance != null && !BattleManager.Instance.IsBattleOver)
{
    BattleManager.Instance.StartPlayerTurn();
}
}

private bool CanAttack()
{
    return !isDead && currentLife > 0 &&
        BattleManager.Instance != null && !BattleManager.Instance.IsBattleOver &&
        !BattleManager.Instance.IsPlayerTurn() &&
        Player.Instance != null && Player.Instance.currentLife > 0;
}
}
