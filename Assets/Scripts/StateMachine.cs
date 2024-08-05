using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class NotifyBase
{
}

public class NotifyStun : NotifyBase
{
    public int a = 10;
}

public interface IVMyState
{
    // 현재 상태에게 무언가를 알리려고 할때 쓰는 API
    void OnNotify<T, T2>(T eValue, T2 vValue) where T : Enum where T2 : NotifyBase;
    
    // 상태가 시작될때 불리는 API
    public void EnterStateWrapper();
    
    // Update와 같음
    public void ExcuteStateWrapper();
    
    // 상태가 끝날 때 불리는 API
    public void ExitStateWrapper();

    // FixedUpdate
    public void ExcuteState_FixedUpdateWrapper();

    // LateUpdate
    public void ExcuteState_LateUpdateWrapper();

    // 상태를 추가해주는 api
    public void AddState<T>(StateMachine<T> owner, ref object _states) where T : Enum;
}

// 혹시 기능을 확장해야 할때 쓰려고 여유로 만들어둠
public abstract class VMyStateBase : MonoBehaviour
{
}

// 제네릭의 타입 T는 enum만 설정 가능하다.
public abstract class VMyState<T> : VMyStateBase, IVMyState where T : Enum
{
    // 모든 스테이트에서 override해서 자신의 상태와 맞춰줘야 하는 변수
    public abstract T StateEnum { get; }
    
    [NonSerialized]public StateMachine<T> OwnerStateMachine;

    protected virtual void Awake()
    {
    }
    
    protected virtual void Start()
    {
    }

    public virtual void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        throw new NotImplementedException();
    }

    public void EnterStateWrapper()
    {
        EnterState();
    }

    public void ExcuteStateWrapper()
    {
        ExcuteState();
    }
    
    public void ExcuteState_FixedUpdateWrapper()
    {
        ExcuteState_FixedUpdate();
    }
    
    public void ExcuteState_LateUpdateWrapper()
    {
        ExcuteState_LateUpdate();
    }

    public virtual void ExitStateWrapper()
    {
        ExitState();
    }

    public void AddState<T1>(StateMachine<T1> owner, ref object _states) where T1 : Enum
    {
        var cast =_states as SerializedDictionary<T, IVMyState>;
        OwnerStateMachine = owner as StateMachine<T>;
        cast?.Add(StateEnum, this);
    }

    // 실제로 사용자가 사용하게 될 api들
    protected virtual void EnterState()
    {
        
    }

    protected virtual void ExcuteState()
    {
        
    }

    protected virtual void ExitState()
    {
        
    }
    
    protected virtual void ExcuteState_FixedUpdate()
    {
        
    }

    protected virtual void ExcuteState_LateUpdate()
    {
        
    }
    // 끝
}

public abstract class HFSMVMyState<T, T2> : VMyState<T2> where T : Enum where T2 : Enum
{
    // HSFM 이용 할 시
    public  StateMachine<T> HSFM_StateMachine;

    public override void ExitStateWrapper()
    {
        base.ExitStateWrapper();
        
        if (HSFM_StateMachine)
        {
            HSFM_StateMachine.ChangeStateNull();
        }
    }
}

public class StateMachine<T> : MonoBehaviour where T : Enum
{
    [SerializeField] private T defaultState;
    
    [SerializeField] private IVMyState _currentMyState;
    private SerializedDictionary<T, IVMyState> _states = new();
    
    StateMachine<T> GetSuperOwnerStateMachile()
    {
        StateMachine<T> stateMachine = GetComponentInParent< StateMachine<T>>();
        if (stateMachine)
        {
            return stateMachine.GetSuperOwnerStateMachile();
        }

        return this;
    }
    
    private void ChangeState_Internal(IVMyState newMyState)
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

    public void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        _currentMyState.OnNotify(eValue, vValue);
    }

    public void ChangeStateNull()
    {
        ChangeState_Internal(null);
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
    protected virtual void Awake()
    {
        // 이거는 성능이 직접 컴포넌트 가져오는 방식 대비 비싸다.
        var stateArray = GetComponents<VMyStateBase>().OfType<IVMyState>().ToList();
        foreach (var state in stateArray)
        {
            object states = _states;
            state.AddState(this, ref states);
        }
    }

    protected virtual void Start()
    {
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

    private void FixedUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_FixedUpdateWrapper();
        }
    }

    private void LateUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_LateUpdateWrapper();
        }
    }
}