using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyRifle : CEnemyBase
{
    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyRifle; }
    public override float DefSpeed { get { return 1.0f; } }

    // ==================== SerializeField ===========================================

    [SerializeField] protected Transform m_MyBodyDeformationSystem = null;

    // ==================== SerializeField ===========================================

    protected override void AddInitState()
    {
        base.AddInitState();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_MyEnemyBaseMemoryShare.m_MyBodyDeformationSystem = m_MyBodyDeformationSystem;
    }
}
