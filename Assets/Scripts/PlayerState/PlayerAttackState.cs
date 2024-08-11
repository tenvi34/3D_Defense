using System;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttackState : VMyState<PlayerState>
{
    public override PlayerState StateEnum => PlayerState.Attack;
    private PlayerController _playerController;
    
    [SerializeField] private float attackCoolTime = 2.0f;
    private bool canAttack = true;
    private IAttack currentTarget;

    protected override void Awake()
    {
        base.Awake();
        _playerController = GetComponent<PlayerController>();
    }
    
    protected override void EnterState()
    {
        //Debug.Log("Player Attack State 진입");
        canAttack = true;
        currentTarget = _playerController.CurrentTarget;
    }

    protected override void ExcuteState_FixedUpdate()
    {
        if (currentTarget == null || !currentTarget.IsAlive())
        {
            _playerController.StateMachine.ChangeState(PlayerState.Move);
            return;
        }
        
        float distance = Vector3.Distance(transform.position, currentTarget.GetTransform().position);
        
        if (distance <= _playerController.AttackRange)
        {
            if (canAttack)
            {
                PerformAttackAsync().Forget();
            }
        }
        else
        {
            Vector3 directionToTarget = (currentTarget.GetTransform().position - transform.position).normalized;
            _playerController.MoveInDirection(directionToTarget);
        }
    }
    
    private async UniTaskVoid PerformAttackAsync()
    {
        canAttack = false;
        _playerController.PerformAttack(currentTarget);

        await UniTask.Delay(TimeSpan.FromSeconds(attackCoolTime));
        canAttack = true;
    }

    protected override void ExitState()
    {
        //Debug.Log("Player Attack State 종료");
        currentTarget = null;
    }

    public void SetTarget(IAttack target)
    {
        currentTarget = target;
    }
}