using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : VMyState<MonsterState>
{
    public override MonsterState StateEnum => MonsterState.Move;
    private MonsterController _monsterController;

    void Awake()
    {
        base.Awake();
        _monsterController = GetComponent<MonsterController>();
    }

    protected override void EnterState()
    {
        //Debug.Log("Move State 접근");
    }
    
    protected override void ExcuteState()
    {
    }

    protected override void ExcuteState_FixedUpdate()
    {
        (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(_monsterController.DestinationIndex);
        _monsterController.DestinationIndex = destinationInfo.Item1;
        if (_monsterController.MoveToDestination(destinationInfo.Item2))
        {
            _monsterController.DestinationIndex++;
        }
    }

    protected override void ExcuteState_LateUpdate()
    {
    }

    protected override void ExitState()
    {
    }
}
