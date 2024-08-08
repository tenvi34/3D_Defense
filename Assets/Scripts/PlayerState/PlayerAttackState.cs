using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : VMyState<PlayerState>
{
    public override PlayerState StateEnum => PlayerState.Attack;
    private PlayerController _playerController;

    protected override void Awake()
    {
        base.Awake();
        _playerController = GetComponent<PlayerController>();
    }

    protected override void EnterState()
    {
        // Debug.Log("Player Attack State 접근");
    }
    
    protected override void ExcuteState()
    {
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
    }
}
