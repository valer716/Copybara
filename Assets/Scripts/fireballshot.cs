using UnityEngine;

public class fireballshot : MonoBehaviour
{

    public float speed = 25;
    private GameObject player;

    private Rigidbody2D rb;
    private float timer;

    private player_health hp;
    public int damage = 10;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed * Time.deltaTime;

    }

  void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if (hp == null)
            {
                hp = collision.gameObject.GetComponent<player_health>();
            }
            Destroy(gameObject);
            hp.Damage(damage);
        }
    }
}
