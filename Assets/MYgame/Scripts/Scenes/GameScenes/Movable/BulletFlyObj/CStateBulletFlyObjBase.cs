using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStateBulletFlyObjBase : CMovableStatePototype
{
    protected CBulletFlyObjMemoryShare m_MyBulletFlyObjMemoryShare = null;
    protected CActor m_TargetActor = null;

    public CStateBulletFlyObjBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyBulletFlyObjMemoryShare = (CBulletFlyObjMemoryShare)m_MyMemoryShare;
        
    }
}
