using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eHit; }
    public override int Priority => 2;

    public CHitStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
