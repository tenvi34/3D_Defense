using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradePanel; // 강화 창 패널 프리팹
    public GameObject upgradeButtonPrefab; // 강화 버튼 프리팹
    public Transform upgradeButtonContainer; // 강화 버튼들을 배치할 부모 Transform

    public Button openUpgradeButton; // OpenUpgradeButton

    private UpgradeManager upgradeManager;

    private void Start()
    {
        // UpgradeManager 컴포넌트 가져오기
        upgradeManager = GetComponent<UpgradeManager>();

        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager component not found on the same GameObject as UpgradeUIManager.");
            return;
        }

        if (openUpgradeButton != null)
        {
            // OpenUpgradeButton에 클릭 이벤트 추가
            openUpgradeButton.onClick.AddListener(OpenUpgradePanel);
        }
        else
        {
            Debug.LogError("OpenUpgradeButton is not assigned in the inspector.");
        }

        // 초기에는 강화 패널 비활성화
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("UpgradePanel is not assigned in the inspector.");
        }
    }

    // 강화 창 패널 열기
    private void OpenUpgradePanel()
    {
        if (upgradePanel == null || upgradeButtonContainer == null || upgradeManager == null)
        {
            Debug.LogError("Some required components are not assigned. Please check the inspector.");
            return;
        }

        upgradePanel.SetActive(true);
        Time.timeScale = 0f; // 게임 일시 정지

        // 기존 버튼들 제거
        foreach (Transform child in upgradeButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // 강화 버튼 생성
        CreateUpgradeButtons("공격력 강화", upgradeManager.attackUpgrade, upgradeManager.UpgradeAttack);
        CreateUpgradeButtons("체력 강화", upgradeManager.hpUpgrade, upgradeManager.UpgradeHp);
    }

    // 강화 버튼 생성 메서드
    private void CreateUpgradeButtons(string buttonPrefix, UpgradeManager.UpgradeOption[] upgradeOptions, System.Func<int, bool> upgradeAction)
    {
        if (upgradeOptions == null || upgradeButtonPrefab == null)
        {
            Debug.LogError($"UpgradeOptions or UpgradeButtonPrefab is null for {buttonPrefix}");
            return;
        }

        for (int i = 0; i < upgradeOptions.Length; i++)
        {
            UpgradeManager.UpgradeOption option = upgradeOptions[i];
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonTextComponent = buttonObj.GetComponentInChildren<Text>();

            if (buttonTextComponent == null)
            {
                Debug.LogError("Text component not found on the button prefab.");
                continue;
            }

            buttonTextComponent.text = $"{buttonPrefix} {i + 1}\n코스트: {option.cost}\n효과: +{option.improvement}";
            
            int index = i; // 클로저를 위해 로컬 변수로 캡처
            button.onClick.AddListener(() => {
                if (upgradeAction(index))
                {
                    // 강화 성공 시 버튼 텍스트 업데이트
                    buttonTextComponent.text = $"{buttonPrefix} {index + 1}\n강화 완료!";
                    button.interactable = false;
                }
            });
        }
    }

    // 강화 창 패널 닫기
    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f; // 게임 재개
        }
    }
}
