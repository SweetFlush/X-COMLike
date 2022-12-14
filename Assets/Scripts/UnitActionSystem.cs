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
        //액션 중이면 입력 무시
        if (isBusy) return;
        //마우스가 UI위에 올라가 있으면 입력 무시
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //선택한 유닛이 없으면 입력 무시
        if (TryHandleUnitSelection()) return;
        //적군 턴이면 입력 무시
        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        //입력 받아 선택, 이동
        HandleSelectedAction();
    }
    
    //액션 제어
    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            //유닛 액션포인트가 충분히 있으면 수행
            if(selectedUnit.TrySpendActionPointToTakeAction(selectedAction))
            {
                //gridPosition이 유효하면 액션 수행
                if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    //Debug.Log(selectedAction.GetActionPointCost().ToString());
                    //선택한 유닛의 액션포인트 소모
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

    //마우스 클릭으로 선택된 유닛을 바꿈
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
                        // 유닛이 이미 선택됨
                        return false;
                    }
                    //적이면 선택하지않음
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

    //선택된 유닛을 바꿈
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        //유닛 바꿨으니 이벤트 fire
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        //액션 바꿨으니 이벤트 Fire
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
    /// 래그돌에 힘을 가할 방향을 주기 위해 쏜 유닛과 맞은 유닛의 방향벡터를 리턴하는 함수
    /// </summary>
    /// <param name="unitTransform">맞은 유닛</param>
    /// <returns></returns>
    public Vector3 GetDirectionBetweenTwoUnit(Transform unitTransform)
    {
        return (unitTransform.position - selectedUnit.transform.position).normalized;
    }
}
