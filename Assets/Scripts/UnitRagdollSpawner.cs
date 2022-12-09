using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform RagdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private Vector3 shootDir;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform RagdollTransform = Instantiate(RagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = RagdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(originalRootBone, shootDir);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetedUnit = shootAction.GetTargetUnit();

                //Debug.Log(shooterUnit + " , " + targetedUnit);

                shootDir = (targetedUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                break;
        }
    }
}
