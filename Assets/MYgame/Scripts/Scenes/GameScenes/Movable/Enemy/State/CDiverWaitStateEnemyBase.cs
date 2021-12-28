using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDiverWaitStateEnemyBase : CWaitStateEnemyBase
{
    Tween m_TweenBuff = null;

    public CDiverWaitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        Vector3 lTempMyPosition = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;

        
        m_TweenBuff = null;
        if (lTempMyPosition.y > CEnemyDiver.CsYDifMoveBuff)
        {
            m_TweenBuff = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOMoveY(CEnemyDiver.CsYDifMove, 1.0f).SetEase(Ease.Linear);

        }


        base.InState();
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        if (m_TweenBuff != null)
            m_TweenBuff.Kill();
    }
}
