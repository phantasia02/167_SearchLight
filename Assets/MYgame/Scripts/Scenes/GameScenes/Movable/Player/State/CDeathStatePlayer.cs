using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStatePlayer : CPlayerStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eDeath; }

    public CDeathStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        //EnabledCollisionTag(false);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
