using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStateActor : CMovableStatePototype
{
    protected CActorMemoryShare m_MyActorMemoryShare = null;

    public CStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        if (pamMovableBase == null)
            return;

        m_MyActorMemoryShare = (CActorMemoryShare)pamMovableBase.MyMemoryShare;       
    }

    public void SetAnimationState(CAnimatorStateCtl.EState AniState, float Speed = 1.0f, int index = -1)
    {
        if (m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl != null)
        {
            m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl.SetCurState(AniState, index);
            m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl.AnimatorSpeed = Speed;
        }
    }

    protected override void OutState()
    {
        base.OutState();

        if (m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl != null)
            m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl.m_KeyFramMessageCallBack = null;
    }
}
