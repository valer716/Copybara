using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private int std = 1;

    private int currD;

    private float halfWidth;

    public float speed = 2;

    private Vector2 move;

    private player_health hp;
    public int damage = 1;

    private void Start() {
        halfWidth = sr.bounds.extents.x;
        currD=std;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        move.x=speed*currD;
        move.y=rb.linearVelocity.y;

        rb.linearVelocity=move;
        SetDirect();
     
    }

    private void SetDirect()
    {
        if(Physics2D.Raycast(transform.position, Vector2.left, halfWidth+0.1f, LayerMask.GetMask("Area")) && rb.linearVelocity.x<0)
        {
            currD*=-1;
            
        }else if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Area")) && rb.linearVelocity.x > 0)
            {
                currD *= -1;

            }
            Debug.DrawRay(transform.position, Vector2.right * (halfWidth + 0.1f), Color.red);
           Debug.DrawRay(transform.position, Vector2.left * (halfWidth + 0.1f), Color.red);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (hp == null)
            {
                hp = collision.gameObject.GetComponent<player_health>();
            }
            hp.Damage(damage);
        }
    }

}
