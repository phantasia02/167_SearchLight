using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSurprisStateEnemyBase : CEnemyStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eHit; }

    public CSurprisStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
