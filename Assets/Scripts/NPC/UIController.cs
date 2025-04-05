using UnityEngine;

public class UIController : MonoBehaviour
{
    private NPCBase activeNPC;

    [SerializeField] private InventoryToggle inventoryToggle;

    public void SetActiveNPC(NPCBase npc)
    {
        activeNPC = npc;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && activeNPC != null)
        {

            activeNPC.CloseUI();
            activeNPC = null;

            if (inventoryToggle != null)
                inventoryToggle.CloseInventory();
        }
    }
}
