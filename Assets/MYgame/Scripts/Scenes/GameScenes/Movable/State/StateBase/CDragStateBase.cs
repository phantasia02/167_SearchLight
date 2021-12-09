using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDragStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eDrag; }

    public CDragStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
