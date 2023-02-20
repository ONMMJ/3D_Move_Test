using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement3D : MonoBehaviour
{
    float GRAVITY => -9.81f * gravityScale;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityScale;

    [Header("Check Ground")]
    [SerializeField] float checkGroundRadius;
    [SerializeField] Vector3 checkGroundOffset;

    CharacterController controller;     // 캐릭터 컨트롤러.
    LayerMask groundMask;               // 지면 마스크.
    Vector3 velocity;                   // 중력 가속도.
    bool isGround;                      // 땅에 서 있는가?

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }   

    private void Update()
    {
        // Check Ground.
        Vector3 pivot = transform.position + checkGroundOffset;
        isGround = Physics.CheckSphere(pivot, checkGroundRadius, groundMask);

        Gravity();
    }

    public void Move(Vector3 dir)
    {
        // 방향이 없다면 실행하지 않는다.
        if (dir == Vector3.zero)
            return;

        controller.Move(dir * moveSpeed * Time.deltaTime);      // 방향 * 속도 = 이동.
    }
    public void Jump()
    {
        if (!isGround)
            return;

        // 물리 공식에 의한 y축 속도 변경.
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
    }
    private void Gravity()
    {
        if (velocity.y < 0 && isGround)
            velocity.y = -6f;

        velocity.y += GRAVITY * Time.deltaTime;         // 중력 가속도.
        controller.Move(velocity * Time.deltaTime);     // 중력에 의한 이동.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 pivot = transform.position + checkGroundOffset;
        Gizmos.DrawWireSphere(pivot, checkGroundRadius);
    }
}
