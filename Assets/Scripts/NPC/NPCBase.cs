using UnityEngine;

public abstract class NPCBase : MonoBehaviour
{
    [SerializeField] protected GameObject npcUI;

    public abstract void Interact();

    public virtual void OpenUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(true);
            Time.timeScale = 0f;
            UIManager.Instance.RegisterOpenMenu(npcUI);
        }
    }

    public virtual void CloseUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(false);
            Time.timeScale = 1f;
            UIManager.Instance.UnregisterMenu(npcUI);
        }
    }
}
