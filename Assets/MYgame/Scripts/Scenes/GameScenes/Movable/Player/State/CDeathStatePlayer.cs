using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eDeath; }

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
