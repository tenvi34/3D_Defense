using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;
    public GameObject healthBarPrefab;
    private GameObject healthBarObject;
    private RectTransform foregroundBar;
    private Image foregroundImage;
    private RectTransform backgroundBar;
    
    public float GetCurrentHp() => currentHp;
    public bool IsAlive() => currentHp > 0;

    void Start()
    {
        currentHp = maxHp;
        CreateHealthBar();
    }

    void LateUpdate()
    {
        UpdateHealthBarPosition();
        
        // 테스트
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TakeDamage(10f);
        //     Debug.Log("체력 감소");
        // }
    }

    void CreateHealthBar()
    {
        healthBarObject = Instantiate(healthBarPrefab, transform);
        Canvas canvas = healthBarObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        RectTransform canvasRect = healthBarObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1f, 0.2f); // 체력바 크기 설정
        canvasRect.localPosition = Vector3.up * 2f; // 캐릭터 위에 표시

        backgroundBar = healthBarObject.transform.Find("Background").GetComponent<RectTransform>();
        foregroundBar = healthBarObject.transform.Find("Foreground").GetComponent<RectTransform>();
        foregroundImage = foregroundBar.GetComponent<Image>();

        // Background 설정
        backgroundBar.anchorMin = new Vector2(0, 0);
        backgroundBar.anchorMax = new Vector2(1, 1);
        backgroundBar.sizeDelta = Vector2.zero;
        backgroundBar.anchoredPosition = Vector2.zero;

        // Foreground 설정
        foregroundBar.anchorMin = new Vector2(0, 0);
        foregroundBar.anchorMax = new Vector2(1, 1);
        foregroundBar.sizeDelta = Vector2.zero;
        foregroundBar.anchoredPosition = Vector2.zero;

        // Foreground Image 설정
        foregroundImage.type = Image.Type.Filled;
        foregroundImage.fillMethod = Image.FillMethod.Horizontal;
        foregroundImage.fillOrigin = (int)Image.OriginHorizontal.Left;

        UpdateHealthBar();
    }

    void UpdateHealthBarPosition()
    {
        healthBarObject.transform.rotation = Quaternion.LookRotation(healthBarObject.transform.position - Camera.main.transform.position);
    }

    void UpdateHealthBar()
    {
        float healthPercent = currentHp / maxHp;
        foregroundImage.fillAmount = healthPercent;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(healthBarObject);
        Destroy(gameObject);
    }
}