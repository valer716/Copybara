using UnityEngine;

public class shot : MonoBehaviour
{
    /*
    public float speed = 8f;
    private GameObject player;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed*Time.deltaTime;
       
        player=GameObject.FindGameObjectWithTag("Player");

    }

   void Update()
    {
        transform.position+=Vector3.left*speed*Time.deltaTime;
    }*/

    public float speed=8f;
    private GameObject player;

    private Rigidbody2D rb;
    private float timer;

    private player_health hp;
    public int damage = 2;

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        player=GameObject.FindGameObjectWithTag("Player");


        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity=new Vector2(direction.x,direction.y).normalized*speed;

    }

    void Update()
    {
        timer+=Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

       

        if (collision.gameObject.tag=="Player")
        {
            if (hp == null) 
            {
                hp=collision.gameObject.GetComponent<player_health>();
            }
            Destroy(gameObject);
            hp.Damage(damage);
        }
    }

}
