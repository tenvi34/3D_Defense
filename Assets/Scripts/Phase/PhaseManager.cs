using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}