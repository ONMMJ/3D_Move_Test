using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] Transform body;            // 대상 몸체.
    [SerializeField] Transform camPivot;        // 카메라 고정 축.
    [SerializeField] Transform cam;             // 카메라.

    [Range(0.5f, 3.0f)]
    [SerializeField] float sencitivityX;        // 수평 감도.
    [Range(0.5f, 3.0f)]
    [SerializeField] float sencitivityY;        // 수직 감도.

    float rotateX;

    private void Start()
    {
        // None : 언락.
        // Locked : 마우스가 중앙으로 고정된다.
        // Confined : 마우스 가두기.
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

        // zoom값이 작기 때문에 조정.
        zoom = (zoom < 0) ? -1f : 1f;

        float distance = Vector3.Distance(cam.position, camPivot.position);
        distance = Mathf.Clamp(distance - zoom, 2f, 10f);

        Vector3 direction = (cam.position - camPivot.position).normalized;
        cam.position = camPivot.position + (direction * distance);
    }
}
