using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectManager : MonoBehaviour
{
    private PlayerController _selectPlayer;
    
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭으로 플레이어 선택
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
                    // 선택 해제
                    DeselectPlayer();
                }
            }
            else
            {
                // 선택 해제
                DeselectPlayer();
            }
        }
        else if (Input.GetMouseButtonDown(1) && _selectPlayer != null) // 선택된 플레이어로 이동하고싶은 위치 우클릭 했을 때
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
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
