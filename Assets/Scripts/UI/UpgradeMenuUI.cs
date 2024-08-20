using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public GameObject upgradePanelPrefab;  // Panel 프리팹
    public Button openUpgradeButton;
    public GameObject upgradeButtonPrefab;
    
    private GameObject upgradePanelInstance;  // 생성된 Panel 인스턴스
    private Transform attackUpgradeContainer;
    private Transform healthUpgradeContainer;
    private Transform spawnerUpgradeContainer;

    private List<Button> attackButtons = new List<Button>();
    private List<Button> healthButtons = new List<Button>();
    private List<Button> spawnerButtons = new List<Button>();

    private void Start()
    {
        openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
    }

    private void OpenUpgradePanel()
    {
        if (upgradePanelInstance == null)
        {
            CreateUpgradePanel();
        }
        Time.timeScale = 0; // 게임 일시정지
        upgradePanelInstance.SetActive(true);
    }

    private void CreateUpgradePanel()
    {
        if (upgradePanelPrefab == null)
        {
            Debug.LogError("upgradePanelPrefab is null");
            return;
        }

        upgradePanelInstance = Instantiate(upgradePanelPrefab, transform);
    
        // Panel 내부의 컨테이너 찾기
        attackUpgradeContainer = upgradePanelInstance.transform.Find("AttackUpgradeContainer");
        healthUpgradeContainer = upgradePanelInstance.transform.Find("HealthUpgradeContainer");
        spawnerUpgradeContainer = upgradePanelInstance.transform.Find("SpawnerUpgradeContainer");

        if (attackUpgradeContainer == null || healthUpgradeContainer == null || spawnerUpgradeContainer == null)
        {
            Debug.LogError("One or more containers are missing in the panel prefab");
            return;
        }

        if (upgradeManager == null)
        {
            Debug.LogError("upgradeManager is null");
            return;
        }

        // 버튼 생성
        CreateUpgradeButtons(attackUpgradeContainer, upgradeManager.attackUpgrade, attackButtons, upgradeManager.UpgradeAttack);
        CreateUpgradeButtons(healthUpgradeContainer, upgradeManager.hpUpgrade, healthButtons, upgradeManager.UpgradeHp);
        CreateUpgradeButtons(spawnerUpgradeContainer, upgradeManager.spawnUpgrade, spawnerButtons, upgradeManager.UpgradeSpawn);

        // Close 버튼 이벤트 연결
        Transform closeButtonTransform = upgradePanelInstance.transform.Find("CloseButton");
        if (closeButtonTransform == null)
        {
            Debug.LogError("CloseButton is missing in the panel prefab");
            return;
        }
        Button closeButton = closeButtonTransform.GetComponent<Button>();
        if (closeButton == null)
        {
            Debug.LogError("Button component is missing on CloseButton");
            return;
        }
        closeButton.onClick.AddListener(CloseUpgradePanel);

        upgradePanelInstance.SetActive(false);
    }

    private void CreateUpgradeButtons(Transform container, UpgradeManager.UpgradeOption[] options, List<Button> buttonList, System.Func<int, bool> upgradeFunction)
    {
        for (int i = 0; i < options.Length; i++)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, container);
            Button button = buttonObj.GetComponent<Button>();
            int index = i;
            button.onClick.AddListener(() => OnUpgradeButtonClicked(index, options[index], upgradeFunction, button));
            UpdateButtonText(button, options[i]);
            buttonList.Add(button);
        }
    }

    private void OnUpgradeButtonClicked(int index, UpgradeManager.UpgradeOption option, System.Func<int, bool> upgradeFunction, Button button)
    {
        if (upgradeFunction(index))
        {
            UpdateButtonText(button, option);
            // 성공적으로 강화됐을 때의 UI 업데이트 로직
        }
        else
        {
            // 강화 실패 시 UI 피드백 (예: "코인 부족" 메시지 표시)
        }
    }

    private void UpdateButtonText(Button button, UpgradeManager.UpgradeOption option)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = $"{option.name}\n비용: {option.cost}";
        }
    }

    public void CloseUpgradePanel()
    {
        Time.timeScale = 1; // 게임 재개
        upgradePanelInstance.SetActive(false);
    }
}
