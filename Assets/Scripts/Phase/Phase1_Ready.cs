using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Phase1_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase1_Ready;

    protected override void EnterState()
    {
        StartCoroutine(GoToNextState());
    }

    IEnumerator GoToNextState()
    {
        yield return new WaitForSeconds(3.0f);
        OwnerStateMachine.ChangeState(PhaseState.Phase1_Running);
    }

    protected  override void ExcuteState()
    {
    }

    protected  override void ExitState()
    {
    }
}