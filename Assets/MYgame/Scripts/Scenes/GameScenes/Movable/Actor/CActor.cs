using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActorBaseListData
{
    public List<CActor> m_ActorBaseListData = new List<CActor>();
}

public class CActorMemoryShare : CMemoryShareBase
{
    public CActor       m_Target            = null;
    public CActor       m_MyActor           = null;
    public int          m_Hp                = 10;
    public Vector3      m_DeathImpactDir    = Vector3.forward;
    public GameObject   m_AllObj            = null;
    public Rigidbody[]  m_MyActorRigidbody  = null;
    public Collider[]   m_MyActorCollider   = null;
    public bool         m_EnabledRagdoll    = false;
};

public abstract class CActor : CMovableBase
{
    public enum EActorType
    {
        ePlayer     = 0,
        eEnemyRifle = 1,
        eMax
    };

    protected int m_ActorBasIndex = -1;
    public int ActorBasIndex
    {
        set { m_ActorBasIndex = value; }
        get { return m_ActorBasIndex; }
    }

    abstract public EActorType MyActorType();
    public override EMovableType MyMovableType() { return  EMovableType.eActor; }
    protected CActorMemoryShare m_MyActorMemoryShare = null;

    // ==================== SerializeField ===========================================

    [SerializeField] protected GameObject m_AllObj = null;

    // ==================== SerializeField ===========================================

    public virtual int TargetMask() { return 0; }
    public virtual int TargetIndex() { return 0; }

    protected CAnimatorStateCtl m_AnimatorStateCtl = null;
    public CAnimatorStateCtl AnimatorStateCtl
    {
        set { m_AnimatorStateCtl = value; }
        get { return m_AnimatorStateCtl; }
    }

    public int ActorTypeDataHp
    {
        set { m_MyActorMemoryShare.m_Hp = value; }
        get { return m_MyActorMemoryShare.m_Hp; }
    }

    public Vector3 DeathImpactDir
    {
        set
        {
            m_MyActorMemoryShare.m_DeathImpactDir = value;
            m_MyActorMemoryShare.m_DeathImpactDir.y = 0.0f;
        }
        get { return m_MyActorMemoryShare.m_DeathImpactDir; }
    }


    protected override void CreateMemoryShare()
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
        m_MyActorMemoryShare.m_AllObj = m_AllObj;

        SetBaseMemoryShare();

        if (AnimatorStateCtl != null)
        {
            m_MyActorMemoryShare.m_MyActorCollider = AnimatorStateCtl.GetComponentsInChildren<Collider>(true);
            m_MyActorMemoryShare.m_MyActorRigidbody = AnimatorStateCtl.GetComponentsInChildren<Rigidbody>(true);
        }
        EnabledRagdoll(false);
    }


    protected override void Start()
    {
        m_MyGameManager.AddActorBaseListData(this);
        base.Start();
    }

    protected override void OnDestroy()
    {
        m_MyGameManager.RemoveActorBaseListData(this);
        base.OnDestroy();
    }

    public void EnabledRagdoll(bool enabled)
    {
        if (m_AnimatorStateCtl == null)
            return;

        if (m_AnimatorStateCtl.m_ThisAnimator != null)
            m_AnimatorStateCtl.m_ThisAnimator.enabled = !enabled;

        m_MyActorMemoryShare.m_MyCollider.enabled = !enabled;
        m_MyActorMemoryShare.m_EnabledRagdoll = enabled;

        foreach (Rigidbody rb in m_MyActorMemoryShare.m_MyActorRigidbody)
            rb.isKinematic = !enabled;

        foreach (Collider cr in m_MyActorMemoryShare.m_MyActorCollider)
            cr.enabled = enabled;
    }

    public void AddRagdolldForce(Vector3 Force)
    {
        foreach (Rigidbody rb in m_MyActorMemoryShare.m_MyActorRigidbody)
        {
            rb.AddForce(Force);
        }
    }


    public void AddRagdolldForcePos(Vector3 Force, Vector3 pos)
    {
        foreach (Rigidbody rb in m_MyActorMemoryShare.m_MyActorRigidbody)
            rb.AddForceAtPosition(Force, pos);
    }

    public void AddRagdolldForce(float explosionForce, Vector3 explosionPosition)
    {
        foreach (Rigidbody rb in m_MyActorMemoryShare.m_MyActorRigidbody)
        {
            rb.AddExplosionForce(explosionForce, explosionPosition, 2.0f);
        }
    }
}
