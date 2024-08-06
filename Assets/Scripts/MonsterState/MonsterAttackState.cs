using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class MonsterAttackState : VMyState<MonsterState>
{
    public override MonsterState StateEnum => MonsterState.Attack;
    private MonsterController _monsterController;
    
    [SerializeField] private float attackCoolTime = 2.0f; // 공격 쿨타임
    private bool canAttack = true;

    protected override void Awake()
    {
        base.Awake();
        _monsterController = GetComponent<MonsterController>();
        if (_monsterController == null)
        {
            Debug.LogError("MonsterController가 연결되지 않음");
        }
    }
    
    protected override void EnterState()
    {
        Debug.Log("공격 가능");
        canAttack = true;
    }

    protected override void ExcuteState()
    {
       
    }
    
    protected override void ExcuteState_FixedUpdate()
    {
        if (_monsterController == null)
        {
            Debug.LogError("MonsterController 초기화 실패");
            return;
        }
        
        GameObject target = _monsterController.FindTarget(); // 목표 찾기
        if (target != null) // 찾으면
        {
            float distance = Vector3.Distance(transform.position, target.transform.position); // 몬스터와 플레이어의 거리
            if (distance <= _monsterController.attackRange && canAttack)
            {
                // 공격 실행
                StartCoroutine(Attack(target));
            }
            else if (distance > _monsterController.attackRange)
            {
                _monsterController._stateMachine.ChangeState(MonsterState.Move);
            }
        }
        else // 못찾으면 그대로 이동
        {
            _monsterController._stateMachine.ChangeState(MonsterState.Move);
        }
    }

    private IEnumerator Attack(GameObject target)
    {
        Debug.Log(target + "에게 공격 시도");
        canAttack = false; // 공격 후 공격 불가능하게 변경

        yield return new WaitForSeconds(attackCoolTime); // 쿨타임이 지나면
        canAttack = true; // 다시 공격 가능 상태로
    }

    protected override void ExitState()
    {
        Debug.Log("공격 상태 종료");
        StopAllCoroutines();
    }

    public T GetEnumValue<T>() where T : System.Enum
    {
        throw new System.NotImplementedException();
    }
}
