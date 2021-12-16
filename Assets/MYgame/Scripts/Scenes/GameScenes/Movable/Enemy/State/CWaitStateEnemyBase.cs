using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eWait; }

    public EMovableState[] MyRandomStateList = new EMovableState[2];

    public CWaitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        MyRandomStateList[0] = EMovableState.eAtk;
        MyRandomStateList[1] = EMovableState.eMove;
    }

    protected override void InState()
    {
        SetAnimationState(CAnimatorStateCtl.EState.eIdle);
        m_MyEnemyBaseMemoryShare.m_StateTime = Random.Range(1.0f, 2.0f);
        ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
    }

    protected override void updataState()
    {
        if (MomentinTime(m_MyEnemyBaseMemoryShare.m_StateTime))
            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(RandomState(MyRandomStateList));
    }

    protected override void OutState()
    {

    }
}
