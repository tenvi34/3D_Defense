using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerStats playerStats;

    [Serializable]
    public class UpgradeOption
    {
        public string name;
        public int cost;
        public float improvement;
    }

    public UpgradeOption[] attackUpgrade; // 공격력 강화
    public UpgradeOption[] hpUpgrade; // 최대 체력 강화
    public UpgradeOption[] spawnUpgrade; // 플레이어 소환 수 강화

    // 공격력 강화
    public bool UpgradeAttack(int upgradeIndex)
    {
        if (upgradeIndex < 0 || upgradeIndex >= attackUpgrade.Length)
        {
            Debug.Log("잘못된 인덱스");
            return false;
        }

        UpgradeOption upgradeOption = attackUpgrade[upgradeIndex];
        if (CoinManager.Instance.UseCoin(upgradeOption.cost))
        {
            playerStats.UpgradeAttackDamage(upgradeOption.improvement);
            Debug.Log($"{upgradeOption.name} 강화 성공");
            return true;
        }
        else
        {
            Debug.Log("코인 부족.");
            return false;
        }
    }

    // 최대 체력 강화
    public bool UpgradeHp(int upgradeIndex)
    {
        if (upgradeIndex < 0 || upgradeIndex >= hpUpgrade.Length)
        {
            Debug.LogError("잘못된 인덱스");
            return false;
        }

        UpgradeOption upgradeOption = hpUpgrade[upgradeIndex];
        if (CoinManager.Instance.UseCoin(upgradeOption.cost))
        {
            playerStats.UpgradeMaxHp(upgradeOption.improvement);
            Debug.Log($"{upgradeOption.name} 강화 성공.");
            return true;
        }
        else
        {
            Debug.Log("코인 부족.");
            return false;
        }
    }

    // 플레이어 소환 수 강화
    public bool UpgradeSpawn(int upgradeIndex)
    {
        if (upgradeIndex < 0 || upgradeIndex >= spawnUpgrade.Length)
        {
            Debug.LogError("잘못된 인덱스");
            return false;
        }

        UpgradeOption upgradeOption = spawnUpgrade[upgradeIndex];
        if (CoinManager.Instance.UseCoin(upgradeOption.cost))
        {
            PlayerEnemySpawnController.Instance.IncreaseMaxCharacterCount((int)upgradeOption.improvement);
            Debug.Log($"{upgradeOption.name} 강화 성공.");
            return true;
        }
        else
        {
            Debug.Log("코인 부족.");
            return false;
        }
    }
}