using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINT_MAX = 2;

    //턴 변경이 발생했을 때, Unit보다 UI가 먼저 이벤트 처리가 발생하면 UI에서는 유닛의 갱신안된 턴 수인 0이 출력되는 오류를 방지하기 위해 따로 만듦
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;

    /* GridPosition 로직을 unit에 남겨두는 이유는 MoveAction이외에도 넉백이나 텔포 등 다른 방법으로 유닛을 움직여야 할 수 있기 때문*/
    private GridPosition gridPosition;  //내가 어느 grid에 위치해있는지 담음
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
        //내 위치가 그리드의 어느 위치인지 알아내어 gridPosition에 할당
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoint()
    {
        return actionPoint;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        //적이고 적 턴이면 액션포인트 초기화, 아군이고 아군 턴이면 액션포인트 초기화
        if(isEnemy && !TurnSystem.Instance.IsPlayerTurn() || (!isEnemy && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoint = ACTION_POINT_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsEnemy()
    {
        return isEnemy;
    }

}
