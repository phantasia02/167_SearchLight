using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStatePlayer : CPlayerStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eWait; }

    public CWaitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_MyMovable.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 0.0f);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }


    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.UpdateDrag();
        m_MyPlayerMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
    }

}
