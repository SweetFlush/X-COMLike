using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /* GridPosition ������ unit�� ���ܵδ� ������ MoveAction�̿ܿ��� �˹��̳� ���� �� �ٸ� ������� ������ �������� �� �� �ֱ� ����*/
    private GridPosition gridPosition;  //���� ��� grid�� ��ġ���ִ��� ����
    private MoveAction moveAction;
    private SpinAction spinAction;

    private BaseAction[] baseActionArray;

    private int actionPoint = 2;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        //�� ��ġ�� �׸����� ��� ��ġ���� �˾Ƴ��� gridPosition�� �Ҵ�
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitGridPosition(gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            //moved
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointToTakeAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointCost());
            return true;
        } else
        {
            return false;
        }
    }

    public bool CanSpendActionPointToTakeAction(BaseAction baseAction)
    {
        if(actionPoint >= baseAction.GetActionPointCost())
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void SpendActionPoint(int amount)
    {
        actionPoint -= amount;
    }
}
