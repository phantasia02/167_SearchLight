using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CATKStateEnemyBulletFlyObj : CStateBulletFlyObjBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }

    Renderer m_RendererMesh = null;

    public CATKStateEnemyBulletFlyObj(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_RendererMesh = m_MyBulletFlyObjMemoryShare.m_MyMovable.GetComponentInChildren<Renderer>();
        if (m_RendererMesh == null)
            return;

       // m_RendererMesh
    }

    protected override void updataState()
    {
 
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        //if (other.tag == StaticGlobalDel.TagEnemy || other.tag == StaticGlobalDel.TagFloor)
        //{
        //    GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable.gameObject);
        //}
    }
}
