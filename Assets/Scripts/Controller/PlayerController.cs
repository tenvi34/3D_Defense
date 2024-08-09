using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMoveState _moveState;
    private Animator _animator;
    public GameObject selectMarker;
    
    private bool isSelect = false;
    
    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Awake()
    {
        _moveState = GetComponent<PlayerMoveState>();
        _animator = GetComponent<Animator>();
        if (selectMarker != null) selectMarker.SetActive(false);
    }

    public void Select()
    {
        Debug.Log("플레이어 선택");
        isSelect = true;
        if (selectMarker != null) selectMarker.SetActive(true);
        WalkAnim(true);
    }

    public void Deselect()
    {
        isSelect = false;
        if (selectMarker != null) selectMarker.SetActive(false);
        WalkAnim(false);
    }
    
    // 선택한 지점으로 이동
    public void MoveToPoint(Vector3 destination)
    {
        Debug.Log("목표로 이동");
        _moveState.StartMove(destination);
    }

    public void WalkAnim(bool isWalk)
    {
        Debug.Log("걷기 동작 실행");
        _animator.SetBool(Walk, isWalk);
    }
}
