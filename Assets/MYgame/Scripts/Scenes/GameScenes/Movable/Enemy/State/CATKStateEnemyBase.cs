using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CATKStateEnemyBase : CEnemyStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eAtk; }

    public CATKStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
