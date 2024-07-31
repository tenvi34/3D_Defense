using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1_Ready : VMyState<PhaseState>
{
    public override PhaseState StateEnum => PhaseState.Phase1_Ready;
    public override void EnterState()
    {
        StartCoroutine(GoToNextState());
    }

    IEnumerator GoToNextState()
    {
        yield return new WaitForSeconds(3.0f);
        OwnerStateMachine.ChangeState(PhaseState.Phase1_Running);
    }

    public override void ExcuteState()
    {
    }

    public override void ExitState()
    {
    }
}