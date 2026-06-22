using UnityEngine;
using UnityEngine.UI;

public class player_health : MonoBehaviour
{
    public int maxLife = 20;
    public int health;
    public Slider slider;

    void Start()
    {
        health = maxLife;
        slider.maxValue = maxLife;
        slider.value = health;
    }
    // Update is called once per frame
    public void Damage(int dam)
    {
        health -= dam;

        slider.value = health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
