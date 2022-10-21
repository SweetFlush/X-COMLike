using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld Instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private LayerMask unitLayerMask;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        transform.position = GetPosition();
    }

    public static Vector3 GetPosition()
    {
        //마우스위치에 물체가 닿았는지
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, Instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
