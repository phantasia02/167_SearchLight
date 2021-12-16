using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMovableStateData{}

public abstract class CMovableStatePototype
{
    public enum EMovableState
    {
        eNull       = 0,
        eWait       = 1,
        eDrag       = 2,
        eMove       = 3,
        eAtk        = 4,
        eJump       = 5,
        eJumpDown   = 6,
        eHit        = 7,
        eWin        = 8,
        eDeath      = 9,
        eFinish     = 10,
        eFlee       = 11,
        eMax
    }

    protected CMovableBase m_MyMovable = null;
    protected CGameManager m_MyGameManager = null;
    protected CMemoryShareBase m_MyMemoryShare = null;

    public virtual int Priority => 0;
    abstract public EMovableState StateType();

    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected float m_OldStateTime = 0.0f;
    protected float m_OldStateUnscaledTime = 0.0f;
    protected int m_OldStateCount = 0;


    protected Vector3 m_v3DownPos = Vector3.zero;
    protected bool m_bDownOKPos = false;

    protected Vector3[] COffsetDownPos = new Vector3[4]{ Vector3.forward * 2.0f, Vector3.right * 2.0f, Vector3.forward * -2.0f, Vector3.right * -2.0f};

    public CMovableStatePototype(CMovableBase pamMovableBase)
    {
        if (pamMovableBase == null)
            return;

        
        m_MyMovable = pamMovableBase;
        m_MyGameManager = m_MyMovable.GetComponentInParent<CGameManager>();
        if (m_MyGameManager == null)
            m_MyGameManager = GameObject.FindObjectOfType<CGameManager>();

        m_MyMemoryShare = pamMovableBase.MyMemoryShare;
    }

    public void ClearTime()
    {
       m_OldStateTime          = m_StateTime         = 0.0f;
       m_OldStateUnscaledTime  = m_StateUnscaledTime = 0.0f;
       m_OldStateCount         = m_StateCount        = 0;
    }

    public void InMovableState()
    {
        InState();
        ClearTime();
    }

    public void updataMovableState()
    {
        m_OldStateTime          = m_StateTime;
        m_OldStateUnscaledTime  = m_StateUnscaledTime;
        m_OldStateCount         = m_StateCount;

        m_StateTime         += Time.deltaTime;
        m_StateUnscaledTime += Time.unscaledDeltaTime;
        m_StateCount++;
        updataState();
    }

    public virtual void UpdateOriginalAnimation()
    {

    }

    public void OutMovableState()
    {
        OutState();
    }

    public bool MomentinTime(float time){return m_OldStateTime < time && m_StateTime >= time;}

    public EMovableState RandomState(EMovableState[] RandomList)
    {
        if (RandomList == null)
            return EMovableState.eMax;

        return RandomList[Random.Range(0, RandomList.Length - 1)];
    }

    protected virtual void InState() {}

    protected virtual void updataState(){}

    public virtual void LateUpdate(){}

    protected virtual void OutState(){}

    public virtual void Input(RaycastHit hit){}

    public virtual void OnTriggerEnter(Collider other){}

    public virtual void OnTriggerExit(Collider other){}

    public virtual void OnTriggerStay(Collider other){}

    public virtual void OnCollisionEnter(Collision collision){}

    public virtual void MouseDown(){}
    public virtual void MouseDrag(){}
    public virtual void MouseUp(){}

    public bool FloatingToFloorChack()
    {
        return false;
    }

}
