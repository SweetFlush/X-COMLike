using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone)
    {
        Vector3 hitDirection = UnitActionSystem.Instance.GetDirectionBetweenTwoUnit(transform);
        Debug.Log(hitDirection);
        MathAllChildTransforms(originalRootBone, ragdollRootBone);

        ApplyExplosionToRagdoll(ragdollRootBone, 500f, transform.position - hitDirection, 10f);
    }

    /// <summary>
    /// �ڽ� ��ü���� ��ġ�� ��Ȯ�� �����ִ� ����Լ�
    /// </summary>
    /// <param name="root">�̹��� ��ġ�� �о�� ��ü</param>
    /// <param name="clone">��ġ�� ������ ���׵� �������� ������</param>
    private void MathAllChildTransforms(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(clone != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MathAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
