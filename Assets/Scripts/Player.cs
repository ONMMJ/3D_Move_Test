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
    bool isLockControl;     // 플레이어 제어 불가능.

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
        // 이동.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //inputX = Input.GetAxis("Horizontal");
        //inputY = Input.GetAxis("Vertical");
        SmoothMove(x, ref inputX);
        SmoothMove(y, ref inputY);
        isMove = x != 0f || y != 0f;

        // 2D처럼 시점이 고정된 환경이 아니라
        // 회전이 들어가는 3D에서는
        // 정면을 '절대좌표'로 잡지 말고 '로컬좌표'로 잡아야한다.
        Vector3 forward = transform.forward * y;        // 내 기준 앞쪽으로.
        Vector3 right = transform.right * x;            // 내 기준 오른쪽으로.
        Vector3 dir = (forward + right).normalized;     // 두 벡터를 더한 후 정규화.

        movement.Move(dir);

        // 점프.
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump();
    }
    private void Rotate()
    {
        // 수평 회전.
        float mouseX = Input.GetAxis("Mouse X");
        rotation.RotateHorizontal(mouseX);

        // 수직 회전.
        float mouseY = Input.GetAxis("Mouse Y");
        rotation.RotateVertical(mouseY);

        // 카메라 줌.
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        rotation.CameraZoom(zoom);
    }
    private void Interaction()
    {
        IInteraction interaction = null;

        // 상호작용 대상을 찾는다.
        Collider[] targets = Physics.OverlapSphere(transform.position, interactionRadius);
        if (targets.Length > 0)
        {
            // OrderBy : 오름차순 정렬(Linq)
            // 기준을 거리로 잡았다.
            var find = from target in targets
                       where target.GetComponent<IInteraction>() != null
                       orderby Vector3.Distance(transform.position, target.transform.position)
                       select target;

            if (find.Count() > 0)
                interaction = find.First().GetComponent<IInteraction>();
        }

        // 값의 유무에 따른 처리.
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