using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] MeleeAttack attackAble;
    [SerializeField] float interactionRadius;

    CameraRotation rotation;
    Movement3D movement;
    Inventory inven;
    Animator anim;

    float inputX;
    float inputY;
    bool isMove;
    bool isLockControl;     // �÷��̾� ���� �Ұ���.

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rotation = GetComponent<CameraRotation>();
        movement = GetComponent<Movement3D>();
        inven = GetComponent<Inventory>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {/*
        inputX = 0;
        inputY = 0;
*/
        if (!isLockControl)
        {
            Movement();
            Rotate();
            AttackTo();
            Interaction();
        }

        ControlMenu();
    }
    private void LateUpdate()
    {
        anim.SetFloat("x", inputX);
        anim.SetFloat("y", inputY);
        anim.SetBool("isMove", isMove);
    }
    public void AttackTo()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attackAble.OnAttack();
        }
    }
    private void SmoothMove(float dir, ref float value)
    {
        switch (dir)
        {
            case 1:
                value = Mathf.Clamp(value + Time.deltaTime * 5, -1f, 1f);
                break;
            case 0:
                if (value > 0)
                    value = Mathf.Clamp(value - Time.deltaTime * 5, 0, 1f);
                else if (value == 0)
                    value = 0;
                else
                    value = Mathf.Clamp(value + Time.deltaTime * 5, -1f, 0);
                break;
            case -1:
                value = Mathf.Clamp(value - Time.deltaTime * 5, -1f, 1f);
                break;
            default:
                break;
        }
    }
    private void Movement()
    {
        // �̵�.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //inputX = Input.GetAxis("Horizontal");
        //inputY = Input.GetAxis("Vertical");
        SmoothMove(x, ref inputX);
        SmoothMove(y, ref inputY);
        isMove = x != 0f || y != 0f;

        // 2Dó�� ������ ������ ȯ���� �ƴ϶�
        // ȸ���� ���� 3D������
        // ������ '������ǥ'�� ���� ���� '������ǥ'�� ��ƾ��Ѵ�.
        Vector3 forward = transform.forward * y;        // �� ���� ��������.
        Vector3 right = transform.right * x;            // �� ���� ����������.
        Vector3 dir = (forward + right).normalized;     // �� ���͸� ���� �� ����ȭ.

        movement.Move(dir);

        // ����.
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();
    }
    private void Rotate()
    {
        // ���� ȸ��.
        float mouseX = Input.GetAxis("Mouse X");
        rotation.RotateHorizontal(mouseX);

        // ���� ȸ��.
        float mouseY = Input.GetAxis("Mouse Y");
        rotation.RotateVertical(mouseY);

        // ī�޶� ��.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        rotation.CameraZoom(zoom);
    }
    private void Interaction()
    {
        IInteraction interaction = null;

        // ��ȣ�ۿ� ����� ã�´�.
        Collider[] targets = Physics.OverlapSphere(transform.position, interactionRadius);
        if (targets.Length > 0)
        {
            // OrderBy : �������� ����(Linq)
            // ������ �Ÿ��� ��Ҵ�.
            var find = from target in targets
                       where target.GetComponent<IInteraction>() != null
                       orderby Vector3.Distance(transform.position, target.transform.position)
                       select target;

            if (find.Count() > 0)
                interaction = find.First().GetComponent<IInteraction>();
        }

        // ���� ������ ���� ó��.
        if (interaction == null)
        {
            InteractionUI.instance.ClosePanel();
        }
        else
        {
            InteractionUI.instance.OpenPanel(interaction.Name, interaction.transform);
            if (Input.GetKeyDown(KeyCode.F))
            {
                interaction.OnInteraction(gameObject);
            }
        }
    }

    private void ControlMenu()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.Instance.UpdateItem(inven);
            isLockControl = InventoryUI.Instance.SwitchInventory();
            Cursor.lockState = isLockControl ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}



public interface IInteraction
{
    public string Name { get; }
    public Transform transform { get; }

    public void OnInteraction(GameObject order);
}