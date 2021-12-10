using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate CMovableBuffPototype DelCreateBuff();

public abstract class CMovableBuffPototype
{
    public enum EMovableBuff
    {
        eSurpris = 0,
        eMax
    }

    protected CMovableBase m_MyMovable = null;
    protected CGameManager m_MyGameManager = null;
    protected CMemoryShareBase m_MyMemoryShare = null;

    abstract public EMovableBuff BuffType();

    protected float m_CurTime           = 0.0f;
    protected float m_CurUnscaledTime   = 0.0f;
    protected int   m_CurCount          = 0;

    protected float m_OldBuffTime           = 0.0f;
    protected float m_OldBuffUnscaledTime   = 0.0f;
    protected int   m_OldBuffCount          = 0;

    public virtual float BuffMaxTime() { return 10.0f; }

    public CMovableBuffPototype(CMovableBase pamMovableBase)
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
        m_OldBuffTime           = m_CurTime         = 0.0f;
        m_OldBuffUnscaledTime   = m_CurUnscaledTime = 0.0f;
        m_OldBuffCount          = m_CurCount        = 0;
    }

    public void InMovableState()
    {
        AddBuff();
        ClearTime();
    }

    public void updataMovableState()
    {
        m_CurTime           += Time.deltaTime;
        m_CurUnscaledTime   += Time.unscaledDeltaTime;
        m_CurCount++;

        updataState();

        if (BuffMaxTime() > 0.0f && BuffMaxTime() <= m_CurUnscaledTime)
            RemoveMovableBuff();

        m_OldBuffTime           = m_CurTime;
        m_OldBuffUnscaledTime   = m_CurUnscaledTime;
        m_OldBuffCount          = m_CurCount;
    }

    public virtual void UpdateOriginalAnimation()
    {
        
    }

    public void RemoveMovableBuff()
    {
        RemoveBuff();
    }

    public bool MomentinTime(float time) { return m_OldBuffTime < time && m_CurTime >= time; }


    protected virtual void AddBuff() { }

    protected virtual void updataState() { }

    public virtual void LateUpdate() { }

    protected virtual void RemoveBuff() { }
}
