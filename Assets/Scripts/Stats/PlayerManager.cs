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
}
