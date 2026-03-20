using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //ez az event dolog egy ilyen jelzés vmi más objectnek, hogy történt valami, az invoke-kal hívjuk meg, ez csak a deklaráció, és a másik objektumot fel kell majd iratkoztatni
    public static event Action CoinPickedUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("aélsdkfjaésldjkf");
        CoinPickedUp?.Invoke();
        Destroy(gameObject);
    }
}

