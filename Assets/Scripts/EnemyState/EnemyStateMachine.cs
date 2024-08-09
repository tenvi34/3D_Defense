using UnityEngine;

public enum EnemyState
{
    Move,   // 이동
    Attack, // 공격
}

public class EnemyStateMachine : StateMachine<EnemyState>
{
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyState.Move);
    }

    public void TriggerMoveState()
    {
        ChangeState(EnemyState.Move);
    }
    
    public void TriggerAttackState()
    {
        ChangeState(EnemyState.Attack);
    }
}
