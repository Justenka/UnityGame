using UnityEngine;

public class WizardNPC : NPCBase
{
    public GameObject storageInventory;
    public override void Interact()
    {
        OpenUI();
    }
}