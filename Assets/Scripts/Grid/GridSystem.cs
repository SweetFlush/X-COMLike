using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 그리드를 생성하고 총괄하는 스크립트
/// </summary>
public class GridSystem
{
    private int width;    //가로세로 셀 개수
    private int height;
    private float cellSize; //높이
    private GridObject[,] gridObjectArray;  //그리드 객체를 담는 2차원 배열

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //셀 개수만큼 그리드오브젝트를 생성해서 배열에 할당
        gridObjectArray = new GridObject[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }

    //그리드가 생성됐는지 보기쉽게 그리드마다 객체를 생성해주는 함수
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //x와 z의 값을 담은 구조체를 생성
                GridPosition gridPosition = new GridPosition(x, z);
                //gridPosition의 좌표에 해당하는 월드 위치에 debugTransform 생성
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                //gridDebugObject의 스크립트에서 SetGridObject 실행
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    /// <summary>
    /// 유효한 GridPosition인지 bool값 리턴
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        //(0,0)보다 크거나 같고, 그리드의 개수인 width나 height보다 작으면 true
        return (gridPosition.x >= 0 && gridPosition.z >= 0) &&
               (gridPosition.x < width && gridPosition.z < height);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
