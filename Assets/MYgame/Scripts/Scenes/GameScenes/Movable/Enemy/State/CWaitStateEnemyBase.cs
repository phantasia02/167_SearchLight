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
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Clear();
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eAtk);
      //  m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eMove);

        SetAnimationState(CAnimatorStateCtl.EState.eIdle);
        m_MyEnemyBaseMemoryShare.m_StateTime = Random.Range(1.0f, 2.0f);
        ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
    }

    protected override void updataState()
    {
        UpdateDiscoveryTime();

        if (MomentinTime(m_MyEnemyBaseMemoryShare.m_StateTime))
            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(RandomState(m_MyEnemyBaseMemoryShare.m_MyRandomStateList));
    }

    protected override void OutState()
    {

    }
}
