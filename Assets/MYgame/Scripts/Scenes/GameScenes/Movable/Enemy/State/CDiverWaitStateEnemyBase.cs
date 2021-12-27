using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDiverWaitStateEnemyBase : CWaitStateEnemyBase
{


    public CDiverWaitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        Vector3 lTempMyPosition = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;
        
        if (lTempMyPosition.y > CEnemyDiver.CsYDifMoveBuff)
            m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOMoveY(CEnemyDiver.CsYDifMove, 1.0f).SetEase(Ease.Linear);

        base.InState();
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {

    }
}
