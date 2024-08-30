using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    public enum EffectType
    {
        AttackBoost,
        Heal,
    }

    public EffectType effectType;
    public float effectValue;
    public float effectDuration;

    public void ApplyEffect()
    {
        switch (effectType)
        {
            case EffectType.Heal:
                PlayerManager.Instance.HealAllPlayer(effectValue);
                break;
            case EffectType.AttackBoost:
                PlayerManager.Instance.AttackBoostAllPlayer(effectValue, effectDuration);
                break;
        }
    }

}
