using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Move,
    Attack,
}

public class PlayerStateMachine : StateMachine<PlayerState>
{
    protected override void Start()
    {
        base.Start();
        ChangeState(PlayerState.Move);
    }
}
