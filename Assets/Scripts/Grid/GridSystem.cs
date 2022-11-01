using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �׸��带 �����ϰ� �Ѱ��ϴ� ��ũ��Ʈ
/// </summary>
public class GridSystem
{
    private int width;    //���μ��� �� ����
    private int height;
    private float cellSize; //����
    private GridObject[,] gridObjectArray;  //�׸��� ��ü�� ��� 2���� �迭

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //�� ������ŭ �׸��������Ʈ�� �����ؼ� �迭�� �Ҵ�
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

    //�׸��尡 �����ƴ��� ���⽱�� �׸��帶�� ��ü�� �������ִ� �Լ�
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //x�� z�� ���� ���� ����ü�� ����
                GridPosition gridPosition = new GridPosition(x, z);
                //gridPosition�� ��ǥ�� �ش��ϴ� ���� ��ġ�� debugTransform ����
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                //gridDebugObject�� ��ũ��Ʈ���� SetGridObject ����
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
    /// ��ȿ�� GridPosition���� bool�� ����
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        //(0,0)���� ũ�ų� ����, �׸����� ������ width�� height���� ������ true
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
