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

        ATKShowObj(CEnemyBase.EATKShowObj.eATKShow);

        Vector3 MytoPlayCtrlLightDir = m_MyGameManager.Player.SearchlightTDObj.transform.position - m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position;
        MytoPlayCtrlLightDir.Normalize();


        m_RotateTween = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.DOLookAt(m_MyGameManager.Player.SearchlightTDObj.transform.position, 0.5f, AxisConstraint.Y).SetEase( Ease.Linear);
        m_RotateTween.onComplete = () =>
        {
            SetAnimationState(CAnimatorStateCtl.EState.eAtk);
            m_MyActorMemoryShare.m_MyActor.AnimatorStateCtl.m_KeyFramMessageCallBack = AnimationATKCallBack;
        };
    }

    protected override void updataState()
    {
        UpdateDiscoveryTime();
    }

    protected override void OutState()
    {
        if (m_RotateTween != null)
            m_RotateTween.Kill();
    }

    public void AnimationATKCallBack(CAnimatorStateCtl.cAnimationCallBackPar Paramete)
    {
        if (Paramete.iIndex == 0)
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
        }
        else if (Paramete.iIndex == 1)
            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(RandomState(m_MyEnemyBaseMemoryShare.m_MyRandomStateList));
    }
}
