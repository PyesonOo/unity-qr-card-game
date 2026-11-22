using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int maxLife = 20;
    public int currentLife = 20;

    public int maxMana = 3;
    public int currentMana = 3;

    public int bonusManaPerTurn = 0;
    public int bonusAttackDamage = 0;

    public bool hasActiveShield = false;
    public int shieldWeakBlockPercent = 0;
    public int shieldStrongBlockPercent = 0;

    public Slider lifeBar;

    public TMP_Text currentCardText;

    public TMP_Text manaText;

 private void Awake()
{
    Instance = this;


}

private void Start()
{
    Debug.Log("1");
    lifeBar.maxValue = maxLife;

    Debug.Log("2");
    lifeBar.value = currentLife;

    Debug.Log("3");
    SetCurrentCard("TEST");

    UpdateManaText();

    Debug.Log("4");
}

public void UpdateManaText()
{
    if (manaText == null)
    {
        Debug.LogError("manaText ist NULL!");
        return;
    }

    manaText.text = "Mana: " + currentMana + " / " + maxMana;
}
public void SetCurrentCard(string cardName)
{
    Debug.Log("SetCurrentCard: " + cardName);

    if (currentCardText == null)
    {
        Debug.LogError("currentCardText ist NULL!");
        return;
    }

    currentCardText.text = "Aktuelle Karte: " + cardName;

    Debug.Log("Text erfolgreich gesetzt.");

    
}

public void TakeEnemyAttack(int damage, bool isStrongAttack)
{
    if (BattleManager.Instance != null && BattleManager.Instance.IsBattleOver)
        return;

    string attackType = isStrongAttack ? "Starker Angriff" : "Schwacher Angriff";

    if (!hasActiveShield)
    {
        Debug.Log(attackType + ". Ursprünglicher Schaden: " + damage +
            ". Blockwert: 0 %. Tatsächlich erhaltener Schaden: " + damage +
            ". Kein aktiver Schild; kein Schild verbraucht.");
        TakeDamage(damage);
        return;
    }

    int blockPercent = Mathf.Clamp(
        isStrongAttack ? shieldStrongBlockPercent : shieldWeakBlockPercent, 0, 100);
    int finalDamage = Mathf.FloorToInt(damage * (100 - blockPercent) / 100f);

    hasActiveShield = false;
    shieldWeakBlockPercent = 0;
    shieldStrongBlockPercent = 0;

    Debug.Log(attackType + ". Ursprünglicher Schaden: " + damage +
        ". Blockwert: " + blockPercent +
        " %. Tatsächlich erhaltener Schaden: " + finalDamage +
        ". Schild verbraucht; alle Schildwerte zurückgesetzt.");

    TakeDamage(finalDamage);
}

public void TakeDamage(int damage)
{
    if (BattleManager.Instance != null && BattleManager.Instance.IsBattleOver)
        return;

    currentLife -= damage;

    if (currentLife < 0)
    {
        currentLife = 0;
    }

    lifeBar.value = currentLife;

    if (currentLife == 0 && BattleManager.Instance != null)
        BattleManager.Instance.EndBattle(BattleManager.BattleState.Lost);

    Debug.Log(
        "Player bekommt " + damage +
        " Schaden. Leben: " + currentLife
    );
}
}
