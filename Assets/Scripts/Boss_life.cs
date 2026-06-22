using UnityEngine;

public class Boss_life : MonoBehaviour
{

    public int maxLife = 3;
    public int health;
    

    public GameObject enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxLife;
    }

    public void Damage(int dam)
    {
        health -= dam;

        if (health <= 0)
        {
            Destroy(enemy);
        }
    }
}
