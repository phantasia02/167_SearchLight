using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGrenadeBulletFlyObj : CBulletFlyObj
{
    public override EBulletArms MyBulletArms() { return EBulletArms.eGrenade; }

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CGrenadeMoveStateBulletFlyObj(this));
    }

    protected override void Start()
    {
        m_Damages = 10;
        base.Start();
    }
}
