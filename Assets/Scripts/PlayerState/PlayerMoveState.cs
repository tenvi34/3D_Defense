using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : VMyState<PlayerState>
{
    public override PlayerState StateEnum => PlayerState.Move;
    private PlayerController _playerController;

    protected override void Awake()
    {
        base.Awake();
        _playerController = GetComponent<PlayerController>();
    }

    protected override void EnterState()
    {
        // Debug.Log("Player Move State 접근");
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
        // Debug.Log("Player Move State 종료");
    }
}
