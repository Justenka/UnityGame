public class StaminaBarStub : StaminaBar
{
    public float storedValue;

    public override void SetStamina(float stamina)
    {
        storedValue = stamina;
    }
}