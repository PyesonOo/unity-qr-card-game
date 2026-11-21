public enum PowerType
{
    ManaBoost,
    AttackBoost
}

[System.Serializable]
public class PowerCard
{
    public string id;
    public string name;
    public int manaCost;
    public PowerType powerType;
    public int powerValue;
}
