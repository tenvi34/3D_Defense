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
        GameObject target = _monsterController.FindTarget();
        
        if (target != null)
        {
            // 감지 범위에 플레이어가 들어오면 플레이어로 이동
            Debug.Log("플레이어 발견");
            _navMeshAgent.SetDestination(target.transform.position);
            
            // 공격 범위 확인
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= _monsterController.attackRange) // 공격 범위에 들어오면
            {
                _monsterController._stateMachine.ChangeState(MonsterState.Attack); // 공격 상태로 전환
            }
        }
        else
        {
            // 감지 범위 밖일 때
            (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(_monsterController.DestinationIndex);
            _monsterController.DestinationIndex = destinationInfo.Item1;
            _navMeshAgent.SetDestination(destinationInfo.Item2);

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
            {
                _monsterController.DestinationIndex++;
            }
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
