using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform RagdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform RagdollTransform = Instantiate(RagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = RagdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(originalRootBone);
    }
}
