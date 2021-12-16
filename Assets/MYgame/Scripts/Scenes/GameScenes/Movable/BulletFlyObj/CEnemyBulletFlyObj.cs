using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyBulletFlyObj : CBulletFlyObj
{
    // ==================== SerializeField ===========================================

    [SerializeField] protected AnimationCurve m_curve;
    public AnimationCurve curve => m_curve;

    // ==================== SerializeField ===========================================

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemyBulletFlyObj(this));
    }


    protected override void Start()
    {
        base.Start();
        SetChangState(CMovableStatePototype.EMovableState.eAtk);
    }
}
