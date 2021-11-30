using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CEnemyBaseMemoryShare : CActorMemoryShare
{

    public CEnemyBase m_MyEnemyBase = null;
};

public abstract class CEnemyBase : CActor
{
    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;

    // ==================== SerializeField ===========================================


    // ==================== SerializeField ===========================================


    protected override void AddInitState()
    {
     
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyBaseMemoryShare = new CEnemyBaseMemoryShare();
        m_MyMemoryShare = m_MyEnemyBaseMemoryShare;

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase = this;

        base.CreateMemoryShare();

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
