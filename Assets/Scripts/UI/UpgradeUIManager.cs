using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button openUpgradeButton;
    public Button closeButton;
    
    [System.Serializable]
    public class UpgradeButtonInfo
    {
        public Button button;
        public Text nameText;
        public Text costText;
    }

    public UpgradeButtonInfo attackUpgrade;
    public UpgradeButtonInfo hpUpgrade;
    public UpgradeButtonInfo spawnUpgrade;

    private UpgradeManager upgradeManager;

    private void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager not found on the same GameObject as UpgradeUIManager.");
            return;
        }

        SetupButton(openUpgradeButton, OpenUpgradePanel, "Open Upgrade Button");
        SetupButton(closeButton, CloseUpgradePanel, "Close Button");
        SetupButton(attackUpgrade.button, UpgradeAttack, "Attack Upgrade Button");
        SetupButton(hpUpgrade.button, UpgradeHP, "HP Upgrade Button");
        SetupButton(spawnUpgrade.button, UpgradeSpawn, "Spawn Upgrade Button");

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Upgrade Panel is not assigned in UpgradeUIManager.");
        }

        UpdateAllButtonTexts();
    }

    private void SetupButton(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogError($"{buttonName} is not assigned in UpgradeUIManager.");
        }
    }

    private void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            UpdateAllButtonTexts();
            Time.timeScale = 0f;
        }
    }

    private void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void UpgradeAttack()
    {
        if (upgradeManager.UpgradeAttack())
        {
            UpdateButtonText(attackUpgrade, "attack");
        }
    }

    private void UpgradeHP()
    {
        if (upgradeManager.UpgradeHp())
        {
            UpdateButtonText(hpUpgrade, "hp");
        }
    }

    private void UpgradeSpawn()
    {
        if (upgradeManager.UpgradeSpawn())
        {
            UpdateButtonText(spawnUpgrade, "spawn");
        }
    }

    private void UpdateButtonText(UpgradeButtonInfo buttonInfo, string upgradeName)
    {
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager is null in UpgradeUIManager.");
            return;
        }

        var (name, cost, level) = upgradeManager.GetUpgradeInfo(upgradeName);
        
        if (buttonInfo.nameText != null)
        {
            buttonInfo.nameText.text = $"{name} Lv.{level}";
        }
        else
        {
            Debug.LogError($"Name Text for {upgradeName} upgrade is not assigned in UpgradeUIManager.");
        }

        if (buttonInfo.costText != null)
        {
            buttonInfo.costText.text = $"비용: {cost}";
        }
        else
        {
            Debug.LogError($"Cost Text for {upgradeName} upgrade is not assigned in UpgradeUIManager.");
        }
    }

    private void UpdateAllButtonTexts()
    {
        UpdateButtonText(attackUpgrade, "attack");
        UpdateButtonText(hpUpgrade, "hp");
        UpdateButtonText(spawnUpgrade, "spawn");
    }
}
