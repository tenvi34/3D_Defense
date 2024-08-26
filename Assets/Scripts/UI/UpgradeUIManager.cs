using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button openUpgradeButton;
    public Button closeButton;

    // 텍스트 폰트 크기 설정
    public int nameFontSize = 14;
    public int costFontSize = 12;
    
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
    public UpgradeButtonInfo coinUpgrade;

    private UpgradeManager upgradeManager;

    private void OnEnable()
    {
        if (openUpgradeButton != null) openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
        if (closeButton != null) closeButton.onClick.AddListener(CloseUpgradePanel);
    }

    private void OnDisable()
    {
        if (openUpgradeButton != null) openUpgradeButton.onClick.RemoveListener(OpenUpgradePanel);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseUpgradePanel);
    }

    private void Start()
    {
        // 필수 컴포넌트 확인
        if (upgradePanel == null || openUpgradeButton == null || closeButton == null)
        {
            return;
        }

        upgradeManager = GetComponent<UpgradeManager>();
        if (upgradeManager == null)
        {
            return;
        }

        // 초기에 업그레이드 패널 비활성화
        upgradePanel.SetActive(false);

        // UI 초기화 코루틴 시작
        StartCoroutine(InitializeUI());
    }

    // UI 초기화 코루틴
    private IEnumerator InitializeUI()
    {
        yield return null; // 1프레임 대기

        SetupButton(attackUpgrade.button, UpgradeAttack, "Attack Upgrade Button");
        SetupButton(hpUpgrade.button, UpgradeHP, "HP Upgrade Button");
        SetupButton(spawnUpgrade.button, UpgradeSpawn, "Spawn Upgrade Button");
        SetupButton(coinUpgrade.button, UpgradeCoin, "Coin Upgrade Button");

        UpdateAllButtonTexts();
    }

    // 버튼 설정
    private void SetupButton(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    // 업그레이드 패널 열기
    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            UpdateAllButtonTexts();
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    // 업그레이드 패널 닫기
    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f; // 게임 재개
        }
    }

    // 공격력 업그레이드
    private void UpgradeAttack()
    {
        if (upgradeManager.UpgradeAttack())
        {
            UpdateButtonText(attackUpgrade, "attack");
        }
    }

    // 체력 업그레이드
    private void UpgradeHP()
    {
        if (upgradeManager.UpgradeHp())
        {
            UpdateButtonText(hpUpgrade, "hp");
        }
    }

    // 소환 수 업그레이드
    private void UpgradeSpawn()
    {
        if (upgradeManager.UpgradeSpawn())
        {
            UpdateButtonText(spawnUpgrade, "spawn");
        }
    }

    // 코인 획득량 업그레이드
    private void UpgradeCoin()
    {
        if (upgradeManager.UpgradeCoin())
        {
            UpdateButtonText(coinUpgrade, "coin");
        }
    }

    // 버튼 텍스트 업데이트
    private void UpdateButtonText(UpgradeButtonInfo buttonInfo, string upgradeName)
    {
        if (upgradeManager == null) return;

        var (name, cost, level) = upgradeManager.GetUpgradeInfo(upgradeName);
        
        if (buttonInfo.nameText != null)
        {
            buttonInfo.nameText.text = $"{name} Lv.{level}";
            buttonInfo.nameText.fontSize = nameFontSize;
        }

        if (buttonInfo.costText != null)
        {
            buttonInfo.costText.text = $"비용: {cost}";
            buttonInfo.costText.fontSize = costFontSize;
        }
    }

    private void UpdateAllButtonTexts()
    {
        UpdateButtonText(attackUpgrade, "attack");
        UpdateButtonText(hpUpgrade, "hp");
        UpdateButtonText(spawnUpgrade, "spawn");
        UpdateButtonText(coinUpgrade, "coin");
    }
}