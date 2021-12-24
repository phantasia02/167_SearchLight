using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyRifle : CEnemyBase
{
    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyRifle; }
    public override float DefSpeed { get { return 1.0f; } }

    protected override void AddInitState()
    {
        base.AddInitState();
      
        //m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemyBase(this));
    }
}
