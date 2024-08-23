using System.Collections;
using UnityEngine;

public class AutoCoinPlus : MonoBehaviour
{
    public float generateInterval = 2f; // 증가 시간 간격
    public int coinCount = 1; // 증가 코인량
    
    void Start()
    {
        StartCoroutine(AutoAddCoin());
    }

    IEnumerator AutoAddCoin()
    {
        while (true)
        {
            yield return new WaitForSeconds(generateInterval);
            CoinManager.Instance.AddCoin(coinCount);
        }
    }

    public void IncreaseAutoCoin(int count)
    {
        coinCount += count;
    }
}
