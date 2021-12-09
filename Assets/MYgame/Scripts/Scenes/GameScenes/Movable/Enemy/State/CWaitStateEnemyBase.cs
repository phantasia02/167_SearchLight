using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStateEnemyBase : CEnemyStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eWait; }

    public CWaitStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
