using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class VMyState<T> : MonoBehaviour where T : Enum
{
    public abstract T StateEnum { get; }
    
    [NonSerialized]public StateMachine<T> OwnerStateMachine;

    // HSFM 이용 할 시
    public  StateMachine<T> HSFM_StateMachine;

    public void EnterStateWrapper()
    {
        EnterState();
    }

    public void ExcuteStateWrapper()
    {
        ExcuteState();
    }

    public void ExitStateWrapper()
    {
        ExitState();
        if (HSFM_StateMachine)
        {
            HSFM_StateMachine.ChangeState((T)Enum.Parse(typeof(T), "NoneMyState"));
        }
    }
    
    public abstract void  EnterState();

    public abstract void ExcuteState();

    public abstract void ExitState();
}

public class StateMachine<T> : MonoBehaviour where T : System.Enum
{
    [SerializeField] private T defaultState;
    
    private VMyState<T> _currentMyState;
    private Dictionary<T, VMyState<T>> _states = new();
    
    StateMachine<T> GetSuperOwnerStateMachile()
    {
        StateMachine<T> stateMachine = GetComponentInParent< StateMachine<T>>();
        if (stateMachine)
        {
            return stateMachine.GetSuperOwnerStateMachile();
        }

        return this;
    }
    
    private void ChangeState_Internal(VMyState<T> newMyState)
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExitStateWrapper();
        }

        if (newMyState == null)
        {
            _currentMyState = null;
            return;
        }

        _currentMyState = newMyState;
        _currentMyState.EnterStateWrapper();
    }

    public void ChangeState(T state)
    {
        // 상태가 None이 아니면 돌릴 상태가 있으므로 Active
        if (_states.TryGetValue(state, out var newState))
        {
            ChangeState_Internal(newState);
        }
        else
        {
            Debug.LogError($"State {state} not found in the state machine.");
            ChangeState_Internal(null);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // 이거는 성능이 직접 컴포넌트 가져오는 방식 대비 비싸다.
        VMyState<T>[] stateArray = GetComponents<VMyState<T>>();
        foreach (var state in stateArray)
        {
            state.OwnerStateMachine = this;
            _states.Add(state.StateEnum, state);
        }   
    
        // DefaultState
        ChangeState(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteStateWrapper();
        }
    }
}
