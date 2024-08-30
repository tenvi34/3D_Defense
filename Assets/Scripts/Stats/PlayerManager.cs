using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private List<PlayerStats> playerStatsList = new List<PlayerStats>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayer(PlayerStats playerStats)
    {
        if (!playerStatsList.Contains(playerStats))
        {
            playerStatsList.Add(playerStats);
        }
    }

    public void RemovePlayer(PlayerStats playerStats)
    {
        playerStatsList.Remove(playerStats);
    }

    public void UpgradeAllPlayersAttack(float multiplier)
    {
        foreach (var playerStats in playerStatsList)
        {
            playerStats.UpgradeAttackDamage(multiplier);
        }
    }

    public void UpgradeAllPlayersHp(float multiplier)
    {
        foreach (var playerStats in playerStatsList)
        {
            playerStats.UpgradeMaxHp(multiplier);
        }
    }

    public void HealAllPlayer(float effectValue)
    {
        foreach (var playerStats in playerStatsList)
        {
            playerStats.Heal(effectValue);
        }
    }

    public void AttackBoostAllPlayer(float effectValue, float effectDuration)
    {
        StartCoroutine(AttackBoostCoroutine(effectValue, effectDuration));
    }

    private IEnumerator AttackBoostCoroutine(float effectValue, float effectDuration)
    {
        UpgradeAllPlayersAttack(effectValue); // 강화
        yield return new WaitForSeconds(effectDuration); // 정해진 지속시간동안만
        UpgradeAllPlayersAttack(-effectValue); // 강화 효과 제거
    }
}
