using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    //카메라 움직임 제어
    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z += -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x += 1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    //카메라 회전 제어
    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    //카메라 확대/축소 제어
    private void HandleZoom() {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
            targetFollowOffset.z += zoomAmount / 5f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
            targetFollowOffset.z -= zoomAmount / 5f;
        }

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        targetFollowOffset.z = Mathf.Clamp(targetFollowOffset.z, -10, -5);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
