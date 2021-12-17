using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }
    protected Vector3 m_EndPoint = Vector3.zero;

    public CMoveStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Clear();
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eAtk);
        m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eWait);

        ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
        SetAnimationState( CAnimatorStateCtl.EState.eRun);

        m_EndPoint = Vector3.zero;
        Vector3 lTempDir = Random.insideUnitSphere;
        lTempDir.y = 0.0f;
        lTempDir.Normalize();
        m_EndPoint = m_MyEnemyBaseMemoryShare.m_MyActor.transform.position + lTempDir * Random.Range(3.0f, 6.0f);
        m_EndPoint.y = m_MyEnemyBaseMemoryShare.m_MyActor.transform.position.y;

        if (m_EndPoint.x < -StaticGlobalDel.g_CsLightDisMaxX)
            m_EndPoint.x = -StaticGlobalDel.g_CsLightDisMaxX;

        if (m_EndPoint.x > StaticGlobalDel.g_CsLightDisMaxX)
            m_EndPoint.x = StaticGlobalDel.g_CsLightDisMaxX;

        if (m_EndPoint.z > StaticGlobalDel.g_CsLightDisMaxZ)
            m_EndPoint.z = StaticGlobalDel.g_CsLightDisMaxZ;

        if (m_EndPoint.z < StaticGlobalDel.g_CsLightDisMinZ)
            m_EndPoint.z = StaticGlobalDel.g_CsLightDisMinZ;


    }

    protected override void updataState()
    {
        UpdateDiscoveryTime();

        Vector3 lTempDir = m_EndPoint - m_MyEnemyBaseMemoryShare.m_MyActor.transform.position;
        lTempDir.y = 0.0f;
        lTempDir.Normalize();

        m_MyEnemyBaseMemoryShare.m_MyActor.transform.forward = Vector3.Lerp(m_MyEnemyBaseMemoryShare.m_MyActor.transform.forward, lTempDir, 0.5f);
        m_MyEnemyBaseMemoryShare.m_MyActor.transform.Translate(new Vector3(0.0f, 0.0f, m_MyEnemyBaseMemoryShare.m_TotleSpeed.Value * Time.deltaTime));

        Vector3 lTempCheck = m_EndPoint - m_MyEnemyBaseMemoryShare.m_MyActor.transform.position;
        lTempCheck.y = 0.0f;

        if (lTempCheck.sqrMagnitude <= 0.1f)
            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(RandomState(m_MyEnemyBaseMemoryShare.m_MyRandomStateList));
    }

    protected override void OutState()
    {
    }
}
