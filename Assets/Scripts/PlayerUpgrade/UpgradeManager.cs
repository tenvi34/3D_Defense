using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public CoinManager coinManager;

    [Serializable]
    public class UpgradeOption // 강화 옵션
    {
        public string name; // 강화 이름
        public int baseCost; // 초기 강화 비용
        public float improvement; // 강화 수치
        public float costIncreaseRate = 1.5f; // 비용 증가 비율
        [HideInInspector] public int currentLevel = 0;
        [HideInInspector] public int currentCost;
        
        public void Initialize()
        {
            currentCost = baseCost;
        }

        public void Upgrade()
        {
            currentLevel++;
            currentCost = Mathf.RoundToInt(currentCost * costIncreaseRate);
        }
    }

    public UpgradeOption attackUpgrade;
    public UpgradeOption hpUpgrade;
    public UpgradeOption spawnUpgrade;

    private void Start()
    {
        attackUpgrade.Initialize();
        hpUpgrade.Initialize();
        spawnUpgrade.Initialize();
    }

    public bool UpgradeAttack()
    {
        if (coinManager.UseCoin(attackUpgrade.currentCost))
        {
            //playerStats.UpgradeAttackDamage(attackUpgrade.improvement);
            PlayerManager.Instance.UpgradeAllPlayersAttack(attackUpgrade.improvement);
            attackUpgrade.Upgrade();
            return true;
        }
        return false;
    }

    public bool UpgradeHp()
    {
        if (coinManager.UseCoin(hpUpgrade.currentCost))
        {
            //playerStats.UpgradeMaxHp(hpUpgrade.improvement);
            PlayerManager.Instance.UpgradeAllPlayersHp(hpUpgrade.improvement);
            hpUpgrade.Upgrade();
            return true;
        }
        return false;
    }

    public bool UpgradeSpawn()
    {
        if (coinManager.UseCoin(spawnUpgrade.currentCost))
        {
            PlayerEnemySpawnController.Instance.IncreaseMaxCharacterCount(1);
            spawnUpgrade.Upgrade();
            return true;
        }
        return false;
    }

    public (string name, int cost, int level) GetUpgradeInfo(string upgradeName)
    {
        switch (upgradeName)
        {
            case "attack":
                return (attackUpgrade.name, attackUpgrade.currentCost, attackUpgrade.currentLevel);
            case "hp":
                return (hpUpgrade.name, hpUpgrade.currentCost, hpUpgrade.currentLevel);
            case "spawn":
                return (spawnUpgrade.name, spawnUpgrade.currentCost, spawnUpgrade.currentLevel);
            default:
                return ("", 0, 0);
        }
    }
    
}










// public PlayerStats playerStats;
    //
    // [Serializable]
    // public class UpgradeOption
    // {
    //     public string name;
    //     public int cost;
    //     public float improvement;
    // }
    //
    // public UpgradeOption[] attackUpgrade; // 공격력 강화
    // public UpgradeOption[] hpUpgrade; // 최대 체력 강화
    // public UpgradeOption[] spawnUpgrade; // 플레이어 소환 수 강화
    //
    // // 공격력 강화
    // public bool UpgradeAttack(int upgradeIndex)
    // {
    //     if (upgradeIndex < 0 || upgradeIndex >= attackUpgrade.Length)
    //     {
    //         Debug.Log("잘못된 인덱스");
    //         return false;
    //     }
    //
    //     UpgradeOption upgradeOption = attackUpgrade[upgradeIndex];
    //     if (CoinManager.Instance.UseCoin(upgradeOption.cost))
    //     {
    //         playerStats.UpgradeAttackDamage(upgradeOption.improvement);
    //         Debug.Log($"{upgradeOption.name} 강화 성공");
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.Log("코인 부족.");
    //         return false;
    //     }
    // }
    //
    // // 최대 체력 강화
    // public bool UpgradeHp(int upgradeIndex)
    // {
    //     if (upgradeIndex < 0 || upgradeIndex >= hpUpgrade.Length)
    //     {
    //         Debug.LogError("잘못된 인덱스");
    //         return false;
    //     }
    //
    //     UpgradeOption upgradeOption = hpUpgrade[upgradeIndex];
    //     if (CoinManager.Instance.UseCoin(upgradeOption.cost))
    //     {
    //         playerStats.UpgradeMaxHp(upgradeOption.improvement);
    //         Debug.Log($"{upgradeOption.name} 강화 성공.");
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.Log("코인 부족.");
    //         return false;
    //     }
    // }
    //
    // // 플레이어 소환 수 강화
    // public bool UpgradeSpawn(int upgradeIndex)
    // {
    //     if (upgradeIndex < 0 || upgradeIndex >= spawnUpgrade.Length)
    //     {
    //         Debug.LogError("잘못된 인덱스");
    //         return false;
    //     }
    //
    //     UpgradeOption upgradeOption = spawnUpgrade[upgradeIndex];
    //     if (CoinManager.Instance.UseCoin(upgradeOption.cost))
    //     {
    //         PlayerEnemySpawnController.Instance.IncreaseMaxCharacterCount((int)upgradeOption.improvement);
    //         Debug.Log($"{upgradeOption.name} 강화 성공.");
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.Log("코인 부족.");
    //         return false;
    //     }
    // }