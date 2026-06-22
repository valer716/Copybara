using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Collider2D Collider;

    private Boss_life hp;
    public int damage = 1;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Shooter" || other.gameObject.tag == "enemy_draft")
        {
            if (hp == null)
            {
                hp = other.gameObject.GetComponent<Boss_life>();
            }
            hp.Damage(damage);
            Destroy(gameObject);
        }

    }
}
