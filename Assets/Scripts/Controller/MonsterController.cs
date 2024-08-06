using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterController : MonoBehaviour
{
    public int DestinationIndex = 0;
    public float Speed = 5.0f;
    
    private Rigidbody _rigidbody;
    public StateMachine<MonsterState> _stateMachine;

    //공격 관련
    public float attackRange = 2.0f; // 공격 범위
    public float detectionRadius = 5.0f; // 플레이어 감지 범위
    public LayerMask playerLayer; // 플레이어 레이어

    private GameObject currentTarget;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<StateMachine<MonsterState>>();
        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<StateMachine<MonsterState>>();
        }
        _stateMachine.ChangeState(MonsterState.Move);
    }
    
    void FixedUpdate()
    {
        MoveToNextDestination();
        
        // // ======================= 이동 기능 =======================
        // // 목적지 정보를 PhaseManager에서 가져옴
        // (int resultIndex, Vector3 destination) = PhaseManager.Instance.GetDestination(DestinationIndex);
        // // Debug.Log($"현재 위치: {transform.position}, 목적지: {destination}, 남은 거리: {Vector3.Distance(transform.position, destination)}");
        //
        // // 몬스터가 이동할 때 목적지에 도달했는지 확인
        // if (MoveToDestination(destination))
        // {
        //     Debug.Log("도착, 다음 목적지로 이동 시도");
        //     DestinationIndex = resultIndex + 1; // 다음 목적지로 인덱스 이동
        // }
        //
        // // ======================= 공격 기능 =======================
        // GameObject target = FindTarget();
        // if (target != null) // 타겟을 찾으면 상태 변경
        // {
        //     _stateMachine.ChangeState(MonsterState.Attack);
        // }
    }
    
    // 다음 목적지로 이동
    public bool MoveToDestination(Vector3 destination)
    {
        // 현재 위치와 목적지의 Y축을 동일하게 설정하여 Y축을 무시
        Vector3 currentPosition = new Vector3(transform.position.x, destination.y, transform.position.z);
        Vector3 direction = destination - currentPosition;

        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, Speed * Time.deltaTime);
            _rigidbody.MovePosition(nextPosition);

            if (direction != Vector3.zero)
            {
                _rigidbody.MoveRotation(Quaternion.LookRotation(direction.normalized));
            }
        }
        else
        {
            return true; // 다음 목적지로 바로 넘어가도록 처리
        }

        return Vector3.Distance(currentPosition, destination) <= 0.5f;
    }

    private void MoveToNextDestination()
    {
        // 플레이어 감지
        GameObject detectedPlayer = FindTarget();
        if (detectedPlayer != null)
        {
            currentTarget = detectedPlayer;
            _stateMachine.ChangeState(MonsterState.Attack);
            return;
        }

        (int resultIndex, Vector3 destination) = PhaseManager.Instance.GetDestination(DestinationIndex);

        if (MoveToDestination(destination))
        {
            DestinationIndex = resultIndex + 1;
        }
    }

    public void MoveInDirection(Vector3 direction)
    {
        Vector3 movement = direction * Speed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + movement);

        if (direction != Vector3.zero)
        {
            _rigidbody.MoveRotation(Quaternion.LookRotation(direction));
        }
    }
    
    // 공격대상 감지
    public GameObject FindTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (findTarget.Length > 0)
        {
            return findTarget[0].gameObject; // 하나의 타겟만 지정
        }

        return null;
    }
}