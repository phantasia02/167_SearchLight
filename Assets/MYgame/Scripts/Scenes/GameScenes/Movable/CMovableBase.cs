using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CMovableBaseListData
{
    public List<CMovableBase> m_MovableBaseListData = new List<CMovableBase>();
}

public class CMemoryShareBase
{
    public Vector3                          m_OldPos;
    public Vector3                          m_CurmousePosition;
    public UniRx.ReactiveProperty<float>    m_TotleSpeed            = new ReactiveProperty<float>(StaticGlobalDel.g_DefMovableTotleSpeed);
    public float                            m_TargetTotleSpeed      = StaticGlobalDel.g_DefMovableTotleSpeed;
    public float[]                          m_Buff                  = new float[(int)CMovableBase.ESpeedBuff.eMax];
    public int                              m_NumericalImage        = 0;
    public CMovableBase                     m_MyMovable             = null;
    public Rigidbody                        m_MyRigidbody           = null;
    public Collider                         m_MyCollider            = null;
    public CMovableStateData[]              m_Data                  = new CMovableStateData[(int)StaticGlobalDel.EMovableState.eMax];
    public CMovableBase.DataState[]         m_AllState              = null;
    public CMovableBase.DataState           m_OldDataState          = new CMovableBase.DataState();
    public Transform                        m_MyTransform           = null;
};

public abstract class CMovableBase : CGameObjBas
{

    public const float CRadius = 20.0f;

    public class DataState
    {
        public List<CMovableStatePototype> AllThisState = new List<CMovableStatePototype>();
        public int index = 0;
    }

    public enum ESpeedBuff
    {
        eHit = 0,
        eMax
    };

    public enum EMovableType
    {
        eActor             = 0,
        eMax
    };

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Handles.color = Color.red;
        //Vector3 pos = transform.position + Vector3.up * 1.0f;
        //Handles.DrawAAPolyLine(6, pos, pos + transform.forward * CMovableBase.CJumpObstacleDis);
        //Handles.color = Color.white;
    }
#endif


    public override EObjType ObjType() { return EObjType.eMovable; }
    public Rigidbody MyRigidbody { get { return m_MyMemoryShare.m_MyRigidbody; } }

    protected DataState[] m_AllState = new DataState[(int)StaticGlobalDel.EMovableState.eMax];

    protected StaticGlobalDel.EMovableState m_CurState = StaticGlobalDel.EMovableState.eNull;
    public StaticGlobalDel.EMovableState CurState { get { return m_CurState; } }

    protected StaticGlobalDel.EMovableState m_OldState = StaticGlobalDel.EMovableState.eNull;
    public StaticGlobalDel.EMovableState OldState { get { return m_OldState; } }

    protected StaticGlobalDel.EMovableState m_ChangState = StaticGlobalDel.EMovableState.eMax;
    public virtual StaticGlobalDel.EMovableState ChangState
    {
        set
        {
            if (LockChangState != StaticGlobalDel.EMovableState.eMax && LockChangState != value)
                return;

            m_ChangState = value;
        }
        get { return m_ChangState; }
    }

    protected int m_ChangStateinndex = -1;
    public int ChangStateinndex
    {
        set{m_ChangStateinndex = value;}
        get { return m_ChangStateinndex; }
    }


    protected StaticGlobalDel.EMovableState m_NextFramChangState = StaticGlobalDel.EMovableState.eMax;
    public StaticGlobalDel.EMovableState NextFramChangState
    {
        set
        {
            if (LockChangState != StaticGlobalDel.EMovableState.eMax && LockChangState != value)
                return;

            m_NextFramChangState = value;
        }
        get { return m_NextFramChangState; }
    }

    protected StaticGlobalDel.EMovableState m_LockChangState = StaticGlobalDel.EMovableState.eMax;
    public StaticGlobalDel.EMovableState LockChangState
    {
        set { m_LockChangState = value; }
        get { return m_LockChangState; }
    }


    protected bool m_SameStatusUpdate = false;
    public bool SameStatusUpdate
    {
        set { m_SameStatusUpdate = value; }
        get { return m_SameStatusUpdate; }
    }

    // ==================== SerializeField ===========================================

    //[SerializeField] protected Rigidbody m_MyRigidbody = null;
    [SerializeField] protected Collider m_MyCollider = null;

    // ==================== SerializeField ===========================================

    protected CMemoryShareBase m_MyMemoryShare = null;
    public CMemoryShareBase MyMemoryShare { get { return m_MyMemoryShare; } }

    public int ImageNumber { get { return m_MyMemoryShare.m_NumericalImage; } }
    public float TotleSpeed { get { return m_MyMemoryShare.m_TotleSpeed.Value; } }
    public float TotleSpeedRatio { get { return m_MyMemoryShare.m_TotleSpeed.Value / StaticGlobalDel.g_DefMovableTotleSpeed; } }

    abstract public EMovableType MyMovableType();
    
    protected int m_MovableBasIndex = -1;
    public int MovableBasIndex
    {
        set { m_MovableBasIndex = value; }
        get { return m_MovableBasIndex; }
    }

    protected bool m_AwakeOK = false;
    protected virtual bool AutoAwake() { return true; }

    public  virtual float DefSpeed { get { return StaticGlobalDel.g_DefMovableTotleSpeed; } }

    protected override void Awake()
    {
        base.Awake();

        if (AutoAwake())
            ManualAwake();
    }

    public virtual void ManualAwake()
    {
        for (int i = 0; i < m_AllState.Length; i++)
            m_AllState[i] = new DataState();

        CreateMemoryShare();
        AddInitState();

        AwakeOK();

        MyMemoryShare.m_TotleSpeed.Value = DefSpeed;
        MyMemoryShare.m_TargetTotleSpeed = DefSpeed;

        m_AwakeOK = true;
    }

    protected virtual void CreateMemoryShare()
    {
        m_MyMemoryShare = new CMemoryShareBase();

        SetBaseMemoryShare();
    }

    protected virtual void AddInitState()
    {

    }

    protected void SetBaseMemoryShare()
    {
        m_MyMemoryShare.m_MyTransform           = this.transform;
        m_MyMemoryShare.m_MyMovable             = this;

        m_MyMemoryShare.m_MyRigidbody           = this.GetComponent<Rigidbody>();
        m_MyMemoryShare.m_MyCollider            = m_MyCollider;
    }

    protected virtual void AwakeEndSetNullState()
    {
        StaticGlobalDel.EMovableState lTempState = StaticGlobalDel.EMovableState.eNull;

        for (int i = 0; i < m_AllState.Length; i++)
        {
            lTempState = (StaticGlobalDel.EMovableState)i;

            if (lTempState == StaticGlobalDel.EMovableState.eNull || 
                m_AllState[i].AllThisState.Count > 0)
                continue;

            switch (lTempState)
            {
                case StaticGlobalDel.EMovableState.eWait:
                    m_AllState[i].AllThisState.Add(new CWaitStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eMove:
                    m_AllState[i].AllThisState.Add(new CMoveStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eDrag:
                    m_AllState[i].AllThisState.Add(new CDragStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eJump:
                    m_AllState[i].AllThisState.Add(new CJumpStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eJumpDown:
                    m_AllState[i].AllThisState.Add(new CJumpDownStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eWin:
                    m_AllState[i].AllThisState.Add(new CWinStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eDeath:
                    m_AllState[i].AllThisState.Add(new CDeathStateBase(this));
                    break;
                case StaticGlobalDel.EMovableState.eFinish:
                    m_AllState[i].AllThisState.Add(new CFinishStateBase(this));
                    break;
            }
        }

        m_MyMemoryShare.m_AllState = m_AllState;
    }

    protected void AwakeOK()
    {
        AwakeEndSetNullState();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_MyGameManager.AddMovableBaseListData(this);
        base.Start();
    }

    protected override void OnDestroy()
    {
        m_MyGameManager.RemoveMovableBaseListData(this);
        base.OnDestroy();
    }

    public override void Init()
    {
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!m_AwakeOK)
            return;

        base.Update();

        //StaticGlobalDel.EMovableState lTempNextFramChangState = m_NextFramChangState;

        DataState lTempDataState = m_AllState[(int)m_CurState];

        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
        {
            lTempDataState.AllThisState[lTempDataState.index].updataMovableState();
        }

        ChangStateFunc();

        if (NextFramChangState != StaticGlobalDel.EMovableState.eMax)
        {
            m_ChangState = m_NextFramChangState;
            NextFramChangState = StaticGlobalDel.EMovableState.eMax;
        }

        m_MyMemoryShare.m_OldPos = transform.position;
    }

    public virtual void ChangStateFunc()
    {

        if (ChangState != StaticGlobalDel.EMovableState.eMax || ChangStateinndex != -1)
        {
            SetCurState(m_ChangState, m_ChangStateinndex);
            
        }
    }

    protected virtual void LateUpdate()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].LateUpdate();
    }

    //public void SetStateIndex(StaticGlobalDel.EMovableState pamState, int index)
    //{

    //    DataState lTempDataState = m_AllState[(int)pamState];
    //    if (lTempDataState == null)
    //        return;

    //    if (m_AllState[(int)pamState].AllThisState.Count <= index)
    //        return;

    //    if (m_AllState[(int)pamState].AllThisState[index] == null)
    //        return;

    //    int lTempOldIndex = m_AllState[(int)pamState].index;

    //    if (lTempOldIndex == index)
    //        return;

    //    m_AllState[(int)pamState].index = index;

    //    if (pamState == CurState && m_ChangState == StaticGlobalDel.EMovableState.eMax)
    //    {
    //        if (CurState != StaticGlobalDel.EMovableState.eNull)
    //        {
    //            if (lTempDataState != null && lTempDataState.AllThisState[lTempOldIndex] != null)
    //                lTempDataState.AllThisState[lTempOldIndex].OutMovableState();
    //        }

    //        if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
    //            lTempDataState.AllThisState[lTempDataState.index].InMovableState();

    //        m_MyMemoryShare.m_OldDataState.AllThisState = m_AllState[(int)CurState].AllThisState;
    //        m_MyMemoryShare.m_OldDataState.index = lTempOldIndex;

    //        if (CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
    //            lTempDataState.AllThisState[lTempDataState.index].updataMovableState();
    //    }
    //}

    public virtual void SetCurState(StaticGlobalDel.EMovableState pamState, int stateindex = -1)
    {
        if (pamState == CurState && !SameStatusUpdate)
            return;

        m_ChangState = StaticGlobalDel.EMovableState.eMax;
        m_ChangStateinndex = -1;
        StaticGlobalDel.EMovableState lTempOldState = CurState;
        m_CurState = pamState;
        m_MyMemoryShare.m_OldDataState.AllThisState = m_AllState[(int)lTempOldState].AllThisState;
        m_MyMemoryShare.m_OldDataState.index = m_AllState[(int)lTempOldState].index;

        DataState lTempDataState = null;

        lTempDataState = m_AllState[(int)lTempOldState];
        if (lTempOldState != StaticGlobalDel.EMovableState.eNull)
        {
            if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].OutMovableState();
        }

        lTempDataState = m_AllState[(int)m_CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull)
        {


            if (lTempDataState != null)
            {
                if (stateindex != -1 && lTempDataState.AllThisState[stateindex] != null)
                    lTempDataState.index = stateindex;
                
                lTempDataState.AllThisState[lTempDataState.index].InMovableState();
            }
        }

        m_OldState = lTempOldState;
        SameStatusUpdate = false;

        if (m_CurState != StaticGlobalDel.EMovableState.eNull)
        {
            if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].updataMovableState();
        }
    }

    public void DestroyThis()
    {
        StartCoroutine(StartCoroutineDestroyThis());
    }


    IEnumerator StartCoroutineDestroyThis()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    public virtual void TouchBouncingBed(Collider other)
    {
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerStay(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerExit(other);
    }


    public virtual void OnCollisionEnter(Collision other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnCollisionEnter(other);
    }

    public virtual void HitInput(RaycastHit hit)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].Input(hit);
    }

    public virtual void InputUpdata()
    {
        //if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //{
        //    m_AllState[(int)m_CurState].Input();
        //}
    }

    public virtual void OnMouseDown()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDown();

    }

    public virtual void OnMouseDrag()
    {
        m_MyMemoryShare.m_CurmousePosition = Input.mousePosition;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();
    }

    public virtual void OnMouseUp()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseUp();
    }

    public virtual void UpdateCurSpeed()
    {
        m_MyMemoryShare.m_TotleSpeed.Value = m_MyMemoryShare.m_TargetTotleSpeed;
    }

    public void SetMoveBuff(ESpeedBuff type, float ratio, bool updateCurSpeed = false)
    {
        m_MyMemoryShare.m_Buff[(int)type] = ratio;
        float lTempMoveRatio = 1.0f;

        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            lTempMoveRatio *= m_MyMemoryShare.m_Buff[i];

        m_MyMemoryShare.m_TargetTotleSpeed = DefSpeed  * lTempMoveRatio;
        //m_MyMemoryShare.m_TotleSpeed 
        if (updateCurSpeed)
            UpdateCurSpeed();
    }

    public void ResetMoveBuff(bool updateCurSpeed = false)
    {
        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            m_MyMemoryShare.m_Buff[i] = 1.0f;

        m_MyMemoryShare.m_TargetTotleSpeed = DefSpeed;
        if (updateCurSpeed)
            UpdateCurSpeed();
    }

    // ===================== UniRx ======================
    public UniRx.ReactiveProperty<float> UpdateTotleSpeed()
    {
        return m_MyMemoryShare.m_TotleSpeed ?? (m_MyMemoryShare.m_TotleSpeed = new ReactiveProperty<float>(DefSpeed));
    }
    // ===================== UniRx ======================
}
