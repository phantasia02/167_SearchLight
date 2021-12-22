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
    
        //EnabledCollisionTag(false);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
