public class ManaBarStub : ManaBar
{
    public float storedValue;

    public override void SetMana(float mana)
    {
        storedValue = mana;
    }
}