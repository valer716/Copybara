using UnityEngine;

public class move_stand : MonoBehaviour
{
    private player_health hp;
    public int damage = 1;
    
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
