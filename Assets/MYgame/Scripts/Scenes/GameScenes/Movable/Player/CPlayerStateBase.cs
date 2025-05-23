using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    protected override void updataState()
    {
        base.updataState();
    }


    public void UpdateSpeed()
    {
        // m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_MyTransform.forward * 300.0f * Time.fixedDeltaTime;

        if (m_MyMemoryShare.m_TotleSpeed.Value != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed.Value = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed.Value, m_MyMemoryShare.m_TargetTotleSpeed, 5.0f * Time.deltaTime);

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
        bool lTempCheckIrradiateEnemy = false;
        bool lTempResetSpeed = true;

        for (int i = 0; i < lTempActorBaseListData.m_ActorBaseListData.Count; i++)
        {
            lTempEnemyBase = (CEnemyBase)lTempActorBaseListData.m_ActorBaseListData[i];
            lTempPlayCtrlLightToEnemyV3 = m_MyPlayerMemoryShare.m_PlayCtrlLight.position - lTempEnemyBase.transform.position;
            lTempPlayCtrlLightToEnemyV3.y = 0.0f;
            //if (lTempPlayCtrlLightToEnemyV3.sqrMagnitude < 1.0f)
            //    lTempEnemyBase.SetChangState(EMovableState.eHit);

            if (lTempPlayCtrlLightToEnemyV3.sqrMagnitude < 1.0f)
            {
                bool lTempAddCurDiscoveryTimeOK = lTempEnemyBase.AddCurDiscoveryTime(Time.deltaTime);
                if (lTempAddCurDiscoveryTimeOK)
                    lTempResetSpeed = false;
            }

            if (lTempPlayCtrlLightToEnemyV3.sqrMagnitude < 0.5f)
            {
                if (!lTempEnemyBase.WasFound && !lTempEnemyBase.Hidden && !lTempCheckIrradiateEnemy)
                {
                    lTempEnemyBase.SetChangState( EMovableState.eHit);
                    m_MyPlayerMemoryShare.m_TargetBuffer = lTempEnemyBase;
                    lTempCheckIrradiateEnemy = true;
                   // m_MyPlayerMemoryShare.m_MyMovable.LockChangState = EMovableState.eAtk;
                    m_MyPlayerMemoryShare.m_MyMovable.SetChangState(EMovableState.eAtk);
                }

                if (!m_MyPlayerMemoryShare.m_SpeedDown)
                    HitSpeedDown(true);
            }
        }

        if (lTempResetSpeed && m_MyPlayerMemoryShare.m_SpeedDown)
            HitSpeedDown(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagEnemyBullet)
        {
            CBulletFlyObj lTempBulletFlyObj = other.GetComponentInParent<CBulletFlyObj>();

            m_MyPlayerMemoryShare.m_Hp.Value -= lTempBulletFlyObj.Damages;
        }
    }

    public void HitSpeedDown(bool down)
    {
        if (down)
            m_MyPlayerMemoryShare.m_MyMovable.SetMoveBuff( CMovableBase.ESpeedBuff.eHit, 0.1f);
        else
            m_MyPlayerMemoryShare.m_MyMovable.ResetMoveBuff();


        m_MyPlayerMemoryShare.m_SpeedDown = down;
    }
}
