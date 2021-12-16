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
            Vector3 LaunchtoPlayCtrlLight = m_MyGameManager.Player.SearchlightTDObj.transform.position - m_MyEnemyBaseMemoryShare.m_AllOtherTransform[0].position;
            Vector3 LaunchtoPlayCtrlLightDir = LaunchtoPlayCtrlLight.normalized;

            Transform lLauncherPointTransform = m_MyEnemyBaseMemoryShare.m_AllOtherTransform[0];
            if (lLauncherPointTransform != null)
            {
                Transform lTempEnemyATKEffect = StaticGlobalDel.NewOtherObjAddParentShow(lLauncherPointTransform, CGGameSceneData.EOtherObj.eEnemyATKEffect);
                lTempEnemyATKEffect.parent = m_MyGameManager.AllBulletParent;
                lTempEnemyATKEffect.forward = LaunchtoPlayCtrlLightDir;
                lTempEnemyATKEffect.localScale = new Vector3(1.0f, 1.0f, LaunchtoPlayCtrlLight.magnitude);
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
