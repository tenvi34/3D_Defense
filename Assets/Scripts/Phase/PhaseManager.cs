using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum PhaseState
{
    NoneMyState,
    Phase1_Ready,
    Phase1_Running
}

public class PhaseManager : SceneSingleton<PhaseManager>
{
    private PhaseStateMachine _stateMachine;
    public List<Transform> _destinations = new();
    
    void Awake()
    {
        base.Awake();
        _stateMachine = GetComponent<PhaseStateMachine>();
    }

    public (int, Vector3) GetDestination(int index)
    {
        int resultIndex = index;
        if (resultIndex >= _destinations.Count)
        {
            resultIndex = 0;
        }

        return (resultIndex, _destinations[resultIndex].position);
    }
    
    public int GetDestinationCount()
    {
        return _destinations.Count;
    }

    // 적 랜덤 생성 위치 찾기
    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 firstPos = _destinations[0].position;
        Vector3 lastPos = _destinations[_destinations.Count - 1].position;

        float randomX = Random.Range(Mathf.Min(firstPos.x, lastPos.x), Mathf.Max(firstPos.x, lastPos.x));
        float randomZ = Random.Range(Mathf.Min(firstPos.z, lastPos.z), Mathf.Max(firstPos.z, lastPos.z));

        return new Vector3(randomX, firstPos.y, randomZ);
    }
    
    // 랜덤 생성 후 가장 가까운 Destination으로 찾아가는 메서드
    public int GetNearDestination(Vector3 position)
    {
        int nearIndex = 0;
        float minDistance = float.MaxValue; // 제일 가까운 Destination 계산

        for (int i = 0; i < _destinations.Count; i++)
        {
            float distance = Vector3.Distance(position, _destinations[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearIndex = i; // 제일 가까운 Destination Index
            }
        }
        
        return nearIndex;
    }
}