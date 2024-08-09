using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class PlayerSelectManager : MonoBehaviour
{
    // 플레이어 선택
    private PlayerController _selectPlayer;
    
    void Update()
    {
        PlayerMouseClick();
    }

    private void PlayerMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 플레이어 선택
            HandleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1) && _selectPlayer != null)
        {
            // 플레이어 이동
            HandleRightClick();
        }
    }

    // 좌클릭
    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Player"))
            {
                SelectPlayer(raycastHit.collider.GetComponent<PlayerController>());
            }
            else
            {
                DeselectPlayer();
            }
        }
        else
        {
            DeselectPlayer();
        }
    }

    // 우클릭
    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                IAttack target = raycastHit.collider.GetComponent<IAttack>();
                if (target != null)
                {
                    _selectPlayer.SetAttackTarget(target);
                    _selectPlayer.ChaseTarget(target.GetTransform());
                    Debug.Log("타켓 추격 시작");
                }
                else
                {
                    // 다른 방법 고민 중...
                }
            }
            else
            {
                // 선택한 지점으로 이동
                _selectPlayer.MoveToPoint(raycastHit.point);
            }
        }
    }

    // 플레이어 선택
    private void SelectPlayer(PlayerController player)
    {
        if (_selectPlayer != null)
        {
            _selectPlayer.Deselect();
        }
        _selectPlayer = player;
        _selectPlayer.Select();
    }

    // 플레이어 선택 해제
    private void DeselectPlayer()
    {
        if (_selectPlayer != null)
        {
            _selectPlayer.Deselect();
            _selectPlayer = null;
        }
    }
}
