using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eDeath; }
    public override int Priority => 10;

    public CDeathStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_TagBox.gameObject.SetActive(false);

        foreach (CPlayer.CDateSearchlightSmoke DSLS in m_MyPlayerMemoryShare.m_AllDateSearchlightSmoke)
            DSLS.m_SmokeSearchlightLEVELObj.SetActive(false);

        Vector3 lTempExplosion = m_MyPlayerMemoryShare.m_MyActor.AnimatorStateCtl.transform.position;
        lTempExplosion.y -= 2.0f;
       // lTempExplosion.z -= 2.0f;
        m_MyPlayerMemoryShare.m_MyActor.EnabledRagdoll(true);
        m_MyPlayerMemoryShare.m_MyActor.AddRagdolldForce(1000.0f, lTempExplosion, 50.0f);

        m_MyPlayerMemoryShare.m_PlayCtrlLight.gameObject.SetActive(false);
        m_MyPlayerMemoryShare.m_LightTDObj.SetActive(false);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
