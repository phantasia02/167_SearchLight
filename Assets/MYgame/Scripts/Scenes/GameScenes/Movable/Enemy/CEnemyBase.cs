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
    public CEnemyBase                                   m_MyEnemyBase               = null;
    public Transform                                    m_PlayerLight               = null;
    public CPlayerLightShowMesh[]                       m_AllPlayerLightShowMesh    = null;
    public Image[]                                      m_AllEmoticons              = null;
    public bool                                         m_WasFound                  = false;
    public bool                                         m_hidden                    = true;
    public List<CEnemyBaseRendererMat>                  m_AllChangRendererMat       = null;
    public SDataListGameObj[]                           m_StateShowObj              = null;
    public float                                        m_CurDiscoveryTime          = 0.0f;
    public float                                        m_OldDiscoveryTime          = 0.0f;
    public List<CMovableStatePototype.EMovableState>    m_MyRandomStateList         = new List<CMovableStatePototype.EMovableState>();
    public Transform                                    m_MyBodyDeformationSystem   = null;
};

public abstract class CEnemyBase : CActor
{
    public enum EEnemyType
    {
        eEnemyRifle = 0,
        eMax
    };

    public enum EATKShowObj
    {
        eNotATKShow     = 0,
        eATKShow        = 1,
        eMax
    };

    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;
    public bool WasFound {get => m_MyEnemyBaseMemoryShare.m_WasFound;}
    public bool Hidden { get => m_MyEnemyBaseMemoryShare.m_hidden; }
    public void AddCurDiscoveryTime(float addTime)
    {
        m_MyEnemyBaseMemoryShare.m_CurDiscoveryTime += addTime;
        if (m_MyEnemyBaseMemoryShare.m_CurDiscoveryTime >= 1.0f)
            m_MyEnemyBaseMemoryShare.m_hidden = false;

        if (m_MyEnemyBaseMemoryShare.m_CurDiscoveryTime <= 0.0f)
            m_MyEnemyBaseMemoryShare.m_CurDiscoveryTime = 0.0f;
    }

    public override void SetChangState(CMovableStatePototype.EMovableState state, int changindex = -1)
    {


        if (state == CMovableStatePototype.EMovableState.eHit && WasFound)
            return;

        base.SetChangState(state, changindex);
    }

    // ==================== SerializeField ===========================================
    [SerializeField] protected Image[]                          m_AllEmoticons          = null;
    [SerializeField] protected List<CEnemyBaseRendererMat>      m_AllChangRendererMat   = new List<CEnemyBaseRendererMat>();
    [SerializeField] protected SDataListGameObj[]               m_StateShowObj          = null;
    // ==================== SerializeField ===========================================

    public override EActorType MyActorType() { return EActorType.eEnemy; }
    abstract public EEnemyType MyEnemyType();

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateEnemyBase(this));


        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateBase(this));

        //m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
        m_AllCreateList[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => {return new CEnemySurprisBuff(this);};
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyBaseMemoryShare = new CEnemyBaseMemoryShare();
        m_MyMemoryShare = m_MyEnemyBaseMemoryShare;

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase          = this;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons         = m_AllEmoticons;
        m_MyEnemyBaseMemoryShare.m_AllChangRendererMat  = m_AllChangRendererMat;
        m_MyEnemyBaseMemoryShare.m_StateShowObj         = m_StateShowObj;

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
