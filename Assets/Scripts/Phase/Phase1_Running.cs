using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Phase1_Running  : VMyState<PhaseState>
{
    public int SpawnMonsterCount = 10;
    public float Interval = 1.0f;
    public override PhaseState StateEnum => PhaseState.Phase1_Running;

    protected override void EnterState()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        for (int i = 0; i < SpawnMonsterCount; ++i)
        {
            Debug.Log((i + 1) + "번째 몬스터 소환");
            PlayerEnemySpawnController.Instance.GetNewEnemy(transform.position, Quaternion.LookRotation(Vector3.right));
            yield return new WaitForSeconds(Interval);
        }
    }

    protected override void ExcuteState()
    {
    }

    protected override void ExitState()
    {
        
    }

    public T GetEnumValue<T>() where T : Enum
    {
        throw new NotImplementedException();
    }
}
