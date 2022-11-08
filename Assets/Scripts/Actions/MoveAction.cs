using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int maxMoveDistance = 4;

    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;
    private Vector3 targetPosition;

    private Animator unitAnimator;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Start()
    {
        unitAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        float stoppingDistance = 0.05f;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            unitAnimator.SetBool("isRunning", true);
        }
        else
        {
            unitAnimator.SetBool("isRunning", false);
            isActive = false;
            onActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    public void Move(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }
    
    /// <summary>
    /// gridPosition이 유효 그리드 리스트 내에 있다면 true
    /// </summary>
    /// <param name="gridPosition">검사할 그리드포지션</param>
    /// <returns></returns>
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    //이동이 유효한 그리드 리스트를 리턴
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        //유효 사거리의 GridPosition들에 대해 반복
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Grid가 LevelGrid에서 생성된 Grid가 아니라면 컨티뉴
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    //유닛이 이미 같은 공간에 있다면 이동하지 않음
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //그리드가 이미 다른 유닛에 의해 점유됨
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}

