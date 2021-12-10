using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CEnemyBuffBase : CMovableBuffPototype
{
    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;

    public CEnemyBuffBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyEnemyBaseMemoryShare = (CEnemyBaseMemoryShare)m_MyMemoryShare;
    }
}
