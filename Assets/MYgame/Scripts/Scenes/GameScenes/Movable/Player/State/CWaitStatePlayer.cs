using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eWait; }

    public CWaitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 0.0f);
    }

    protected override void updataState()
    {
        CheckIrradiateEnemy();
    }

    protected override void OutState()
    {

    }


    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.UpdateDrag();
        m_MyPlayerMemoryShare.m_MyMovable.SetChangState(EMovableState.eMove); 
    }

}
