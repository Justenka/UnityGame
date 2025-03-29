using UnityEngine;

public class MerchantNPC : NPCBase
{
    public override void Interact()
    {
        Debug.Log("Merchant: Opening shop...");
        OpenUI();
    }
}
