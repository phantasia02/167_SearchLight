using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CEnemyBaseListData
{
    public List<CEnemyBase> m_EnemyBaseListData = new List<CEnemyBase>();
}

[System.Serializable]
public class CEnemyBaseRendererMat
{
    public Renderer m_RendererObj = null;
    public Material m_DeathMat = null;
    public Material m_NormalMat = null;
}


public class CEnemyBaseMemoryShare : CActorMemoryShare
{
    public CEnemyBase                   m_MyEnemyBase               = null;
    public Transform                    m_PlayerLight               = null;
    public CPlayerLightShowMesh[]       m_AllPlayerLightShowMesh    = null;
    public Image[]                      m_AllEmoticons              = null;
    public bool                         m_WasFound                  = false;
    public List<CEnemyBaseRendererMat> m_AllChangRendererMat        = null;
};

public abstract class CEnemyBase : CActor
{
    public enum EEnemyType
    {
        eEnemyRifle = 0,
        eMax
    };

    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;
    public bool WasFound {get => m_MyEnemyBaseMemoryShare.m_WasFound;}

    public override void SetChangState(CMovableStatePototype.EMovableState state, int changindex = -1)
    {
        if (state == CMovableStatePototype.EMovableState.eHit && WasFound)
            return;

        base.SetChangState(state, changindex);
    }

    // ==================== SerializeField ===========================================
    [SerializeField] protected Image[]                          m_AllEmoticons          = null;
    [SerializeField] protected List<CEnemyBaseRendererMat>      m_AllChangRendererMat   = new List<CEnemyBaseRendererMat>();
    // ==================== SerializeField ===========================================

    public override EActorType MyActorType() { return EActorType.eEnemy; }
    abstract public EEnemyType MyEnemyType();

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemyBase(this));


        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateBase(this));

        //m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
        m_AllCreateList[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => 
        {
            return new CEnemySurprisBuff(this);
        };
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyBaseMemoryShare = new CEnemyBaseMemoryShare();
        m_MyMemoryShare = m_MyEnemyBaseMemoryShare;

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase          = this;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons         = m_AllEmoticons;
        m_MyEnemyBaseMemoryShare.m_AllChangRendererMat  = m_AllChangRendererMat;

       // this.transform.FindChild();

        base.CreateMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_MyGameManager.AddEnemyBaseListData(this);

        base.Start();

        m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh = this.GetComponentsInChildren<CPlayerLightShowMesh>();

        foreach (CPlayerLightShowMesh CPLSM in m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh)
            CPLSM.PlayerLight = m_MyGameManager.Player.PlayCtrlLight;

        this.SetChangState(CMovableStatePototype.EMovableState.eWait);
    }

}
