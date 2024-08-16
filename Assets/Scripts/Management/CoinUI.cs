using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinUI;
    
    void Start()
    {
        CoinDisplay();
    }

    void Update()
    {
        CoinDisplay();
    }

    private void CoinDisplay()
    {
        int currentCoin = CoinManager.Instance.TotalCoin;
        coinUI.text = $"{currentCoin}";
    }
}
