using UnityEngine;

namespace Interface
{
    public interface IAttack
    {
        void TakeDamage(float damage);
        float GetHealth();
        bool IsAlive();
        Transform GetTransform();
        HpScript GetHpScript();
    }
}