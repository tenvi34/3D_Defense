using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int DestinationIndex = 0;
    public float Speed = 5.0f;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    // public bool MoveToDestination(Vector3 destination)
    // {
    //     // 이동
    //     Vector3 nextPosition = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
    //     _rigidbody.MovePosition(nextPosition);
    //     _rigidbody.MoveRotation(Quaternion.LookRotation((nextPosition - transform.position).normalized));
    //
    //     // 다음 Destination에 도달했는지 확인
    //     return Vector3.Distance(transform.position, destination) <= 1.0f;
    // }
    
    public bool MoveToDestination(Vector3 destination)
    {
        // 현재 위치와 목적지의 Y축을 동일하게 설정하여 Y축을 무시
        Vector3 currentPosition = new Vector3(transform.position.x, destination.y, transform.position.z);
        Vector3 direction = destination - currentPosition;

        if (direction.sqrMagnitude > 0.01f) // 벡터의 길이가 0이 아니면
        {
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, Speed * Time.deltaTime);
            _rigidbody.MovePosition(nextPosition);

            if (direction != Vector3.zero) // 방향 벡터가 0이 아닐 때만 회전
            {
                _rigidbody.MoveRotation(Quaternion.LookRotation(direction.normalized));
            }
        }
        else
        {
            Debug.LogWarning("Target position is the same as current position. Moving to next destination.");
            return true; // 다음 목적지로 바로 넘어가도록 처리
        }

        return Vector3.Distance(currentPosition, destination) <= 0.5f; // 임계값 조정
    }


    
    
    void FixedUpdate()
    {
        // 목적지 정보를 PhaseManager에서 가져옴
        (int resultIndex, Vector3 destination) = PhaseManager.Instance.GetDestination(DestinationIndex);
        Debug.Log($"현재 위치: {transform.position}, 목적지: {destination}, 남은 거리: {Vector3.Distance(transform.position, destination)}");
    
        // 몬스터가 이동할 때 목적지에 도달했는지 확인
        if (MoveToDestination(destination))
        {
            Debug.Log("도착, 다음 목적지로 이동 시도");
            DestinationIndex = resultIndex + 1; // 다음 목적지로 인덱스 이동
        }
    }
}