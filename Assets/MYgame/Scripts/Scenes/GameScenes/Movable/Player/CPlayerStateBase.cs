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


}
