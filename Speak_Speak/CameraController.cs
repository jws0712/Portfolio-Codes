// # System
using System.Collections;
using System.Collections.Generic;

// # UnityEngine
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("CameraSetting")]
    [SerializeField] private float rotSensitive     = default;
    [SerializeField] private float dis              = default;
    [SerializeField] private float RotationMin      = default;
    [SerializeField] private float RotationMax      = default;

    [HideInInspector] public Transform target = null;

    private Camera mainCamera = null;

    private float Yaxis = default;
    private float Xaxis = default;
    private Vector3 targetRotation = default;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsGameOver || target == null) return;

        CameraMovement();
    }

    /// <summary>
    /// 마우스에 따라 카메라를 움직이는 함수
    /// </summary>
    private void CameraMovement()
    {
        Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
        Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        targetRotation = new Vector3(Xaxis, Yaxis);

        transform.eulerAngles = targetRotation;
        transform.position = target.position - transform.forward * dis;

        mainCamera.transform.LookAt(target.position);
    }
}
