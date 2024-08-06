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
    
    // public MonsterState currentState;
    // private MonsterController _monsterController;
    //
    // void Awake()
    // {
    //     _monsterController = GetComponent<MonsterController>();
    // }
    //
    // void Start()
    // {
    //     ChangeState(MonsterState.Move);
    // }
    //
    // void Update()
    // {
    //     ExecuteState();
    // }
    //
    // private void ChangeState(MonsterState state)
    // {
    //     currentState = state;
    //     OnStateEnter(state);
    // }
    //
    // private void OnStateEnter(MonsterState state)
    // {
    //     switch (state)
    //     {
    //         case MonsterState.Move:
    //             // Move 상태
    //             break;
    //         case MonsterState.Attack:
    //             // Attack 상태
    //             break;
    //     }
    // }
    //
    // private void ExecuteState()
    // {
    //     switch (currentState)
    //     {
    //         case MonsterState.Move:
    //             RunMoveState();
    //             break;
    //         case MonsterState.Attack:
    //             // Attack 구현
    //             break;
    //     }
    // }
    //
    // private void RunMoveState()
    // {
    //     (int resultIndex, Vector3 destination) =
    //         PhaseManager.Instance.GetDestination(_monsterController.DestinationIndex);
    //
    //     _monsterController.DestinationIndex = resultIndex + 1;
    // }
}
