using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFleeStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eFlee; }
    public override int Priority => 4;

    public CFleeStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
