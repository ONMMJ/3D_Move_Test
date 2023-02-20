using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IInteraction
{
    public enum STATE
    {
        Idle,
        Patrol,
        Chase,
        Attack,
    }

    [SerializeField] new string name;
    [SerializeField] Transform uiPivot;     // ui가 출력될 기준점.

    [Header("Event")]
    [SerializeField] UnityEvent onDead;     // 적이 죽었을 때 호출되는 이벤트 함수.

    [Header("Enemy")]
    [SerializeField] STATE state;
    [SerializeField] float patrolRadius;    // 순찰 범위.
    [SerializeField] float searchRadius;    // 탐지 범위.
    [SerializeField] float attackRadius;    // 공격 범위.

    [Header("Movement")]
    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float attackRate;      // 공격 주기.

    public string Name => name;
    Transform IInteraction.transform => uiPivot;
    //public Transform transform => uiPivot;

    bool isChaseToPlayer;       // 플레이어를 추격하는가.
    bool isAttackToPlayer;      // 플레이어를 공격하는가.
    bool isChangedState;        // 상태가 변했는가.
    
    float attackTime;           // 공격 시간.
    float idleTime = 0f;        // 대기 시간.
    Vector3 originPoint;        // 최초 부활 지점.
    Vector3 patrolPoint;        // 정찰 지점.

    STATE beforeState;

    void IInteraction.OnInteraction(GameObject order)
    {
        onDead?.Invoke();
        Destroy(gameObject);
    }

    private void Start()
    {
        idleTime = Random.Range(2f, 5f);
        originPoint = transform.position;
        patrolPoint = transform.position;
    }
    private void Update()
    {
        isChaseToPlayer = Physics.CheckSphere(transform.position, searchRadius, 1 << LayerMask.NameToLayer("Player"));
        isAttackToPlayer = Physics.CheckSphere(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Player"));

        // 상태머신.
        if (isChaseToPlayer && !isAttackToPlayer)
            state = STATE.Chase;
        else if (isChaseToPlayer && isAttackToPlayer)
            state = STATE.Attack;
        else if (Vector3.Distance(transform.position, patrolPoint) > 0)
            state = STATE.Patrol;
        else
            state = STATE.Idle;

        isChangedState = beforeState != state;      // 이전 상태와 현재 상태가 다를 경우.
        beforeState = state;                        // 다음 프레임으로 넘어가기 전에 이전 상태 저장.

        switch (state)
        {
            case STATE.Idle: OnIdle(); break;
            case STATE.Chase: OnChase(); break;
            case STATE.Patrol: OnPatrol(); break;
            case STATE.Attack: OnAttack(); break;
        }
    }

    private void OnIdle()
    {
        // 상태 변화로 인해 대기 상태가 되었을 경우.
        if (isChangedState)
            idleTime = Random.Range(2f, 5f);

        // 3초간 대기하고 있다가 정찰한다.
        idleTime -= Time.deltaTime;
        if (idleTime <= 0f)
        {
            Vector2 randomUnitCircle = Random.insideUnitCircle * patrolRadius;
            patrolPoint = originPoint + new Vector3(randomUnitCircle.x, 0f, randomUnitCircle.y);
        }
    }
    private void OnPatrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoint, moveSpeed * Time.deltaTime);
    }
    private void OnChase()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        playerPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);
    }
    private void OnAttack()
    {
        // 이전 상태에서 공격으로 전환된 타이밍.
        if(isChangedState)
        {
            attackTime = attackRate;
        }

        attackTime -= Time.deltaTime;
        if (attackTime <= 0f)
        {
            attackTime = attackRate;
            Debug.Log("플레이어 공격");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 순찰 범위.
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(originPoint, Vector3.up, patrolRadius);

        // 탐지 범위.
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);

        // 공격 범위.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
}
