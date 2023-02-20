using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] Transform body;            // ��� ��ü.
    [SerializeField] Transform camPivot;        // ī�޶� ���� ��.
    [SerializeField] Transform cam;             // ī�޶�.

    [Range(0.5f, 3.0f)]
    [SerializeField] float sencitivityX;        // ���� ����.
    [Range(0.5f, 3.0f)]
    [SerializeField] float sencitivityY;        // ���� ����.

    float rotateX;

    private void Start()
    {
        // None : ���.
        // Locked : ���콺�� �߾����� �����ȴ�.
        // Confined : ���콺 ���α�.
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void RotateHorizontal(float x)
    {
        body.Rotate(Vector3.up * x * sencitivityX);
    }
    public void RotateVertical(float y)
    {
        rotateX -= (y * sencitivityX);
        rotateX = Mathf.Clamp(rotateX, -20f, 50f);
        camPivot.rotation = Quaternion.Euler(rotateX, camPivot.eulerAngles.y, 0);
    }
    public void CameraZoom(float zoom)
    {
        if (zoom == 0f)
            return;

        // zoom���� �۱� ������ ����.
        zoom = (zoom < 0) ? -1f : 1f;

        float distance = Vector3.Distance(cam.position, camPivot.position);
        distance = Mathf.Clamp(distance - zoom, 2f, 10f);

        Vector3 direction = (cam.position - camPivot.position).normalized;
        cam.position = camPivot.position + (direction * distance);
    }
}
