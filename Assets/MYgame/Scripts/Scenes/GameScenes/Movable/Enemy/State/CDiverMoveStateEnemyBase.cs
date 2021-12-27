using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDiverMoveStateEnemyBase : CAIStateEnemyBase
{
    public CDiverMoveStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        Vector3 lTempMyPosition = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;

        if (lTempMyPosition.y > -1.9f)
            m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOMoveY(-2.0f, 1.0f).SetEase(Ease.Linear);

        base.InState();
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }
}
