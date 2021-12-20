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

    [System.Serializable]
    public class PlayerFortData
    {
        public Transform m_Fort = null;
        public Transform m_LauncherPoint = null;
    }


    public const float CsLightDisOverallRatioZ      = StaticGlobalDel.g_CsLightDisMaxZ - StaticGlobalDel.g_CsLightDisMinZ;
    public const float CsLightScaleMaxZ             = 0.08f;
    public const float CsLightScaleMinZ             = 0.04f;
    public const float CsLightScaleOverallRatioZ    = CsLightScaleMaxZ - CsLightScaleMinZ;

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
    // ==================== SerializeField ===========================================

    public override float DefSpeed { get { return 5.0f; } }

    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    int m_MoveingHash = 0;
 

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


        base.CreateMemoryShare();

        SetBaseMemoryShare();
      
        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 5.0f;

        UpdateSearchLightDir();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(CMovableStatePototype.EMovableState.eWait);

        //UpdateAnimationVal().Subscribe(_ => {
        //    UpdateAnimationChangVal();
        //}).AddTo(this.gameObject);


    }

    public void UpdateAnimationChangVal()
    {
       // if (m_MyPlayerMemoryShare.m_isupdateAnimation)
            m_AnimatorStateCtl.SetFloat(m_MoveingHash, m_MyPlayerMemoryShare.m_AnimationVal.Value);
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
        //if (!PlayerCtrl())
        //{
        //    if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //    {
        //        m_AllState[(int)m_CurState].MouseDown();
        //    }
        //}

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

    public void GameOver()
    {
     //   float lTempResultPercent = 1.0f - (float)m_MyPlayerMemoryShare.m_PlayerFollwer.result.percent;
        //float lTempFeverScoreRatio = (float)m_MyPlayerMemoryShare.m_UpdateFeverScore.Value / (float)StaticGlobalDel.g_MaxFever;
        //float lTempResult = lTempFeverScoreRatio;

        //CAllScoringBox lTempAllScoringBox = CAllScoringBox.SharedInstance;
        //m_MyPlayerMemoryShare.m_EndIndex = (int)(lTempResult * (float)lTempAllScoringBox.AllScoringBox.Count - 1);

        //m_MyPlayerMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWin;
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
