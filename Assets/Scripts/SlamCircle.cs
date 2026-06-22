using UnityEngine;

public class SlamCircle : MonoBehaviour
{

    public float timer = 0.5f;
    void Start()
    {
        // Schedule destruction once at spawn time.
        Destroy(gameObject, timer);
    }
}
