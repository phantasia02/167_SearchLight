using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eWait; }

    public CWaitStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
