using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CEnemyStateBase : CStateActor
{
    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;

    public CEnemyStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyEnemyBaseMemoryShare = (CEnemyBaseMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
    }


    public void UpdateSpeed()
    {
        // m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_MyTransform.forward * 300.0f * Time.fixedDeltaTime;

        //if (m_MyMemoryShare.m_TotleSpeed.Value != m_MyMemoryShare.m_TargetTotleSpeed)
        //{
        //    m_MyMemoryShare.m_TotleSpeed.Value = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed.Value, m_MyMemoryShare.m_TargetTotleSpeed, 3.0f * Time.deltaTime);

        //    if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed.Value - m_MyMemoryShare.m_TargetTotleSpeed) < 0.001f)
        //        m_MyMemoryShare.m_TotleSpeed.Value = m_MyMemoryShare.m_TargetTotleSpeed;
        //}
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagBullet)
        {
            m_MyEnemyBaseMemoryShare.m_MyActor.EnabledRagdoll(true);

            Vector3 lBulletFlyObjToEnemy = m_MyEnemyBaseMemoryShare.m_MyMovable.transform.position - other.transform.position;
            lBulletFlyObjToEnemy.Normalize();
            Vector3 lTempV3 = other.transform.position;
            lTempV3.y = -1.0f;
            lTempV3.z -= lBulletFlyObjToEnemy.z * 2.0f;
            lTempV3.x -= lBulletFlyObjToEnemy.x * 2.0f;
            m_MyEnemyBaseMemoryShare.m_MyActor.AddRagdolldForce(100.0f, lTempV3, 50.0f);

            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState( EMovableState.eDeath);
            //GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable);
        }
    }
}
