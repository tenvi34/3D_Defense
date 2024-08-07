using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;
    public GameObject hpbarPrefab;
    private GameObject hpbarInstance;
    private Slider hpSlider;

    void Awake()
    {
        currentHp = maxHp;
        ShowHpBar();
    }

    void Update()
    {
        UpdateHpBarPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
            Debug.Log("체력 감소");
        }
    }

    // 체력바 표시
    void ShowHpBar()
    {
        hpbarInstance = Instantiate(hpbarPrefab, transform);
        hpbarInstance.transform.localPosition = new Vector3(0, 2.5f, 0);
        Canvas canvas = hpbarInstance.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        hpSlider = hpbarInstance.GetComponentInChildren<Slider>();
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }


    // 체력바 위치 업데이트
    void UpdateHpBarPosition()
    {
        if (hpbarInstance != null)
        {
            hpbarInstance.transform.position = transform.position + new Vector3(0, 2.5f, 0);
            hpbarInstance.transform.LookAt(Camera.main.transform);
            //hpbarInstance.transform.Rotate(0, 180, 0); 
        }
    }

    // 체력 업데이트
    void UpdateHp()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
        }
    }

    // 데미지 처리
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHp();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    // 캐릭터 사망 처리
    void Die()
    {
        Destroy(gameObject);
    }
}
