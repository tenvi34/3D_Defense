using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : VMyState<EnemyState>
{
    public override EnemyState StateEnum => EnemyState.Move;
    private EnemyController _enemyController;
    private NavMeshAgent _navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        _enemyController = GetComponent<EnemyController>();
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
        GameObject target = _enemyController.FindTarget();
        
        if (target != null)
        {
            // 감지 범위에 플레이어가 들어오면 플레이어로 이동
            // Debug.Log("플레이어 발견");
            _navMeshAgent.SetDestination(target.transform.position);
            
            // 공격 범위 확인
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= _enemyController.AttackRange) // 공격 범위에 들어오면
            {
                _enemyController._stateMachine.ChangeState(EnemyState.Attack); // 공격 상태로 전환
            }
        }
        else
        {
            // 감지 범위 밖일 때
            (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(_enemyController.DestinationIndex);
            _enemyController.DestinationIndex = destinationInfo.Item1;
            _navMeshAgent.SetDestination(destinationInfo.Item2);

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
            {
                _enemyController.DestinationIndex++;
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
