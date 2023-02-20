using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NaviPlayer : MonoBehaviour
{
    enum STATE
    {
        Idle,           // 가만히 있다.
        MoveOnly,       // 이동만 하는 중.
        MoveAttack,     // 이동하면서 공격.
        Targetting,     // 정확하게 특정한 적을 타겟팅.
    }

    [SerializeField] float attackRange;

    NavMeshAgent agent;

    STATE state;            // 상태.
    Vector3 destination;    // 목적지.
    Transform target;       // 공격 대상.
    bool isAttackCommand;   // 공격 명령.

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            isAttackCommand = true;

        if (Input.GetMouseButtonDown(0) && isAttackCommand)
        {
            RaycastHit hit = GetRayPoint();
            destination = hit.point;

            // hit대상인지 Enemy인지 판단.
            int targetLayer = 1 << hit.collider.gameObject.layer;
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            if ((targetLayer & enemyLayer) > 0)
            {
                state = STATE.Targetting;
                target = hit.transform;
            }
            else
            {
                state = STATE.MoveAttack;
                target = null;
            }

            isAttackCommand = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = GetRayPoint();
            destination = hit.point;
            isAttackCommand = false;
            target = null;

            state = STATE.MoveOnly;
        }

        switch (state)
        {
            case STATE.Idle:
                Debug.Log("Idle");
                break;
            case STATE.MoveOnly:
                OnMoveOnly();
                Debug.Log("이동");
                break;
            case STATE.MoveAttack:
                OnMoveAttack();
                Debug.Log("공격 이동");
                break;
            case STATE.Targetting:
                Debug.Log("타겟 설정");
                break;
        }
    }

    private void OnMoveOnly()
    {
        agent.SetDestination(destination);      // 목적지 설정.
        Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance <= 0)       // 목적지와의 남은 거리 0이하 일 때
            state = STATE.Idle;                 // 상태 변경.
    }
    private void OnMoveAttack()
    {
        agent.SetDestination(destination);      // 목적지 설정.
        if (Physics.CheckSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy")))
        {
            state = STATE.Targetting;
            // target = 
        }
        else if (agent.remainingDistance <= 0)  // 목적지와의 남은 거리 0이하 일 때
            state = STATE.Idle;                 // 상태 변경.
    }

    private RaycastHit GetRayPoint()
    {
        // 마우스 포지션 위치에 ray를 생성한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue);
        return hit;
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
}
