using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CEnemyBaseMemoryShare : CActorMemoryShare
{
    public CEnemyBase                   m_MyEnemyBase               = null;
    public Transform                    m_PlayerLight               = null;
    public CPlayerLightShowMesh[]       m_AllPlayerLightShowMesh    = null;
    public Image[]                      m_AllEmoticons              = null;
};

public abstract class CEnemyBase : CActor
{
    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;


    // ==================== SerializeField ===========================================
    [SerializeField] protected Image[]      m_AllEmoticons          = null;

    // ==================== SerializeField ===========================================

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemyBase(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eHit].AllThisState.Add(new CSurprisStateEnemyBase(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));


        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateBase(this));

        //m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyBaseMemoryShare = new CEnemyBaseMemoryShare();
        m_MyMemoryShare = m_MyEnemyBaseMemoryShare;

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase = this;

       // this.transform.FindChild();

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
