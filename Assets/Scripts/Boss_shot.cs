using UnityEngine;

public class Boss_shot : MonoBehaviour
{
    public GameObject bull;
    public Transform FirePoint;
    public float shootInt = 2;

    private float timer;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log(player.name);
    }
    void Update()
    {

        

            timer += Time.deltaTime;
            if (timer > shootInt)
            {
                timer = 0;
                Shoot();

            }

        
    }
    void Shoot()
    {

        Instantiate(bull, FirePoint.position, Quaternion.identity);
    }

}
