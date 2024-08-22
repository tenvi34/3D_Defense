using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button openUpgradeButton;
    public Button closeUpgradeButton;
    
    public Button attackUpgradeButton;
    public Button hpUpgradeButton;
    public Button spawnUpgradeButton;

    private UpgradeManager upgradeManager;

    private void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager not found in the scene.");
            return;
        }

        openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
        closeUpgradeButton.onClick.AddListener(CloseUpgradePanel);
        
        attackUpgradeButton.onClick.AddListener(UpgradeAttack);
        hpUpgradeButton.onClick.AddListener(UpgradeHP);
        spawnUpgradeButton.onClick.AddListener(UpgradeSpawn);

        upgradePanel.SetActive(false);
    }

    private void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0f; // 게임 일시 정지
    }

    private void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
    }

    private void UpgradeAttack()
    {
        if (upgradeManager.UpgradeAttack()) 
        {
            UpdateButtonText(attackUpgradeButton, "attack");
        }
    }

    private void UpgradeHP()
    {
        if (upgradeManager.UpgradeHp()) 
        {
            UpdateButtonText(hpUpgradeButton, "hp");
        }
    }

    private void UpgradeSpawn()
    {
        if (upgradeManager.UpgradeSpawn()) 
        {
            UpdateButtonText(spawnUpgradeButton, "spawn");
        }
    }

    private void UpdateButtonText(Button button, string upgradeName)
    {
        var (name, cost, level) = upgradeManager.GetUpgradeInfo(upgradeName);
        Text nameText = button.transform.Find("name").GetComponent<Text>();
        Text costText = button.transform.Find("cost").GetComponent<Text>();
        
        if (nameText != null && costText != null)
        {
            nameText.text = $"{name} Lv.{level}";
            costText.text = $"비용: {cost}";
        }
    }
    
    private void UpdateAllButtonTexts()
    {
        UpdateButtonText(attackUpgradeButton, "attack");
        UpdateButtonText(hpUpgradeButton, "hp");
        UpdateButtonText(spawnUpgradeButton, "spawn");
    }
}
