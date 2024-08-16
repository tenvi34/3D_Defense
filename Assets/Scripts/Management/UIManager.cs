using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiPrefab;
    private Canvas uiCanvas;
    private Text coinText;

    void Start()
    {
        GameObject uiInstance = Instantiate(uiPrefab);
        uiCanvas = uiInstance.GetComponent<Canvas>();
        coinText = uiCanvas.transform.Find("CoinText").GetComponent<Text>();
        
        // CoinManager와 연결
        // CoinManager.Instance.OnCoinChanged += UpdateCoinDisplay;
    }

    void UpdateCoinDisplay(int coins)
    {
        coinText.text = $"Coins: {coins}";
    }
}
