using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CATKStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eAtk; }
    Tween m_RotateTween = null;
    //protected Sequence m_TempSequence = null;

    public CATKStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Clear();
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eMove);
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eWait);

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase.ATKShowObj(CEnemyBase.EATKShowObj.eATKShow);

        Vector3 MytoPlayCtrlLightDir = m_MyGameManager.Player.SearchlightTDObj.transform.position - m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;
        MytoPlayCtrlLightDir.Normalize();

        m_RotateTween = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOLookAt(m_MyGameManager.Player.SearchlightTDObj.transform.position, 0.5f, AxisConstraint.Y).SetEase( Ease.Linear);
        m_RotateTween.onComplete = () =>
        {
            SetAnimationState(CAnimatorStateCtl.EState.eAtk, 1, 0);
            m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl.m_KeyFramMessageCallBack = NormalAnimationATKCallBack;
            m_MyEnemyBaseMemoryShare.m_MyEnemyBase.Hidden = false;
        };
    }

    protected override void updataState()
    {
        UpdateDiscoveryTime();
    }

    protected override void OutState()
    {
        m_MyEnemyBaseMemoryShare.m_MyEnemyBase.Hidden = true;
        if (m_RotateTween != null)
            m_RotateTween.Kill();
    }

   
}
