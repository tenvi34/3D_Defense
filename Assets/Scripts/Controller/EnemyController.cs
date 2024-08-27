using Interface;
using Management;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAttack
{
    public int DestinationIndex = 0;
    public float Speed = 5.0f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    public StateMachine<EnemyState> _stateMachine;

    //공격 관련
    [SerializeField] private float attackRange = 2.0f; // 공격 범위
    [SerializeField] private float attackDamage = 5f; // 공격 데미지
    [SerializeField] private LayerMask playerLayer; // 플레이어 레이어
    [SerializeField] private float detectionRange = 3.0f; // 플레이어 감지 범위
    private GameObject currentTarget; // 공격 대상
    
    public float AttackRange => attackRange;
    public float DetectionRange => detectionRange;
    
    private HpScript _hpScript;
    
    // 목적지 이동
    private bool isReverse = false; // Destination 역순으로
    
    // 적 감지 범위 표시 관련
    public bool showDetectionRange = true; // 표시 여부
    private LineRenderer detectionRangeRenderer; // 감지 범위 표시
    
    // 적 감지 범위 표시 관련
    public bool showAttackRange = true; // 표시 여부
    private LineRenderer attackRangeRenderer; // 감지 범위 표시
    
    // 선택 관련
    public GameObject selectMarker;
    private bool isSelect = false;
    
    // 애니메이터 캐싱
    private static readonly int Attack = Animator.StringToHash("Attack");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<StateMachine<EnemyState>>();
        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<StateMachine<EnemyState>>();
        }
        
        // 감지 범위, 공격 범위 표시
        if (showDetectionRange) ShowDetectionRange();
        if (showAttackRange) ShowAttackRange();
        
        // 선택 마커
        if (selectMarker != null) selectMarker.SetActive(false);
        
        // HP 스크립트
        _hpScript = GetComponent<HpScript>();
    }
    
    void Update()
    {
        // 감지 범위
        if (showDetectionRange && detectionRangeRenderer != null)
        {
            detectionRangeRenderer.gameObject.SetActive(true);
        }
        else if (detectionRangeRenderer != null)
        {
            detectionRangeRenderer.gameObject.SetActive(false);
        }
        
        // 공격 범위
        if (showAttackRange && attackRangeRenderer != null)
        {
            attackRangeRenderer.gameObject.SetActive(true);
        }
        else if (attackRangeRenderer != null)
        {
            attackRangeRenderer.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        // ======================= 이동 기능 =======================
        // 목적지 정보를 PhaseManager에서 가져옴
        (int resultIndex, Vector3 destination) = PhaseManager.Instance.GetDestination(DestinationIndex);
        
        // 몬스터가 이동할 때 목적지에 도달했는지 확인
        if (MoveToDestination(destination))
        {
            UpdateDestinationIndex(resultIndex);
        }
        
        // ======================= 공격 기능 =======================
        // GameObject target = FindTarget();
        // if (target != null) // 타겟을 찾으면 상태 변경
        // {
        //     Debug.Log("공격 상태 전환");
        //     _stateMachine.ChangeState(MonsterState.Attack);
        // }
    }
    
    // 다음 목적지로 이동
    public bool MoveToDestination(Vector3 destination)
    {
        // 현재 위치와 목적지의 Y축을 동일하게 설정하여 Y축을 무시 -> 자꾸 비정상적인 움직임을 보이면서 다음 목적지로 이동 불가능해서 수정
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

    // 목적지 도착 시 역순으로 다시 순찰
    private void UpdateDestinationIndex(int resultIndex)
    {
        if (isReverse)
        {
            DestinationIndex--;

            if (DestinationIndex <= 0) // 첫 목적지에 도달하면 방향을 변경
            {
                DestinationIndex = 0;
                isReverse = false;
            }
        }
        else
        {
            DestinationIndex++;

            if (DestinationIndex >= PhaseManager.Instance.GetDestinationCount() - 1) // 마지막 목적지에 도달하면 방향을 변경
            {
                DestinationIndex = PhaseManager.Instance.GetDestinationCount() - 1;
                isReverse = true;
            }
        }
    }
    
    // 공격대상 감지
    public GameObject FindTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        if (findTarget.Length > 0)
        {
            // Debug.Log("플레이어 감지 완료");
            return findTarget[0].gameObject; // 하나의 타겟만 지정
        }

        return null;
    }
    
    public void MoveInDirection(Vector3 direction)
    {
        Vector3 movement = direction.normalized * (Speed * Time.deltaTime);
        _rigidbody.MovePosition(transform.position + movement);
    
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    public void PerformAttack(IAttack target)
    {
        AttackSystem.PerformAttack(this, target, attackDamage);
        OnAttackPerformed();
    }
    
    private void OnAttackPerformed()
    {
        AttackAnim(true);
        // 사운드 또는 파티클 추가
        
    }

    private void AttackAnim(bool isAttack)
    {
        _animator.SetBool(Attack, isAttack);
    }

    // 2024-08-26
    // 제일 가까운 Destination 설정
    public void SetNearDestination()
    {
        DestinationIndex = PhaseManager.Instance.GetNearDestination(transform.position);
    }
    

    // 감지 범위 표시 생성
    void ShowDetectionRange()
    {
        GameObject rangeObject = new GameObject("DetectionRange");
        rangeObject.transform.SetParent(transform);
        rangeObject.transform.localPosition = Vector3.zero;

        detectionRangeRenderer = rangeObject.AddComponent<LineRenderer>();
        detectionRangeRenderer.useWorldSpace = false;
        detectionRangeRenderer.startWidth = 0.1f;
        detectionRangeRenderer.endWidth = 0.1f;
        detectionRangeRenderer.positionCount = 51;
        detectionRangeRenderer.material = new Material(Shader.Find("Sprites/Default"));
        detectionRangeRenderer.startColor = Color.yellow;
        detectionRangeRenderer.endColor = Color.yellow;

        Vector3[] positions = new Vector3[51];
        for (int i = 0; i < 51; i++)
        {
            float angle = i * (360f / 50) * Mathf.Deg2Rad;
            positions[i] = new Vector3(Mathf.Sin(angle) * detectionRange, 0.1f, Mathf.Cos(angle) * detectionRange);
        }
        detectionRangeRenderer.SetPositions(positions);
    }
    
    // 공격 범위 표시 생성
    void ShowAttackRange()
    {
        GameObject rangeObject = new GameObject("AttackRange");
        rangeObject.transform.SetParent(transform);
        rangeObject.transform.localPosition = Vector3.zero;

        attackRangeRenderer = rangeObject.AddComponent<LineRenderer>();
        attackRangeRenderer.useWorldSpace = false;
        attackRangeRenderer.startWidth = 0.1f;
        attackRangeRenderer.endWidth = 0.1f;
        attackRangeRenderer.positionCount = 51;
        attackRangeRenderer.material = new Material(Shader.Find("Sprites/Default"));
        attackRangeRenderer.startColor = Color.red;
        attackRangeRenderer.endColor = Color.red;

        Vector3[] positions = new Vector3[51];
        for (int i = 0; i < 51; i++)
        {
            float angle = i * (360f / 50) * Mathf.Deg2Rad;
            positions[i] = new Vector3(Mathf.Sin(angle) * attackRange, 0.1f, Mathf.Cos(angle) * attackRange);
        }
        attackRangeRenderer.SetPositions(positions);
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
    
    public void TakeDamage(float damage) => _hpScript.TakeDamage(damage);
    public float GetHealth() => _hpScript.GetCurrentHp();
    public bool IsAlive() => _hpScript.IsAlive();
    public Transform GetTransform() => transform;
    public HpScript GetHpScript() => _hpScript;
}







// 다음 목적지로 이동
// public bool MoveToDestination(Vector3 destination)
// {
//     // 현재 위치와 목적지의 Y축을 동일하게 설정하여 Y축을 무시
//     Vector3 currentPosition = new Vector3(transform.position.x, destination.y, transform.position.z);
//     Vector3 direction = destination - currentPosition;
//
//     if (direction.sqrMagnitude > 0.01f)
//     {
//         Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, Speed * Time.deltaTime);
//         _rigidbody.MovePosition(nextPosition);
//
//         if (direction != Vector3.zero)
//         {
//             _rigidbody.MoveRotation(Quaternion.LookRotation(direction.normalized));
//         }
//     }
//     else
//     {
//         return true; // 다음 목적지로 바로 넘어가도록 처리
//     }
//
//     return Vector3.Distance(currentPosition, destination) <= 0.5f;
// }