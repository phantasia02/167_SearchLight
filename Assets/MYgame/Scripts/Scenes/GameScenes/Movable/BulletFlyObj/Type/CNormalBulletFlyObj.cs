using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNormalBulletFlyObj : CBulletFlyObj
{
    public override EBulletArms MyBulletArms() { return EBulletArms.eNormalBullet; }

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateBulletFlyObj(this));
    }

    protected override void Start()
    {
        m_Damages = 2;
        base.Start();
    }
}
