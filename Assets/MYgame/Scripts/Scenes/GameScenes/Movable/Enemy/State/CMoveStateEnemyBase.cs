using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }

    public CMoveStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
