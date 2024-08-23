using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : DD_Singleton<CoinManager>
{
    private int _totalCoin = 9999;

    public int TotalCoin
    {
        get { return _totalCoin;}
        private set { _totalCoin = value; }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // 코인 추가
    public void AddCoin(int coinNum)
    {
        TotalCoin += coinNum;
        Debug.Log($"코인 {coinNum}개 추가. 현재 보유 코인: {TotalCoin}");
    }
    
    // 코인 사용
    public bool UseCoin(int coinNum)
    {
        if (TotalCoin >= coinNum)
        {
            TotalCoin -= coinNum;
            Debug.Log($"코인 {coinNum}개 사용. 현재 보유 코인: {TotalCoin}");
            return true;
        }
        Debug.Log($"코인 부족. 현재 보유 코인: {TotalCoin}, 필요 코인: {coinNum}");
        return false;
    }
    
    // public bool HasEnoughCoins(int coinNum)
    // {
    //     return TotalCoin >= coinNum;
    // }
    
}
