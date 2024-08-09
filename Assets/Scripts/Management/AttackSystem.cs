using Interface;
using UnityEngine;

namespace Management
{
    public class AttackSystem
    {
        public static void PerformAttack(IAttack attacker, IAttack target, float damage)
        {
            if (target.IsAlive())
            {
                target.TakeDamage(damage);
                Debug.Log($"{attacker}가 {target}를 {damage} 피해 입힘");
            }
        }
    }
}