[System.Serializable]
public class StatValue
{
    public float baseValue;
    public float bonusValue;
    public float currentValue;
    public float Total => baseValue + bonusValue;
}
