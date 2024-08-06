using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveState : VMyState<MonsterState>
{
    public override MonsterState StateEnum => MonsterState.Move;
    private MonsterController _monsterController;
    private NavMeshAgent _navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        _monsterController = GetComponent<MonsterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void EnterState()
    {
        // Debug.Log("Move State 접근");
    }
    
    protected override void ExcuteState()
    {
    }

    protected override void ExcuteState_FixedUpdate()
    {
        // Debug.Log("Move State 실행");
        // (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(_monsterController.DestinationIndex);
        // _monsterController.DestinationIndex = destinationInfo.Item1;
        // if (_monsterController.MoveToDestination(destinationInfo.Item2))
        // {
        //     _monsterController.DestinationIndex++;
        // }
        
        // 목적지로 이동
        (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(_monsterController.DestinationIndex);
        _monsterController.DestinationIndex = destinationInfo.Item1;
        _navMeshAgent.SetDestination(destinationInfo.Item2);

        // 목적지에 도달했는지 확인하고, 도달 시 다음 목적지로 인덱스 증가
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
        {
            _monsterController.DestinationIndex++;
        }
    }

    protected override void ExcuteState_LateUpdate()
    {
    }

    protected override void ExitState()
    {
        // Debug.Log("Move State 종료");
    }
}
