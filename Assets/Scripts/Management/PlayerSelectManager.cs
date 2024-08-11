using Interface;
using UnityEngine;

public class PlayerSelectManager : MonoBehaviour
{
    private PlayerController _selectPlayer;
    
    void Update()
    {
        PlayerMouseClick();
    }

    private void PlayerMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1) && _selectPlayer != null)
        {
            HandleRightClick();
        }
    }

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
                    // Debug.Log("적 타겟 지정 및 공격 시작");
                }
            }
            else
            {
                // 공격 중이어도 다른 지점 클릭 시 이동
                _selectPlayer.MoveToPoint(raycastHit.point);
                // Debug.Log("새로운 위치로 이동");
            }
        }
    }

    private void SelectPlayer(PlayerController player)
    {
        if (_selectPlayer != null)
        {
            _selectPlayer.Deselect();
        }
        _selectPlayer = player;
        _selectPlayer.Select();
    }

    private void DeselectPlayer()
    {
        if (_selectPlayer != null)
        {
            _selectPlayer.Deselect();
            _selectPlayer = null;
        }
    }
}