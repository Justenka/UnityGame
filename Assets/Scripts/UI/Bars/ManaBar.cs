using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaSlider;

    public void SetMaxMana(float mana)
    {
        manaSlider.maxValue = mana;
        manaSlider.value = mana;
    }
    public void SetMana(float mana)
    {
        manaSlider.value = mana;
    }
}
