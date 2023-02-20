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

    CharacterController controller;     // ĳ���� ��Ʈ�ѷ�.
    LayerMask groundMask;               // ���� ����ũ.
    Vector3 velocity;                   // �߷� ���ӵ�.
    bool isGround;                      // ���� �� �ִ°�?

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
        // ������ ���ٸ� �������� �ʴ´�.
        if (dir == Vector3.zero)
            return;

        controller.Move(dir * moveSpeed * Time.deltaTime);      // ���� * �ӵ� = �̵�.
    }
    public void Jump()
    {
        if (!isGround)
            return;

        // ���� ���Ŀ� ���� y�� �ӵ� ����.
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
    }
    private void Gravity()
    {
        if (velocity.y < 0 && isGround)
            velocity.y = -6f;

        velocity.y += GRAVITY * Time.deltaTime;         // �߷� ���ӵ�.
        controller.Move(velocity * Time.deltaTime);     // �߷¿� ���� �̵�.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 pivot = transform.position + checkGroundOffset;
        Gizmos.DrawWireSphere(pivot, checkGroundRadius);
    }
}
