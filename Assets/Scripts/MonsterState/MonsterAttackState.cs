using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class MonsterAttackState : VMyState<MonsterState>
{
    public override MonsterState StateEnum => MonsterState.Attack;
    private MonsterController _monsterController;
    
    [SerializeField] private float attackCoolTime = 2.0f;
    private bool canAttack = true;
    private GameObject currentTarget;

    protected override void Awake()
    {
        base.Awake();
        _monsterController = GetComponent<MonsterController>();
    }
    
    protected override void EnterState()
    {
        //Debug.Log("공격 상태 진입");
        canAttack = true;
        currentTarget = _monsterController.FindTarget();
    }

    protected override void ExcuteState_FixedUpdate()
    {
        if (currentTarget == null)
        {
            currentTarget = _monsterController.FindTarget();
            if (currentTarget == null)
            {
                _monsterController._stateMachine.ChangeState(MonsterState.Move);
                return;
            }
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, currentTarget.transform.position);
        
        if (distanceToPlayer <= _monsterController.attackRange)
        {
            // 공격 범위 안에 있으면 공격
            if (canAttack)
            {
                StartCoroutine(Attack(currentTarget));
            }
        }
        else if (distanceToPlayer <= _monsterController.detectionRange)
        {
            // 감지 범위 안에 있지만 공격 범위 밖이면 플레이어를 향해 이동
            Vector3 directionToTarget = (currentTarget.transform.position - transform.position).normalized;
            _monsterController.MoveInDirection(directionToTarget);
        }
        else
        {
            // 감지 범위를 벗어나면 다시 Move 상태
            _monsterController._stateMachine.ChangeState(MonsterState.Move);
        }
    }

    private IEnumerator Attack(GameObject target)
    {
        Debug.Log(target + "에게 공격 시도");
        canAttack = false;

        yield return new WaitForSeconds(attackCoolTime);
        canAttack = true;
    }

    protected override void ExitState()
    {
        //Debug.Log("공격 상태 종료");
        StopAllCoroutines();
        currentTarget = null;
    }
    
    public T GetEnumValue<T>() where T : System.Enum
    {
        throw new System.NotImplementedException();
    }
}