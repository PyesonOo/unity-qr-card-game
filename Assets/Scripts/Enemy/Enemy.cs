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
    public EnemyAttack[] weakAttacks = new EnemyAttack[0];
    public EnemyAttack[] strongAttacks = new EnemyAttack[0];
    private bool hasEnteredRageMode = false;

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

    if (currentLife > 0)
    {
        CheckRageMode();
    }

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

    bool isInRageMode = CheckRageMode();
    EnemyAttack[] attackPool = isInRageMode ? strongAttacks : weakAttacks;

    if (attackPool == null || attackPool.Length == 0)
    {
        Debug.LogWarning(isInRageMode
            ? "Enemy hat keine starken Angriffe konfiguriert."
            : "Enemy hat keine schwachen Angriffe konfiguriert.");
    }
    else
    {
        EnemyAttack selectedAttack = attackPool[UnityEngine.Random.Range(0, attackPool.Length)];

        if (selectedAttack == null)
        {
            Debug.LogWarning("Enemy hat einen nicht konfigurierten Angriff ausgewählt.");
        }
        else
        {
            Player.Instance.TakeDamage(selectedAttack.damage);

            Debug.Log(
                "Enemy benutzt " + selectedAttack.attackName +
                " und verursacht " + selectedAttack.damage + " Schaden."
            );
        }
    }

    if (BattleManager.Instance != null && !BattleManager.Instance.IsBattleOver)
{
    BattleManager.Instance.StartPlayerTurn();
}
}

private bool CheckRageMode()
{
    bool isInRageMode = currentLife <= maxLife * 0.5f;
    if (isInRageMode && !hasEnteredRageMode)
    {
        hasEnteredRageMode = true;
        Debug.Log("Enemy enters RAGE MODE!");
    }

    return isInRageMode;
}

private bool CanAttack()
{
    return !isDead && currentLife > 0 &&
        BattleManager.Instance != null && !BattleManager.Instance.IsBattleOver &&
        !BattleManager.Instance.IsPlayerTurn() &&
        Player.Instance != null && Player.Instance.currentLife > 0;
}
}
