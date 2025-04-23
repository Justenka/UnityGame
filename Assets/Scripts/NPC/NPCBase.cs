using UnityEngine;

public abstract class NPCBase : MonoBehaviour
{
    [SerializeField] protected GameObject npcUI;
    PlayerAudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager>();
    }
    public abstract void Interact();

    public virtual void OpenUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(true);
            Time.timeScale = 0f;
            UIManager.Instance.RegisterOpenMenu(npcUI);
            audioManager.PlaySound(audioManager.interact);
        }
    }

    public virtual void CloseUI()
    {
        if (npcUI != null)
        {
            npcUI.SetActive(false);
            Time.timeScale = 1f;
            UIManager.Instance.UnregisterMenu(npcUI);
            audioManager.PlaySound(audioManager.interact);
        }
    }
}
