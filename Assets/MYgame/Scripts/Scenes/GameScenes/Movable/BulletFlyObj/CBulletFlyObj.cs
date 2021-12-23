using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletFlyObjMemoryShare : CMemoryShareBase
{
    public CBulletFlyObj    m_MyBulletFlyObj    = null;
    public Transform        m_Target            = null;
    public CGameObjBas      m_Launcher          = null;
    public string           m_TargetTag         = "";
};

public abstract class CBulletFlyObj : CMovableBase
{
    public enum EBulletArms
    {
        eNormalBullet   = 0,
        eGrenade        = 1,
        eMax
    };

    public override EMovableType MyMovableType() { return EMovableType.eBulletFlyObj; }
    abstract public EBulletArms MyBulletArms();

    protected int m_Damages = 2;
    public int Damages => m_Damages;

    protected CBulletFlyObjMemoryShare m_MyBulletFlyObjMemoryShare = null;

    public Transform Target{set => m_MyBulletFlyObjMemoryShare.m_Target = value;}
    public string TargetTag { set => m_MyBulletFlyObjMemoryShare.m_TargetTag = value;}
    public CGameObjBas Launcher { set => m_MyBulletFlyObjMemoryShare.m_Launcher = value;}

    public override float DefSpeed { get { return 20.0f; } }



    protected override void CreateMemoryShare()
    {
        m_MyBulletFlyObjMemoryShare = new CBulletFlyObjMemoryShare();
        m_MyMemoryShare = m_MyBulletFlyObjMemoryShare;
        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj = this;

        SetBaseMemoryShare();
    }

    protected override void Start()
    {
      //  m_MyGameManager.AddActorBaseListData(this);
        base.Start();
    }

    public void SetChildTag(string settag)
    {
        for (int i = 0; i < transform.childCount; i++)
             transform.GetChild(i).tag = settag;
    }
}
