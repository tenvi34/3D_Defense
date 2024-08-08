using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : VMyState<PlayerState>
{
    public override PlayerState StateEnum => PlayerState.Move;
    private PlayerController _playerController;
    private Vector3 _destination;
    private float speed = 5f;
    private bool isMove = false;

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
        if (isMove)
        {
            MoveToClickPosition();
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
        // Debug.Log("Player Move State 종료");
    }

    public void StartMove(Vector3 destination)
    {
        _destination = destination;
        isMove = true;
        EnterState();
    }

    private void MoveToClickPosition()
    {
        if (Vector3.Distance(transform.position, _destination) > 0.1f)
        {
            Vector3 direction = (_destination - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);
        }
        else
        {
            isMove = false;
            ExitState();
        }
    }
}
