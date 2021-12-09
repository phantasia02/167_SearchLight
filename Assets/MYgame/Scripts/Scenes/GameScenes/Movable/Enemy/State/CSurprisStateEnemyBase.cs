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
        m_MyEnemyBaseMemoryShare.m_WasFound = true;

        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(true);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
