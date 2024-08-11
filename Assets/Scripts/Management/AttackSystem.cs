using Interface;
using UnityEngine;

namespace Management
{
    public class AttackSystem
    {
        public static void PerformAttack(IAttack attacker, IAttack target, float damage)
        {
            if (target != null && target.IsAlive())
            {
                target.TakeDamage(damage);
                Debug.Log($"{attacker}가 {target}에게 {damage} 데미지를 입혔습니다.");
            }
            else
            {
                Debug.Log($"공격 실패: 대상({target})이 없거나 이미 사망했습니다.");
            }
        }
    }
}