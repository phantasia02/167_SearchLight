using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;



public class CPlayerMemoryShare : CActorMemoryShare
{
    public bool                             m_bDown                     = false;
    public Vector3                          m_OldMouseDownPos           = Vector3.zero;
    public Vector3                          m_OldMouseDragDirNormal     = Vector3.zero;
    public Vector3                          m_CurMoveDirNormal          = Vector3.zero;
    public CPlayer                          m_MyPlayer                  = null;
    public Vector3[]                        m_AllPathPoint              = new Vector3[8];
    public int                              m_CurStandPointindex        = 0;
    public Vector3                          m_TargetStandPoint          = Vector3.zero;
    public Collider                         m_SwordeCollider            = null;
    public UniRx.ReactiveProperty<float>    m_AnimationVal              = new ReactiveProperty<float>(0.5f);
    public float                            m_AddSpeedSecond            = 5.0f;
   // public UniRx.ReactiveProperty<int>      m_UpdateFeverScore          = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever);

    public GameObject                       m_CollisionBox              = null;
    public GameObject                       m_TagBox                    = null;
    public GameObject                       m_SearchlightRLObj          = null;
    public GameObject                       m_SearchlightTDObj          = null;
    public Transform                        m_PlayCtrlLight             = null;
    public CPlayer.PlayerFortData[]         m_AllPlayerFortData         = new CPlayer.PlayerFortData[(int)CPlayer.EFortRL.EMax];
    public CEnemyBase                       m_TargetBuffer              = null;
    public CPlayer.CDateSearchlightSmoke[]  m_AllDateSearchlightSmoke   = null;
    
    // public float                            m_LauncherSpeed             = 0.2f;
};

public class CPlayer : CActor
{
    public enum EFortRL
    {
        ERFort = 0,
        ELFort = 1,
        EMax
    }

    public enum EMpbType
    {
        ELightTD = 0,
        EMax
    }

    [System.Serializable]
    public class PlayerFortData
    {
        public Transform m_Fort = null;
        public Transform m_LauncherPoint = null;
    }

    [System.Serializable]
    public class CDateSearchlightLEVEL
    {
        public GameObject m_ShowSearchlightLEVEL = null;
        [HideInInspector] public int m_ShowHp = 100;
    }

    [System.Serializable]
    public class CDateSearchlightSmoke
    {
        public GameObject m_SmokeSearchlightLEVELObj = null;
        [HideInInspector] public ParticleSystem m_SmokeParticleSystem = null;
        public float m_HpRatio = 0.9f;
    }

    public const float CsLightDisOverallRatioZ      = StaticGlobalDel.g_CsLightDisMaxZ - StaticGlobalDel.g_CsLightDisMinZ;
    public const float CsLightScaleMaxZ             = 0.1f;
    public const float CsLightScaleMinZ             = 0.04f;
    public const float CsLightScaleOverallRatioZ    = CsLightScaleMaxZ - CsLightScaleMinZ;
    public const int   CSMaxHp                      = 100;

    public override EObjType ObjType() { return EObjType.ePlayer; }
    public override EActorType MyActorType() { return EActorType.ePlayer; }
    public override EMovableType MyMovableType() { return EMovableType.ePlayer; }
    //public override EMovableType MyMovableType() { return EMovableType.ePlayer; }

    protected float m_MaxMoveDirSize = 5.0f;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    // ==================== SerializeField ===========================================

    [SerializeField] protected GameObject m_CollisionBox = null;
    [SerializeField] protected GameObject m_TagBox = null;

    [SerializeField] protected GameObject   m_SearchlightRLObj  = null;
    [SerializeField] protected GameObject   m_SearchlightTDObj  = null;
    public GameObject SearchlightTDObj
    {
        get
        {
            if (m_MyPlayerMemoryShare == null)
                return m_SearchlightTDObj;
            
            return m_MyPlayerMemoryShare.m_SearchlightTDObj;
        }
    }
    [SerializeField] protected Transform    m_LightTDObj        = null;
    [SerializeField] protected Transform    m_PlayCtrlLight     = null;
    [SerializeField] protected PlayerFortData[]  m_RLFortData            = null;
    public Transform PlayCtrlLight
    {
        get
        {
            if (m_MyPlayerMemoryShare == null)
                return m_PlayCtrlLight;

            return m_MyPlayerMemoryShare.m_PlayCtrlLight;
        }
    }

    [SerializeField] protected CDateSearchlightLEVEL[] m_AllSearchlightLEVEL = null;
    protected int m_ShowSearchlightLEVELIndex = -1;

    [SerializeField] protected CDateSearchlightSmoke[] m_AllDateSearchlightSmoke = null;
    
    // [SerializeField] protected GameObject[] m_AllSearchlightLEVEL = null;
    // ==================== SerializeField ===========================================

    public override float DefSpeed { get { return 5.0f; } }

    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    protected Renderer m_LightTDRenderer = null;
    MaterialPropertyBlock[] m_Allmpb = new MaterialPropertyBlock[(int)EMpbType.EMax];
    public MaterialPropertyBlock GetMpb(EMpbType MpbType)
    {
        int lTempindex = (int)MpbType;
        if (m_Allmpb[lTempindex] == null)
            m_Allmpb[lTempindex] = new MaterialPropertyBlock();
        return m_Allmpb[lTempindex];
    }


    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayer(this));
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayer(this));

        
        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_MyPlayer                = this;
        m_MyPlayerMemoryShare.m_CollisionBox            = m_CollisionBox;
        m_MyPlayerMemoryShare.m_TagBox                  = m_TagBox;
        m_MyPlayerMemoryShare.m_SearchlightRLObj        = m_SearchlightRLObj;
        m_MyPlayerMemoryShare.m_SearchlightTDObj        = m_SearchlightTDObj;
        m_MyPlayerMemoryShare.m_PlayCtrlLight           = m_PlayCtrlLight;
        m_MyPlayerMemoryShare.m_AllPlayerFortData       = m_RLFortData;
        m_MyPlayerMemoryShare.m_Hp.Value                = CSMaxHp;
        m_MyPlayerMemoryShare.m_AllDateSearchlightSmoke = m_AllDateSearchlightSmoke;

        base.CreateMemoryShare();

        SetBaseMemoryShare();
      
        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 5.0f;

        UpdateSearchLightDir();

        int lTempSearchlightLEVELAddNumber = CSMaxHp / (m_AllSearchlightLEVEL.Length - 1);
        int lTempCurSearchlightLEVELHp = CSMaxHp;

        for (int i = 0; i < m_AllSearchlightLEVEL.Length; i++)
        {
            if (i == 0)
                m_AllSearchlightLEVEL[0].m_ShowHp = CSMaxHp;
            else if (i == m_AllSearchlightLEVEL.Length - 1)
                m_AllSearchlightLEVEL[m_AllSearchlightLEVEL.Length - 1].m_ShowHp = 0;
            else
            {
                lTempCurSearchlightLEVELHp -= lTempSearchlightLEVELAddNumber;
                m_AllSearchlightLEVEL[i].m_ShowHp = lTempCurSearchlightLEVELHp;
            }
        }

        for (int i = 0; i < m_AllDateSearchlightSmoke.Length; i++)
            m_AllDateSearchlightSmoke[i].m_SmokeParticleSystem = m_AllDateSearchlightSmoke[i].m_SmokeSearchlightLEVELObj.GetComponentInChildren<ParticleSystem>();

        UpdateShowSearchLightIndex(0);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_LightTDRenderer = m_LightTDObj.GetComponent<Renderer>();
        

       // SetCurState(CMovableStatePototype.EMovableState.eWait);

        UpdateHpVal().Subscribe(x => 
        {
            HpValUpdate(x);
        }).AddTo(this.gameObject);
    }

    public void HpValUpdate(int updatehp)
    {
        float lTempHpratio = (float)updatehp / (float)CSMaxHp;
        float lTempReverseHpratio = 1.0f - lTempHpratio;

        m_LightTDRenderer.material.EnableKeyword("_EMISSION");
        MaterialPropertyBlock lTempMaterialPropertyBlock = GetMpb(EMpbType.ELightTD);
        lTempMaterialPropertyBlock.SetColor(StaticGlobalDel._emissionColor, Color.Lerp(Color.black, Color.white, lTempHpratio));
        m_LightTDRenderer.SetPropertyBlock(lTempMaterialPropertyBlock);

        int lTempindex = 0;
        for (int i = m_AllSearchlightLEVEL.Length - 1; i >= 0; i--)
        {
            if (m_AllSearchlightLEVEL[i].m_ShowHp >= updatehp)
            {
                lTempindex = i;
                break;
            }
        }

        CPlayer.CDateSearchlightSmoke[] TempSearchlightSmoke = m_MyPlayerMemoryShare.m_AllDateSearchlightSmoke;

        const float MaxRateOverTime = 40.0f;
        for (int i = 0; i < TempSearchlightSmoke.Length; i++)
        {
            if (TempSearchlightSmoke[i].m_HpRatio >= lTempHpratio)
            {

                if (!TempSearchlightSmoke[i].m_SmokeSearchlightLEVELObj.activeSelf)
                    TempSearchlightSmoke[i].m_SmokeSearchlightLEVELObj.SetActive(true);

                var Tempemission = TempSearchlightSmoke[i].m_SmokeParticleSystem.emission;
               Tempemission.rateOverTime = MaxRateOverTime * lTempReverseHpratio * (1 - TempSearchlightSmoke[i].m_HpRatio);
            }
        }
       
        UpdateShowSearchLightIndex(lTempindex);

        if (updatehp <= 0 && m_MyGameManager.CurState == CGameManager.EState.ePlay)
        {
            SetChangState(CMovableStatePototype.EMovableState.eDeath);
            m_MyGameManager.SetState( CGameManager.EState.eGameOver);
        }
    }

    public void UpdateShowSearchLightIndex(int Index)
    {
        if (m_ShowSearchlightLEVELIndex == Index)
            return;
        
        m_ShowSearchlightLEVELIndex = Index;
        foreach (CDateSearchlightLEVEL DSL in m_AllSearchlightLEVEL)
            DSL.m_ShowSearchlightLEVEL.SetActive(false);

        m_AllSearchlightLEVEL[m_ShowSearchlightLEVELIndex].m_ShowSearchlightLEVEL.SetActive(true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
        InputUpdata();
    }

    public override void InputUpdata()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            PlayerMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }
    }

    public void PlayerMouseDown()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDown();

        m_MyPlayerMemoryShare.m_bDown = true;
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseDrag()
    {
        //if (!PlayerCtrl())
        //    return;
        if (!m_MyPlayerMemoryShare.m_bDown)
            return;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();

        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseUp()
    {
        if (m_MyPlayerMemoryShare.m_bDown)
        {
            DataState lTempDataState = m_AllState[(int)CurState];
            if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].MouseUp();

            m_MyPlayerMemoryShare.m_bDown = false;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;
        }
    }

    public void UpdateDrag()
    {
        if (!m_MyPlayerMemoryShare.m_bDown)
            return;


        Vector3 lTempMouseDrag = Input.mousePosition - m_MyPlayerMemoryShare.m_OldMouseDownPos;
        lTempMouseDrag.z = lTempMouseDrag.y;
        lTempMouseDrag.y = 0.0f;
        //  float lTempScreenDragProportion = lTempMouseDrag.magnitude / m_MinScreenSize;
        //if (lTempScreenDragProportion >= 0.01f)
        // {
        SetChangState(CMovableStatePototype.EMovableState.eMove);
        m_OldMouseDragDir += lTempMouseDrag * 5.0f;
        m_OldMouseDragDir.y = 0.0f;

        m_OldMouseDragDir = Vector3.ClampMagnitude(m_OldMouseDragDir, m_MaxMoveDirSize);
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal = m_OldMouseDragDir;
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal.Normalize();
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;

        if (m_MyPlayerMemoryShare.m_OldMouseDragDirNormal == Vector3.zero)
            return;

        m_MyPlayerMemoryShare.m_PlayCtrlLight.forward = m_MyPlayerMemoryShare.m_OldMouseDragDirNormal;
      //  m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_OldMouseDragDirNormal;
    }

    public void UpdateSearchLightDir()
    {
        Vector3 lTempRLForward = m_MyPlayerMemoryShare.m_PlayCtrlLight.position - m_MyPlayerMemoryShare.m_SearchlightRLObj.transform.position;
        lTempRLForward.y = 0.0f;
        lTempRLForward.Normalize();

        m_MyPlayerMemoryShare.m_SearchlightRLObj.transform.forward = lTempRLForward;

        Vector3 lTempTDupV3 = m_MyPlayerMemoryShare.m_PlayCtrlLight.position - m_MyPlayerMemoryShare.m_SearchlightTDObj.transform.position;
        lTempTDupV3.x = lTempTDupV3.z;
        lTempTDupV3.z = 0.0f;
        lTempTDupV3.Normalize();

        float lTempUpAngle = Vector2.Angle(Vector2.up, lTempTDupV3);
        m_MyPlayerMemoryShare.m_SearchlightTDObj.transform.localRotation = Quaternion.Euler(90.0f + -lTempUpAngle, 0.0f, 0.0f);

        float lTempDisRatioZ = (m_MyPlayerMemoryShare.m_PlayCtrlLight.position.z - StaticGlobalDel.g_CsLightDisMinZ) / CsLightDisOverallRatioZ;
        Vector3 lTemplocalScale = m_LightTDObj.localScale;
        lTemplocalScale.z = lTempDisRatioZ * CsLightScaleOverallRatioZ + CsLightScaleMinZ;
        m_LightTDObj.localScale = lTemplocalScale;


        for (int i = 0; i < m_MyPlayerMemoryShare.m_AllPlayerFortData.Length; i++)
        {
            lTempRLForward = m_MyPlayerMemoryShare.m_PlayCtrlLight.position - m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_Fort.position;
            lTempRLForward.y = 0.0f;
            lTempRLForward.Normalize();

            Quaternion rot = Quaternion.LookRotation(lTempRLForward * -1.0f);

            rot *= Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            m_MyPlayerMemoryShare.m_AllPlayerFortData[i].m_Fort.rotation = rot;
        }
    }



    // ===================== UniRx ======================


    //public UniRx.ReactiveProperty<int> UpdateFeverScoreVal()
    //{
    //    return m_MyPlayerMemoryShare.m_UpdateFeverScore ?? (m_MyPlayerMemoryShare.m_UpdateFeverScore = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever));
    //}
    // ===================== UniRx ======================
}
