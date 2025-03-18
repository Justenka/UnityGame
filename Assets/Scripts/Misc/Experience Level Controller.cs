using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int currentExperience = 0;
    public int currentLevel = 1;
    public int experienceToNextLevel = 100;
    public float levelMultiplier = 1.2f;

    public delegate void OnLevelUpDelegate(int newLevel);
    public event OnLevelUpDelegate OnLevelUp;

    public void GetExp(int amountToGet)
    {
        currentExperience += amountToGet;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            currentLevel++;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * levelMultiplier);

            OnLevelUp?.Invoke(currentLevel);
        }
    }
}
