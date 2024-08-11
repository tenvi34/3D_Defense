using System.Collections;
using Interface;
using Management;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAttack
{
    private PlayerMoveState _moveState;
    private Animator _animator;
    private HpScript _hpScript;
    public GameObject selectMarker;
    
    private bool isSelect = false;
    
    // 애니메이터 캐싱
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    // 공격 관련
    public StateMachine<PlayerState> StateMachine { get; private set; }
    public IAttack CurrentTarget { get; private set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float detectionRange = 3f;

    public float AttackRange => attackRange;

    private Coroutine currentAction;

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
    }

    public void Deselect()
    {
        isSelect = false;
        if (selectMarker != null) selectMarker.SetActive(false);
    }
    
    public void MoveToPoint(Vector3 destination)
    {
        StopCurrentAction();
        currentAction = StartCoroutine(MoveCoroutine(destination));
    }

    private IEnumerator MoveCoroutine(Vector3 destination)
    {
        WalkAnim(true);
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            Vector3 direction = (destination - transform.position).normalized;
            MoveInDirection(direction);
            yield return null;
        }
        WalkAnim(false);
        StateMachine.ChangeState(PlayerState.Move);
    }

    public void MoveInDirection(Vector3 direction)
    {
        transform.position += direction * (moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
    }

    private void WalkAnim(bool isWalk)
    {
        _animator.SetBool(Walk, isWalk);
    }

    private void AttackAnim(bool isAttack)
    {
        _animator.SetBool(Attack, isAttack);
    }

    public void SetAttackTarget(IAttack target)
    {
        StopCurrentAction();
        CurrentTarget = target;
        if (target != null)
        {
            currentAction = StartCoroutine(AttackCoroutine(target));
        }
    }
    
    private IEnumerator AttackCoroutine(IAttack target)
    {
        while (target != null && target.IsAlive())
        {
            float distance = Vector3.Distance(transform.position, target.GetTransform().position);
            if (distance > attackRange)
            {
                Vector3 direction = (target.GetTransform().position - transform.position).normalized;
                MoveInDirection(direction);
                WalkAnim(true);
            }
            else
            {
                WalkAnim(false);
                PerformAttack(target);
                yield return new WaitForSeconds(1f); // 공격 쿨타임
            }
            yield return null;
        }
        
        Debug.Log("적 처치");
        CurrentTarget = null;
        StateMachine.ChangeState(PlayerState.Move);
        WalkAnim(false);
        AttackAnim(false);
    }
    
    public void PerformAttack(IAttack target)
    {
        AttackSystem.PerformAttack(this, target, attackDamage);
        OnAttackPerformed();
        
        // 공격 후 타겟의 상태 확인
        if (target == null || !target.IsAlive())
        {
            Debug.Log("적 처치, Move 상태로 전환");
            StopCurrentAction();
            StateMachine.ChangeState(PlayerState.Move);
        }
    }
    
    private void OnAttackPerformed()
    {
        Debug.Log("플레이어 공격 수행");
        AttackAnim(true);
        // 사운드 또는 파티클 추가
    }

    public IAttack FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        IAttack nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            IAttack enemy = collider.GetComponent<IAttack>();
            if (enemy != null && enemy.IsAlive())
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestEnemy = enemy;
                    nearestDistance = distance;
                }
            }
        }

        return nearestEnemy;
    }

    private void StopCurrentAction()
    {
        if (currentAction != null)
        {
            StopCoroutine(currentAction);
            currentAction = null;
        }
        WalkAnim(false);
        AttackAnim(false);
        CurrentTarget = null;
    }
    
    public void TakeDamage(float damage)
    {
        _hpScript.TakeDamage(damage);
        
        // 공격받았을 때 자동으로 반격
        if (CurrentTarget == null)
        {
            IAttack attacker = FindNearestEnemy();
            if (attacker != null)
            {
                SetAttackTarget(attacker);
            }
        }
    }

    public float GetHealth() => _hpScript.GetCurrentHp();
    public bool IsAlive() => _hpScript.IsAlive();
    public Transform GetTransform() => transform;
    public HpScript GetHpScript() => _hpScript;
}