using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public CharacterStats stats = new CharacterStats();
    private HpScript _hpScript;

    private void Awake()
    {
        _hpScript = GetComponent<HpScript>();
        UpdateMaxHp();
    }

    // 최대 체력 설정
    private void UpdateMaxHp()
    {
        _hpScript.SetMaxHealth(stats.MaxHp);
    }

    // 공격력 강화
    public void UpgradeAttackDamage(float multiplier)
    {
        stats.UpgradeAttackDamage(multiplier);
        Debug.Log($"공격력 {multiplier * 100}% 증가. 현재 공격력: {stats.AttackDamage}");
    }
    
    // 최대 체력 강화
    public void UpgradeMaxHp(float multiplier)
    {
        stats.UpgradeMaxHp(multiplier);
        Debug.Log($"최대 체력 {multiplier * 100}% 증가. 현재 최대 체력: {stats.MaxHp}");
    }
}
