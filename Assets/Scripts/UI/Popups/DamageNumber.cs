using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float lifeTime = 2;
    private float lifeCounter;

    public float floatSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeCounter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeCounter > 0)
        {

            lifeCounter -= Time.deltaTime;
            if (lifeCounter <= 0)
            {
                //Destroy(gameObject);

                DamageNumberController.instance.PlaceInPool(this);
            }
        }

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
    public void Setup(int damageDisplay, bool isPlayerDamage)
    {
        lifeCounter = lifeTime;
        damageText.text = damageDisplay.ToString();
        damageText.color = isPlayerDamage ? Color.red : Color.white;
    }
}
