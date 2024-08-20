using System;

[Serializable]
public class CharacterStats
{
    // 기본 스탯
    public float originAttackDamage = 10f; // 기본 공격력
    public float originMaxHp = 100f; // 기본 체력
    
    // 강화 수치
    private float attackDamageMultiplier  = 1f; // 공격력 강화 수치
    private float maxHpMultiplier = 1f; // 체력 강화 수치
    
    // 추후 최대 캐릭 소환 수, 코인 증가량 등 추가 예정 
    
    
    public float AttackDamage => originAttackDamage * attackDamageMultiplier;
    public float MaxHp => originMaxHp * maxHpMultiplier;

    // 공격력 강화
    public void UpgradeAttackDamage(float multiplier)
    {
        attackDamageMultiplier += multiplier;
    }

    // 체력 강화
    public void UpgradeMaxHp(float multiplier)
    {
        maxHpMultiplier += multiplier;
    }
}
