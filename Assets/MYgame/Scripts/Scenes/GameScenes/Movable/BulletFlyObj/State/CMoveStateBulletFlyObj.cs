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

    }

    protected override void updataState()
    {
        if (m_MyBulletFlyObjMemoryShare.m_Target == null)
            Debug.LogError("Null m_Target ");

        Vector3 lTempDir = m_MyBulletFlyObjMemoryShare.m_Target.position - m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.position;
        //lTempDir.y = 0.0f;
        lTempDir.Normalize();

        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.forward = Vector3.Lerp(m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.forward, lTempDir, 20.0f * Time.deltaTime);
        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.Translate(new Vector3(0.0f, 0.0f, m_MyBulletFlyObjMemoryShare.m_TotleSpeed.Value * Time.deltaTime));
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagEnemy)
        {
            GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable);
        }
    }
}
