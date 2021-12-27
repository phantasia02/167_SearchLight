using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pathfinding;

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
    public bool                                         m_IsShow                    = false;
    public CBulletFlyObj.EBulletArms                    m_AtkType                   = CBulletFlyObj.EBulletArms.eNormalBullet;
    public List<CMovableStatePototype.EMovableState>    m_MyRandomStateList         = new List<CMovableStatePototype.EMovableState>();
    public Transform                                    m_MyBodyDeformationSystem   = null;
    public CEnemyBase.EATKShowObj                       m_CurATKShowObj             = CEnemyBase.EATKShowObj.eNotATKShow;
    public Seeker                                       m_seeker                    = null;
    public IAstarAI                                     m_AiMove                    = null;
};

public abstract class CEnemyBase : CActor
{
    public enum EEnemyType
    {
        eEnemyRifle         = 0,
        eEnemyMachineGunner = 1,
        eBoxEnemyRifle      = 2,
        eEnemyDiver         = 3,
        eMax
    };

    public enum EATKShowObj
    {
        eNotATKShow     = 0,
        eATKShow        = 1,
        eGrenade        = 2,
        eMax
    };

    protected CEnemyBaseMemoryShare m_MyEnemyBaseMemoryShare = null;
    public bool WasFound
    {
        set => m_MyEnemyBaseMemoryShare.m_WasFound = value;
        get => m_MyEnemyBaseMemoryShare.m_WasFound;
    }
    public bool Hidden
    {
        set => m_MyEnemyBaseMemoryShare.m_hidden = value;
        get => m_MyEnemyBaseMemoryShare.m_hidden;
    }
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
    [SerializeField] protected Image[]                          m_AllEmoticons              = null;
    [SerializeField] protected List<CEnemyBaseRendererMat>      m_AllChangRendererMat       = new List<CEnemyBaseRendererMat>();
    [SerializeField] protected SDataListGameObj[]               m_StateShowObj              = null;

    [SerializeField] protected Transform m_MyTargetBodys = null;
    public Transform MyTargetBodys => m_MyTargetBodys;

    [SerializeField] protected Transform                        m_MyBodyDeformationSystem   = null;

    [SerializeField] protected CBulletFlyObj.EBulletArms m_AtkType = CBulletFlyObj.EBulletArms.eNormalBullet;
    public CBulletFlyObj.EBulletArms AtkType
    {
        set => m_AtkType = value;
        get => m_AtkType;
    }
    // ==================== SerializeField ===========================================

    public override EObjType ObjType() { return EObjType.eEnemy; }
    public override EActorType MyActorType() { return EActorType.eEnemy; }
    public override EMovableType MyMovableType() { return EMovableType.eEnemy; }
    abstract public EEnemyType MyEnemyType();

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemyBase(this));

        //m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateEnemyBase(this));
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CAIStateEnemyBase(this));


        m_AllCreateList[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => {return new CEnemySurprisBuff(this);};
    }

    protected override void CreateMemoryShare()
    {
        m_MyGameManager.AddEnemyBaseListData(this);
        m_MyEnemyBaseMemoryShare = new CEnemyBaseMemoryShare();
        m_MyMemoryShare = m_MyEnemyBaseMemoryShare;

        m_MyEnemyBaseMemoryShare.m_MyEnemyBase          = this;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons         = m_AllEmoticons;
        m_MyEnemyBaseMemoryShare.m_AllChangRendererMat  = m_AllChangRendererMat;
        m_MyEnemyBaseMemoryShare.m_StateShowObj         = m_StateShowObj;
        m_MyEnemyBaseMemoryShare.m_AtkType              = m_AtkType;
        m_MyEnemyBaseMemoryShare.m_seeker               = this.GetComponent<Seeker>();
        m_MyEnemyBaseMemoryShare.m_AiMove               = this.GetComponent<IAstarAI>();
       // this.transform.FindChild();

        base.CreateMemoryShare();

        ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_MyEnemyBaseMemoryShare.m_MyBodyDeformationSystem = m_MyBodyDeformationSystem;
        m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh = this.GetComponentsInChildren<CPlayerLightShowMesh>();

        foreach (CPlayerLightShowMesh CPLSM in m_MyEnemyBaseMemoryShare.m_AllPlayerLightShowMesh)
            CPLSM.PlayerLight = m_MyGameManager.Player.PlayCtrlLight;


        this.transform.forward = Vector3.back;
        //this.SetChangState(CMovableStatePototype.EMovableState.eWait);
    }

    protected override void InitSetActorCR()
    {
        if (m_AllObj != null)
        {
            m_MyActorMemoryShare.m_MyActorCollider = m_AllObj.GetComponentsInChildren<Collider>(true);
            m_MyActorMemoryShare.m_MyActorRigidbody = m_AllObj.GetComponentsInChildren<Rigidbody>(true);
        }
    }

    public void ATKShowObj(CEnemyBase.EATKShowObj showType)
    {
        void showobj(SDataListGameObj allobj, bool show)
        {
            if (allobj != null)
            {
                for (int x = 0; x < allobj.m_ListObj.Count; x++)
                    allobj.m_ListObj[x].SetActive(show);
            }
        }

        SDataListGameObj lTempSDataListGameObj = null;
        for (int i = 0; i < m_MyEnemyBaseMemoryShare.m_StateShowObj.Length; i++)
        {
            lTempSDataListGameObj = m_MyEnemyBaseMemoryShare.m_StateShowObj[i];
            showobj(lTempSDataListGameObj, false);
        }

        lTempSDataListGameObj = m_MyEnemyBaseMemoryShare.m_StateShowObj[(int)showType];
        showobj(lTempSDataListGameObj, true);
    }

    protected override void Update()
    {
        int hdsiughduio = 0;
        base.Update();
    }

    public override void SetCurState(CMovableStatePototype.EMovableState pamState, int stateindex = -1)
    {
        base.SetCurState(pamState, stateindex);
    }
}
