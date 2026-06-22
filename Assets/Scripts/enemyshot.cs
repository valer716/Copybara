using UnityEngine;

public class enemyshot : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float shootInt = 2;

    private float timer;

    private GameObject player;

    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");

        Debug.Log(player.name);
    }
    void Update()
    {

        float dis=Vector2.Distance(transform.position,player.transform.position);

        Debug.Log(dis);

        if (dis < 9.5)
        {

            timer += Time.deltaTime;
            if (timer > shootInt)
            {
                timer = 0;
                Shoot();

            }

        }
    }
    void Shoot(){

        Instantiate(bullet,firePoint.position,Quaternion.identity);
    }
    

}
