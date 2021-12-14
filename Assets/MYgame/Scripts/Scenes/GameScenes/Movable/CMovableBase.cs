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
    public CMovableStateData[]              m_Data                  = new CMovableStateData[(int)CMovableStatePototype.EMovableState.eMax];
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
        eActor              = 0,
        eBulletFlyObj       = 1,
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

    protected DataState[] m_AllState = new DataState[(int)CMovableStatePototype.EMovableState.eMax];

    protected CMovableStatePototype.EMovableState m_CurState = CMovableStatePototype.EMovableState.eNull;
    public CMovableStatePototype.EMovableState CurState { get { return m_CurState; } }

    protected CMovableStatePototype.EMovableState m_OldState = CMovableStatePototype.EMovableState.eNull;
    public CMovableStatePototype.EMovableState OldState { get { return m_OldState; } }

    protected CMovableStatePototype.EMovableState m_ChangState = CMovableStatePototype.EMovableState.eMax;
    public CMovableStatePototype.EMovableState ChangState => m_ChangState;

    protected int m_ChangStateinndex = -1;
    public int ChangStateinndex => m_ChangStateinndex;

    public virtual void SetChangState(CMovableStatePototype.EMovableState state, int changindex = -1)
    {
        if (LockChangState != CMovableStatePototype.EMovableState.eMax && LockChangState != state)
            return;

        if (changindex != -1)
        {
            DataState lTempChangDataState = m_AllState[(int)state];
            if (lTempChangDataState.AllThisState.Count <= changindex)
                return;

            int CurPriority = 0;
            if (m_ChangState != CMovableStatePototype.EMovableState.eMax)
            {
                DataState lTempCurDataState = m_AllState[(int)m_ChangState];
                CurPriority = lTempCurDataState.AllThisState[lTempCurDataState.index].Priority;
            }

            if (CurPriority > lTempChangDataState.AllThisState[changindex].Priority)
                return;
        }

        m_ChangState = state;
        m_ChangStateinndex = changindex;
    }

    protected CMovableStatePototype.EMovableState m_NextFramChangState = CMovableStatePototype.EMovableState.eMax;
    public CMovableStatePototype.EMovableState NextFramChangState
    {
        set
        {
            if (LockChangState != CMovableStatePototype.EMovableState.eMax && LockChangState != value)
                return;

            m_NextFramChangState = value;
        }
        get { return m_NextFramChangState; }
    }

    protected CMovableStatePototype.EMovableState m_LockChangState = CMovableStatePototype.EMovableState.eMax;
    public CMovableStatePototype.EMovableState LockChangState
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

    protected List<CMovableBuffPototype> m_CurAllBuff = new List<CMovableBuffPototype>();

    protected DelCreateBuff[] m_NullDelCreateFunc = new DelCreateBuff[(int)CMovableBuffPototype.EMovableBuff.eMax];
    protected DelCreateBuff[] m_AllCreateList = new DelCreateBuff[(int)CMovableBuffPototype.EMovableBuff.eMax];

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
    
    //protected int m_MovableBasIndex = -1;
    //public int MovableBasIndex
    //{
    //    set { m_MovableBasIndex = value; }
    //    get { return m_MovableBasIndex; }
    //}

    protected bool m_AwakeOK = false;
    protected virtual bool AutoAwake() { return true; }

    public  virtual float DefSpeed { get { return StaticGlobalDel.g_DefMovableTotleSpeed; } }

    protected override void Awake()
    {
        m_NullDelCreateFunc[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => { return new CSurprisBuffBase(this); };

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

        //for (int i = 0; i < )
        //new CSurprisBuffBase(this)
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
        CMovableStatePototype.EMovableState lTempState = CMovableStatePototype.EMovableState.eNull;

        for (int i = 0; i < m_AllState.Length; i++)
        {
            lTempState = (CMovableStatePototype.EMovableState)i;

            if (lTempState == CMovableStatePototype.EMovableState.eNull || 
                m_AllState[i].AllThisState.Count > 0)
                continue;

            switch (lTempState)
            {
                case CMovableStatePototype.EMovableState.eWait:
                    m_AllState[i].AllThisState.Add(new CWaitStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eMove:
                    m_AllState[i].AllThisState.Add(new CMoveStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eDrag:
                    m_AllState[i].AllThisState.Add(new CDragStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eJump:
                    m_AllState[i].AllThisState.Add(new CJumpStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eJumpDown:
                    m_AllState[i].AllThisState.Add(new CJumpDownStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eWin:
                    m_AllState[i].AllThisState.Add(new CWinStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eDeath:
                    m_AllState[i].AllThisState.Add(new CDeathStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eFinish:
                    m_AllState[i].AllThisState.Add(new CFinishStateBase(this));
                    break;
                case CMovableStatePototype.EMovableState.eHit:
                    m_AllState[i].AllThisState.Add(new CHitStateBase(this));
                    break;
            }
        }

        m_MyMemoryShare.m_AllState = m_AllState;
    }

    

    protected void AwakeOK()
    {
        AwakeEndSetNullState();

        for (int i = 0; i < m_AllCreateList.Length; i++)
        {
            if (m_AllCreateList[i] == null)
                m_AllCreateList[i] = m_NullDelCreateFunc[i];
        }
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

        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
        {
            lTempDataState.AllThisState[lTempDataState.index].updataMovableState();

            for (int i = 0; i < m_CurAllBuff.Count; i++)
                m_CurAllBuff[i].updataMovableState();
        }

        ChangStateFunc();

        if (NextFramChangState != CMovableStatePototype.EMovableState.eMax)
        {
            m_ChangState = m_NextFramChangState;
            NextFramChangState = CMovableStatePototype.EMovableState.eMax;
        }

        m_MyMemoryShare.m_OldPos = transform.position;
    }

    public virtual void ChangStateFunc()
    {

        if (ChangState != CMovableStatePototype.EMovableState.eMax || ChangStateinndex != -1)
        {
            SetCurState(m_ChangState, m_ChangStateinndex);
            
        }
    }

    protected virtual void LateUpdate()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
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

    public virtual void SetCurState(CMovableStatePototype.EMovableState pamState, int stateindex = -1)
    {
        if (pamState == CurState && !SameStatusUpdate)
            return;

        m_ChangState = CMovableStatePototype.EMovableState.eMax;
        m_ChangStateinndex = -1;
        CMovableStatePototype.EMovableState lTempOldState = CurState;
        m_CurState = pamState;
        m_MyMemoryShare.m_OldDataState.AllThisState = m_AllState[(int)lTempOldState].AllThisState;
        m_MyMemoryShare.m_OldDataState.index = m_AllState[(int)lTempOldState].index;

        DataState lTempDataState = null;

        lTempDataState = m_AllState[(int)lTempOldState];
        if (lTempOldState != CMovableStatePototype.EMovableState.eNull)
        {
            if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].OutMovableState();
        }

        lTempDataState = m_AllState[(int)m_CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull)
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

        if (m_CurState != CMovableStatePototype.EMovableState.eNull)
        {
            if (lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].updataMovableState();
        }
    }

    public virtual void AddBuff(CMovableBuffPototype.EMovableBuff pamAddBuff)
    {
        foreach (CMovableBuffPototype CAB in m_CurAllBuff)
        {
            if (CAB.BuffType() == pamAddBuff)
                return;
        }

        CMovableBuffPototype lTempCreaterBuff = m_AllCreateList[(int)pamAddBuff]();
        if (lTempCreaterBuff == null)
            return;

        lTempCreaterBuff.InMovableState();
        m_CurAllBuff.Add(lTempCreaterBuff);
    }

    public virtual void RemoveBuff(CMovableBuffPototype pamremoveBuff)
    {
        bool lTempRemoveOK = m_CurAllBuff.Remove(pamremoveBuff);
        if (!lTempRemoveOK)
            return;

        pamremoveBuff.RemoveMovableBuff();
    }

    public virtual void ERemoveBuff(CMovableBuffPototype.EMovableBuff pamAddBuff)
    {
        foreach (CMovableBuffPototype CAB in m_CurAllBuff)
        {
            if (CAB.BuffType() == pamAddBuff)
            {
                m_CurAllBuff.Remove(CAB);
                CAB.RemoveMovableBuff();
                return;
            }
        }
    }

    public void DestroyObj(){StartCoroutine(StartCoroutineDestroyObj());}
    IEnumerator StartCoroutineDestroyObj()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    public void DestroyScript() { StartCoroutine(StartCoroutineDestroyScript()); }
    IEnumerator StartCoroutineDestroyScript()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this);
    }

    public virtual void TouchBouncingBed(Collider other)
    {
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerStay(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].OnTriggerExit(other);
    }


    public virtual void OnCollisionEnter(Collision other)
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
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
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDown();

    }

    public virtual void OnMouseDrag()
    {
        m_MyMemoryShare.m_CurmousePosition = Input.mousePosition;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();
    }

    public virtual void OnMouseUp()
    {
        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
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
