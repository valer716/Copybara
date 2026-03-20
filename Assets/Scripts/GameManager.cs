using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coins = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Coin.CoinPickedUp += HandleCoinPickedUp;
    }

    void OnDisable()
    {
        Coin.CoinPickedUp -= HandleCoinPickedUp;
    }

    void HandleCoinPickedUp()
    {
        coins++;
    }
}
