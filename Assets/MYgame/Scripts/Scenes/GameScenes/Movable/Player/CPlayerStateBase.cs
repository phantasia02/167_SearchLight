using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPlayerStateBase : CStateActor
{
    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;


    public CPlayerStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
    }


    public void UpdateSpeed()
    {
        // m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_MyTransform.forward * 300.0f * Time.fixedDeltaTime;

        if (m_MyMemoryShare.m_TotleSpeed.Value != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed.Value = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed.Value, m_MyMemoryShare.m_TargetTotleSpeed, 3.0f * Time.deltaTime);

            if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed.Value - m_MyMemoryShare.m_TargetTotleSpeed) < 0.001f)
                m_MyMemoryShare.m_TotleSpeed.Value = m_MyMemoryShare.m_TargetTotleSpeed;
        }
    }

    public void CheckIrradiateEnemy()
    {
        //List<CActor>
        CActorBaseListData lTempActorBaseListData = m_MyGameManager.GetTypeActorBaseListData(CActor.EActorType.eEnemy);
        Vector3 lTempPlayCtrlLightToEnemyV3 = Vector3.zero;
        CEnemyBase lTempEnemyBase = null;

        for (int i = 0; i < lTempActorBaseListData.m_ActorBaseListData.Count; i++)
        {
            lTempEnemyBase = (CEnemyBase)lTempActorBaseListData.m_ActorBaseListData[i];
            lTempPlayCtrlLightToEnemyV3 = m_MyPlayerMemoryShare.m_PlayCtrlLight.position - lTempEnemyBase.transform.position;
            lTempPlayCtrlLightToEnemyV3.y = 0.0f;
            //if (lTempPlayCtrlLightToEnemyV3.sqrMagnitude < 1.0f)
            //    lTempEnemyBase.SetChangState(EMovableState.eHit);

            if (lTempPlayCtrlLightToEnemyV3.sqrMagnitude < 1.0f)
                lTempEnemyBase.AddBuff( CMovableBuffPototype.EMovableBuff.eSurpris);

        }

    }
}
