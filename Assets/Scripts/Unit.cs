using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;

    private Vector3 targetPosition;

    private Animator unitAnimator;

    private void Start()
    {
        unitAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float stoppingDistance = 0.05f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

            unitAnimator.SetBool("isRunning", true);
        }
        else
        {
            unitAnimator.SetBool("isRunning", false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
