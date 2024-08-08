using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMoveState _moveState;
    public GameObject selectMarker;
    private bool isSelect = false;

    private void Awake()
    {
        _moveState = GetComponent<PlayerMoveState>();
        if (selectMarker != null) selectMarker.SetActive(false);
    }

    public void Select()
    {
        Debug.Log("플레이어 선택");
        isSelect = true;
        if (selectMarker != null) selectMarker.SetActive(true);
    }

    public void Deselect()
    {
        isSelect = false;
        if (selectMarker != null) selectMarker.SetActive(false);
    }
    
    // 선택한 지점으로 이동
    public void MoveToPoint(Vector3 destination)
    {
        _moveState.StartMove(destination);
    }
}
