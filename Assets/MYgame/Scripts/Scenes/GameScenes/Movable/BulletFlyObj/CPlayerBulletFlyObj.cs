using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBulletFlyObj : CBulletFlyObj
{


    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateBulletFlyObj(this));
    }

}
