using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using Management;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAttack
{
    private PlayerMoveState _moveState;
    private Animator _animator;
    public GameObject selectMarker;
    
    private bool isSelect = false;
    
    private static readonly int Walk = Animator.StringToHash("Walk");
    
    // 공격 관련
    public StateMachine<PlayerState> StateMachine { get; private set; }
    public IAttack CurrentTarget { get; private set; }
    [SerializeField] private float moveSpeed = 5f; // 이동 속도
    [SerializeField] private float attackRange = 2f; // 공격 범위
    [SerializeField] private float attackDamage = 10f; // 공격 데미지

    public float AttackRange => attackRange;

    private HpScript _hpScript;

    private void Awake()
    {
        _moveState = GetComponent<PlayerMoveState>();
        _animator = GetComponent<Animator>();
        if (selectMarker != null) selectMarker.SetActive(false);
        StateMachine = GetComponent<StateMachine<PlayerState>>();
        _hpScript = GetComponent<HpScript>();
    }

    public void Select()
    {
        isSelect = true;
        if (selectMarker != null) selectMarker.SetActive(true);
        WalkAnim(true);
    }

    public void Deselect()
    {
        isSelect = false;
        if (selectMarker != null) selectMarker.SetActive(false);
        WalkAnim(false);
    }
    
    // 선택한 지점으로 이동
    public void MoveToPoint(Vector3 destination)
    {
        // Debug.Log("목표로 이동");
        _moveState.StartMove(destination);
    }

    public void WalkAnim(bool isWalk)
    {
        _animator.SetBool(Walk, isWalk);
    }

    // 목적지로 이동
    public void MoveInDirection(Vector3 direction)
    {
        transform.position += direction * (moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
    }

    // 공격대상 지정
    public void SetAttackTarget(IAttack target)
    {
        CurrentTarget = target;
        PlayerAttackState attackState = GetComponent<PlayerAttackState>();
        if (attackState != null)
        {
            attackState.SetTarget(target);
            // 공격 상태로 전환
            StateMachine.ChangeState(PlayerState.Attack);
        }
    }
    
    public void PerformAttack(IAttack target)
    {
        AttackSystem.PerformAttack(this, target, attackDamage);
        OnAttackPerformed();
    }
    
    private void OnAttackPerformed()
    {
        Debug.Log("플레이어 공격 수행");
        // 사운드 또는 파티클 추가
    }

    // 적을 우클릭으로 지정해서 해당 적을 추격
    public void ChaseTarget(Transform target)
    {
        StopCurrentMove();
        StartCoroutine(ChaseCoroutine(target));
    }
    
    private void StopCurrentMove() => StopAllCoroutines();

    private IEnumerator ChaseCoroutine(Transform target)
    {
        while (target !=null && Vector3.Distance(transform.position, target.position) > attackRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            MoveInDirection(direction);
            yield return null;
        }
        
        // 적을 따라잡으면 공격 상태로 전환
        StateMachine.ChangeState(PlayerState.Attack);
    }
    
    public void TakeDamage(float damage) => _hpScript.TakeDamage(damage);
    public float GetHealth() => _hpScript.GetCurrentHp();
    public bool IsAlive() => _hpScript.IsAlive();
    public Transform GetTransform() => transform;
    public HpScript GetHpScript() => _hpScript;
}
