using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eWait; }


    public CWaitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
       
    }

    protected override void updataState()
    {
        if (MomentinTime(1.0f))
        {
            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(EMovableState.eAtk);
        }
    }

    protected override void OutState()
    {

    }
}
