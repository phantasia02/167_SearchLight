using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFinishStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eFinish; }

    public CFinishStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
    }

    protected override void updataState()
    {
    }

    protected override void OutState()
    {
    }
}
