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
            }
        }
    }
}