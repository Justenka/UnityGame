using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    // Reference to your player's currency variable
    public Player player; // Assume this has an integer or float 'currency' property

    // Reference to the TextMeshPro text component in the scene
    public TMP_Text currencyText;

    void Update()
    {
        // Update the UI text with the current currency
        currencyText.text = player.currencyHeld.ToString();
    }
}