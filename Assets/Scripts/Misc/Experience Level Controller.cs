using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;
    private void Awake()
    {
        instance = this;
    }
    public int currentExperience;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetExp(int amountToGet)
    {
        currentExperience += amountToGet;
    }
}
