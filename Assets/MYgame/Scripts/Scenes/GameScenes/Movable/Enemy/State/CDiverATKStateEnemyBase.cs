using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDiverATKStateEnemyBase : CATKStateEnemyBase
{
    public override EMovableState StateType() { return EMovableState.eAtk; }
    Tween m_RotateTween = null;


    public CDiverATKStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
    }

    protected override void InState()
    {
        Vector3 lTempMyPosition = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;

        if (lTempMyPosition.y <= -0.3f)
            m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOMoveY(-0.2f, 1.0f).SetEase(Ease.Linear);

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase.Hidden = false;
        base.InState();

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase.ShowOther1(false);
    }

    protected override void updataState()
    {
        UpdateDiscoveryTime();
    }

    protected override void OutState()
    {
        base.OutState();

        if (m_MyEnemyBaseMemoryShare.m_MyEnemyBase.CurState != EMovableState.eHit)
            m_MyEnemyBaseMemoryShare.m_MyEnemyBase.ShowOther1(true);
    }
}
