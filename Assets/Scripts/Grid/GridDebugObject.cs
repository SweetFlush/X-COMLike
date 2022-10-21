using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    private TextMeshPro textMeshPro;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;

        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
