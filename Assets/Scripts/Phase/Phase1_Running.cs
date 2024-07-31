using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1_Running  : VMyState<PhaseState>
{
    public int SpawnMonsterCount = 10;
    public float Interval = 1.0f;
    public override PhaseState StateEnum => PhaseState.Phase1_Running;

    public override void EnterState()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        for (int i = 0; i < SpawnMonsterCount; ++i)
        {
            MyPlayerController.Instance.GetNewMonster(transform.position, Quaternion.LookRotation(Vector3.back));
            yield return new WaitForSeconds(Interval);
        }
    }

    public override void ExcuteState()
    {
    }

    public override void ExitState()
    {
    }
}