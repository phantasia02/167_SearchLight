using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CEnemyBaseMemoryShare : CActorMemoryShare
{
    public CEnemyBase                   m_MyEnemyBase               = null;
    public Transform                    m_PlayerLight               = null;
    public CPlayerLightShowMesh[]       m_AllPlayerLightShowMesh    = null;
};

public abstract class CEnemyBase : CActor
{
    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;


    // ==================== SerializeField ===========================================
    [SerializeField] protected Renderer     m_MyBodeMeshRenderer = null;
    [SerializeField] protected Renderer     m_MyArmsMeshRenderer = null;

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

        m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh = this.GetComponentsInChildren<CPlayerLightShowMesh>();

        foreach (CPlayerLightShowMesh CPLSM in m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh)
            CPLSM.PlayerLight = m_MyGameManager.Player.PlayCtrlLight;
    }
}
