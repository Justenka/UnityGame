using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    public Text levelText;
    public Slider xpBar;

    private ExperienceLevelController expController;

    private void Start()
    {
        expController = ExperienceLevelController.instance;
        expController.OnLevelUp += UpdateUI;
        UpdateUI(expController.currentLevel);
    }

    private void Update()
    {
        xpBar.maxValue = expController.experienceToNextLevel;
        xpBar.value = expController.currentExperience;
    }

    private void UpdateUI(int newLevel)
    {
        levelText.text = "Level: " + newLevel;
    }
}
