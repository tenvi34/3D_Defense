using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

public class PlayerAttackState : VMyState<PlayerState>
{
    public override PlayerState StateEnum => PlayerState.Attack;
    private PlayerController _playerController;

    [SerializeField] private float attackCoolTime = 2.0f;
    private bool canAttack = false;
    private IAttack currentTarget;
    
    protected override void Awake()
    {
        base.Awake();
        _playerController = GetComponent<PlayerController>();
        canAttack = true;
    }

    protected override void EnterState()
    {
        // Debug.Log("Player Attack State 접근");
        canAttack = true;
        currentTarget = _playerController.CurrentTarget;
    }
    
    protected override void ExcuteState()
    {
        //Debug.Log("PlayerAttackState.ExcuteState 실행 중");
        if (currentTarget == null || !currentTarget.IsAlive())
        {
            Debug.Log("타겟이 없거나 죽음");
            _playerController.StateMachine.ChangeState(PlayerState.Move);
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, currentTarget.GetTransform().position);
        //Debug.Log($"적과의 거리: {distanceToEnemy}, 공격 범위: {_playerController.AttackRange}");

        if (distanceToEnemy <= _playerController.AttackRange)
        {
            if (canAttack)
            {
                //Debug.Log("공격 시도");
                PerformAttackSync().Forget();
            }
        }
        else
        {
            //Debug.Log("적을 향해 이동");
            Vector3 directionToTarget = (currentTarget.GetTransform().position - transform.position).normalized;
            _playerController.MoveInDirection(directionToTarget);
        }
    }

    protected override void ExcuteState_FixedUpdate()
    {
    }

    protected override void ExcuteState_LateUpdate()
    {
    }

    protected override void ExitState()
    {
        // Debug.Log("Player Attack State 종료");
        currentTarget = null;
    }

    public void SetTarget(IAttack target)
    {
        currentTarget = target;
    }

    private async UniTaskVoid PerformAttackSync()
    {
        canAttack = false;
        _playerController.PerformAttack(currentTarget);

        await UniTask.Delay(TimeSpan.FromSeconds(attackCoolTime));
        canAttack = true;
    }
}
