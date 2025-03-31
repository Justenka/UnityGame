using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    public virtual void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }
    public virtual void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }
}
