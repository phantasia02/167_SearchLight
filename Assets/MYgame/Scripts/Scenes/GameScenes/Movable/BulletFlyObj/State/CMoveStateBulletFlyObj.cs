using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStateBulletFlyObj : CStateBulletFlyObjBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }

    public CMoveStateBulletFlyObj(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_TargetActor = m_MyBulletFlyObjMemoryShare.m_Target.GetComponentInParent<CActor>();
        if (m_MyBulletFlyObjMemoryShare.m_Launcher.ObjType() == CGameObjBas.EObjType.ePlayer)
        {
            Vector3 lTempTargetDir = m_MyBulletFlyObjMemoryShare.m_TargetPoint - m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.position;
            lTempTargetDir.Normalize();
            m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.forward = lTempTargetDir;

        }
    }

    protected override void updataState()
    {
        if (m_MyBulletFlyObjMemoryShare.m_Target == null)
            Debug.LogError("Null m_Target ");

        if (m_TargetActor != null)
        {
            if (m_TargetActor.CurState == EMovableState.eDeath)
                m_TargetActor = null;

            Vector3 lTempTargetPos = m_MyBulletFlyObjMemoryShare.m_Target.position;
            Vector3 lTempDir = lTempTargetPos - m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.position;
            lTempDir.Normalize();

            m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.forward = Vector3.Lerp(m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.forward, lTempDir, 20.0f * Time.deltaTime);
        }

        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.Translate(new Vector3(0.0f, 0.0f, m_MyBulletFlyObjMemoryShare.m_TotleSpeed.Value * Time.deltaTime));
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_MyBulletFlyObjMemoryShare.m_TargetTag || other.tag == StaticGlobalDel.TagFloor)
        {
            Transform lTempSparkEffect = StaticGlobalDel.NewOtherObjAddParentShow(m_MyBulletFlyObjMemoryShare.m_MyMovable.transform, CGGameSceneData.EOtherObj.eSpark);
            lTempSparkEffect.parent = null;
            lTempSparkEffect.up = -m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.forward;
            lTempSparkEffect.position = m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.position;
            lTempSparkEffect.localScale = Vector3.one * 5.0f;

            GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable.gameObject);
        }
    }
}
