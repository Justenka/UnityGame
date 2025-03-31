public class HealthBarStub : HealthBar
{
    public float storedValue;

    public override void SetHealth(float health)
    {
        storedValue = health;
    }
}