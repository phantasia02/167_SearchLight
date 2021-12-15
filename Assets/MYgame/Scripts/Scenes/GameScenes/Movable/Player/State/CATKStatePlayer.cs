using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CATKStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eAtk; }

    public CATKStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        //EnabledCollisionTag(false);
    }

    protected override void updataState()
    {
        if (m_MyPlayerMemoryShare.m_TargetBuffer == null)
            return;

        if (MomentinTime(0.3f))
        {
            void Launcher()
            {
                for (int i = 0; i < m_MyPlayerMemoryShare.m_AllPlayerFortData.Length; i++)
                {
                    Transform lTempBulletFlyObjTransform = StaticGlobalDel.NewOtherObjAddParentShow(m_MyMemoryShare.m_MyMovable.transform, CGGameSceneData.EOtherObj.eBulletFlyObj);
                    lTempBulletFlyObjTransform.parent = m_MyGameManager.AllBulletParent;
                    lTempBulletFlyObjTransform.position = m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_LauncherPoint.position;
                    lTempBulletFlyObjTransform.forward = m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_LauncherPoint.forward;

                    Transform lTempSparkTransform = StaticGlobalDel.NewOtherObjAddParentShow(m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_LauncherPoint, CGGameSceneData.EOtherObj.eSpark);
                    lTempSparkTransform.parent = m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_LauncherPoint;
                    lTempSparkTransform.localPosition = Vector3.zero;
                    lTempSparkTransform.up = m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_LauncherPoint.forward;
                    lTempSparkTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    CBulletFlyObj lTempBulletFlyObj = lTempBulletFlyObjTransform.GetComponent<CBulletFlyObj>();
                    lTempBulletFlyObj.Target = m_MyPlayerMemoryShare.m_TargetBuffer.transform;
                    lTempBulletFlyObj.SetChangState(EMovableState.eMove);
                }
            }


            Sequence TempSequence = DOTween.Sequence();
            TempSequence.AppendCallback(Launcher);
            TempSequence.AppendInterval(0.2f);
            TempSequence.AppendCallback(Launcher);
            TempSequence.AppendInterval(0.2f);
            TempSequence.AppendCallback(Launcher);
            TempSequence.AppendInterval(0.2f);
            TempSequence.AppendCallback(Launcher);
            TempSequence.AppendCallback(() => { m_MyPlayerMemoryShare.m_MyMovable.SetChangState(EMovableState.eWait); });
        }
        else
        {
            Vector3 lSetPlayCtrlLightpos = Vector3.Lerp(m_MyPlayerMemoryShare.m_PlayCtrlLight.position, m_MyPlayerMemoryShare.m_TargetBuffer.transform.position, Time.deltaTime * 10.0f);
            lSetPlayCtrlLightpos.y = m_MyPlayerMemoryShare.m_PlayCtrlLight.position.y;
            m_MyPlayerMemoryShare.m_PlayCtrlLight.position = lSetPlayCtrlLightpos;
        }
    }

    protected override void OutState()
    {
        m_MyPlayerMemoryShare.m_TargetBuffer = null;
    }
}
