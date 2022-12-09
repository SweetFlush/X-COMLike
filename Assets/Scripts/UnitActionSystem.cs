using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set;}

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        //�׼� ���̸� �Է� ����
        if (isBusy) return;
        //���콺�� UI���� �ö� ������ �Է� ����
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //������ ������ ������ �Է� ����
        if (TryHandleUnitSelection()) return;
        //���� ���̸� �Է� ����
        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        //�Է� �޾� ����, �̵�
        HandleSelectedAction();
    }
    
    //�׼� ����
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            //���� �׼�����Ʈ�� ����� ������ ����
            if(selectedUnit.TrySpendActionPointToTakeAction(selectedAction))
            {
                //gridPosition�� ��ȿ�ϸ� �׼� ����
                if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    //Debug.Log(selectedAction.GetActionPointCost().ToString());
                    //������ ������ �׼�����Ʈ �Ҹ�
                    selectedUnit.SpendActionPoint(selectedAction.GetActionPointCost());
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);

                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
            }
            
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    //���콺 Ŭ������ ���õ� ������ �ٲ�
    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0)) {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        // ������ �̹� ���õ�
                        return false;
                    }
                    //���̸� ������������
                    if(unit.IsEnemy())
                    {
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    //���õ� ������ �ٲ�
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        //���� �ٲ����� �̺�Ʈ fire
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        //�׼� �ٲ����� �̺�Ʈ Fire
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    /// <summary>
    /// ���׵��� ���� ���� ������ �ֱ� ���� �� ���ְ� ���� ������ ���⺤�͸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="unitTransform">���� ����</param>
    /// <returns></returns>
    public Vector3 GetDirectionBetweenTwoUnit(Transform unitTransform)
    {
        return (unitTransform.position - selectedUnit.transform.position).normalized;
    }
}
