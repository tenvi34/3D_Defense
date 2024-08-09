using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyAttackState : VMyState<EnemyState>
{
    public override EnemyState StateEnum => EnemyState.Attack;
    private EnemyController _enemyController;
    
    [SerializeField] private float attackCoolTime = 2.0f;
    private bool canAttack = true;
    // private GameObject currentTarget;
    private IAttack currentTarget;

    protected override void Awake()
    {
        base.Awake();
        _enemyController = GetComponent<EnemyController>();
    }
    
    protected override void EnterState()
    {
        //Debug.Log("공격 상태 진입");
        canAttack = true;
        currentTarget = _enemyController.FindTarget().GetComponent<IAttack>();
    }

    protected override void ExcuteState_FixedUpdate()
    {
        if (currentTarget == null || !currentTarget.IsAlive())
        {
            currentTarget = _enemyController.FindTarget()?.GetComponent<IAttack>();
            if (currentTarget == null)
            {
                _enemyController._stateMachine.ChangeState(EnemyState.Move);
                return;
            }
        }
        
        float distance = Vector3.Distance(transform.position, currentTarget.GetTransform().position);
        
        if (distance <= _enemyController.attackRange)
        {
            // 공격 범위 안에 있으면 공격
            if (canAttack)
            {
                // StartCoroutine(Attack(currentTarget));
                PerformAttackAsync().Forget();
            }
        }
        else if (distance <= _enemyController.detectionRange)
        {
            // 감지 범위 안에 있지만 공격 범위 밖이면 플레이어를 향해 이동
            Vector3 directionToTarget = (currentTarget.GetTransform().position - transform.position).normalized;
            _enemyController.MoveInDirection(directionToTarget);
        }
        else
        {
            // 감지 범위를 벗어나면 다시 Move 상태
            _enemyController._stateMachine.ChangeState(EnemyState.Move);
        }
    }

    private IEnumerator Attack(GameObject target)
    {
        Debug.Log(target + "에게 공격 시도");
        canAttack = false;

        yield return new WaitForSeconds(attackCoolTime);
        canAttack = true;
    }
    
    private async UniTaskVoid PerformAttackAsync()
    {
        canAttack = false;
        _enemyController.PerformAttack(currentTarget);

        await UniTask.Delay(TimeSpan.FromSeconds(attackCoolTime));
        canAttack = true;
    }

    protected override void ExitState()
    {
        // Debug.Log("공격 상태 종료");
        // StopAllCoroutines();
        currentTarget = null;
    }
    
    public T GetEnumValue<T>() where T : System.Enum
    {
        throw new System.NotImplementedException();
    }
}