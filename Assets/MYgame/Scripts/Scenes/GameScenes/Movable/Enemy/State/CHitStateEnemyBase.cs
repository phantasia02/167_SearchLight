using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eHit; }

    public CHitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_MyActor.AddBuff(CMovableBuffPototype.EMovableBuff.eSurpris);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = false;
    }
}
