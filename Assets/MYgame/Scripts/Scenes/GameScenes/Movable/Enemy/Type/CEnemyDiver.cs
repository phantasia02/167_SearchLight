using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyDiver : CEnemyBase
{
    public const float CsYDifMove = -0.6f;
    public const float CsYDifMoveBuff = CsYDifMove + 0.1f;

    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyDiver; }
    public override float DefSpeed { get { return 1.0f; } }

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CDiverWaitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CDiverATKStateEnemyBase(this));

        //m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateEnemyBase(this));
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CDiverMoveStateEnemyBase(this));


        m_AllCreateList[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => { return new CEnemySurprisBuff(this); };
    }
}
