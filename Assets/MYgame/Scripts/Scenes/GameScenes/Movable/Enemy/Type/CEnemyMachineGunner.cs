using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyMachineGunner : CEnemyBase
{
    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyMachineGunner; }
    public override float DefSpeed { get { return 1.0f; } }

    protected override void AddInitState()
    {
        base.AddInitState();
        //m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateEnemyBase(this));
    }
}
