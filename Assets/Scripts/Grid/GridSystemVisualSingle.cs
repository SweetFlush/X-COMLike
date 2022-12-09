using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridVisual ¥‹¿œ ∞¥√º
/// </summary>
public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
