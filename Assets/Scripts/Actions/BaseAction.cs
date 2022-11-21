using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    //��ư�� �� ���� ����
    public abstract string GetActionName();
    //�׼� ���� �� ȣ��Ǵ� �Լ�
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    /// <summary>
    /// gridPosition�� ��ȿ �׸��� ����Ʈ ���� �ִٸ� true
    /// </summary>
    /// <param name="gridPosition">�˻��� �׸���������</param>
    /// <returns></returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost()
    {
        //default action cost
        return 1;
    }
}
