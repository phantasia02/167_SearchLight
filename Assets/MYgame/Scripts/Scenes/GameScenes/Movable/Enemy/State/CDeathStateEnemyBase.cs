using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eDeath; }

    public CDeathStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_MyMovable.ERemoveBuff(CMovableBuffPototype.EMovableBuff.eSurpris);
        m_MyEnemyBaseMemoryShare.m_MyMovable.DestroyScript();
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
