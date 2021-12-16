using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CATKStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eAtk; }
    Tween m_RotateTween = null;

    public CATKStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        Vector3 MytoPlayCtrlLightDir = m_MyGameManager.Player.SearchlightTDObj.transform.position - m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;
        MytoPlayCtrlLightDir.Normalize();

        SetAnimationState(CAnimatorStateCtl.EState.eAtk);
        m_RotateTween = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOLookAt(m_MyGameManager.Player.SearchlightTDObj.transform.position, 0.5f, AxisConstraint.Y).SetEase( Ease.Linear);
        m_RotateTween.onComplete = () =>
        {

            Transform lLauncherPointTransform = m_MyEnemyBaseMemoryShare.m_AllOtherTransform[0];
            if (lLauncherPointTransform != null)
            {
                Transform lTempEnemyATKEffect = StaticGlobalDel.NewOtherObjAddParentShow(lLauncherPointTransform, CGGameSceneData.EOtherObj.eEnemyATKEffect);

                Transform lTempSparkEffect = StaticGlobalDel.NewOtherObjAddParentShow(lLauncherPointTransform, CGGameSceneData.EOtherObj.eSpark);
                lTempSparkEffect.up = lLauncherPointTransform.forward;
                lTempSparkEffect.position = lLauncherPointTransform.position;
                lTempSparkEffect.localScale = Vector3.one * 5.0f;
            }
        };
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {
        if (m_RotateTween != null)
            m_RotateTween.Kill();
    }
}
