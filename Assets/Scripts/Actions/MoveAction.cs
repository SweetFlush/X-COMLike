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
    /// gridPosition�� ��ȿ �׸��� ����Ʈ ���� �ִٸ� true
    /// </summary>
    /// <param name="gridPosition">�˻��� �׸���������</param>
    /// <returns></returns>
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    //�̵��� ��ȿ�� �׸��� ����Ʈ�� ����
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        //��ȿ ��Ÿ��� GridPosition�鿡 ���� �ݺ�
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //Grid�� LevelGrid���� ������ Grid�� �ƴ϶�� ��Ƽ��
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    //������ �̹� ���� ������ �ִٸ� �̵����� ����
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //�׸��尡 �̹� �ٸ� ���ֿ� ���� ������
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}

