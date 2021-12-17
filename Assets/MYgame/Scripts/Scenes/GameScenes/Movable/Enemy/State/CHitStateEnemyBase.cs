using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eHit; }
    public override int Priority => 5;


    public CHitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_MyActor.transform.forward = Vector3.back;
        SetAnimationState(CAnimatorStateCtl.EState.eHit);
        m_MyEnemyBaseMemoryShare.m_MyActor.AddBuff(CMovableBuffPototype.EMovableBuff.eSurpris);
    }

    protected override void updataState()
    {
        if (MomentinTime(0.5f))
        {
            m_MyEnemyBaseMemoryShare.m_MyBodyDeformationSystem.forward = -m_MyEnemyBaseMemoryShare.m_MyMovable.transform.forward;
            SetAnimationState(CAnimatorStateCtl.EState.eFlee);
        }
    }

    protected override void OutState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = false;
    }
}
