using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eWin; }
    public override int Priority => 7;

    public CWinStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
