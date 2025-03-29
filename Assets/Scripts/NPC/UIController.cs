using UnityEngine;

public class UIController : MonoBehaviour
{
    private NPCBase activeNPC;

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
        }
    }
}
