using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINT_MAX = 2;

    //�� ������ �߻����� ��, Unit���� UI�� ���� �̺�Ʈ ó���� �߻��ϸ� UI������ ������ ���žȵ� �� ���� 0�� ��µǴ� ������ �����ϱ� ���� ���� ����
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;

    private HealthSystem healthSystem;

    /* GridPosition ������ unit�� ���ܵδ� ������ MoveAction�̿ܿ��� �˹��̳� ���� �� �ٸ� ������� ������ �������� �� �� �ֱ� ����*/
    private GridPosition gridPosition;  //���� ��� grid�� ��ġ���ִ��� ����
    private MoveAction moveAction;
    private SpinAction spinAction;

    private BaseAction[] baseActionArray;

    private int actionPoint = 2;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        //�� ��ġ�� �׸����� ��� ��ġ���� �˾Ƴ��� gridPosition�� �Ҵ�
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //�����׸��忡 �� ��ġ�� ���
        LevelGrid.Instance.AddUnitGridPosition(gridPosition, this);

        //�̺�Ʈ ����
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
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

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointToTakeAction(baseAction))
        {
            //SpendActionPoint(baseAction.GetActionPointCost());
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

    public void SpendActionPoint(int amount)
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
        //���̰� �� ���̸� �׼�����Ʈ �ʱ�ȭ, �Ʊ��̰� �Ʊ� ���̸� �׼�����Ʈ �ʱ�ȭ
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

    public void DealDamage(int damageAmount)
    {
        healthSystem.DealDamage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

}
