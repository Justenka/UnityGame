using UnityEngine;

public class GameSystemsRoot : MonoBehaviour
{
    private static GameSystemsRoot instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist entire group
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates on reload
        }
    }
}