using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;

    private Vector3 targetPosition;
    private GridPosition gridPosition;  //내가 어느 grid에 위치해있는지 담음

    private Animator unitAnimator;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        unitAnimator = GetComponentInChildren<Animator>();

        //내 위치가 그리드의 어느 위치인지 알아내어 gridPosition에 할당
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitGridPosition(gridPosition, this);
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != gridPosition)
        {
            //moved
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
