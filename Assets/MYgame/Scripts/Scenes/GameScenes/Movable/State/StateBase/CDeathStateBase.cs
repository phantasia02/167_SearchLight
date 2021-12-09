using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eDeath; }
    public override int Priority => 10;

    public CDeathStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
