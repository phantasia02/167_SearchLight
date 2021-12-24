using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CGrenadeMoveStateBulletFlyObj : CStateBulletFlyObjBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }
    protected Tween m_lmemTween = null;

    public CGrenadeMoveStateBulletFlyObj(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        float RandomA() { return Random.Range(360.0f, 1080.0f); };

        m_TargetActor = m_MyBulletFlyObjMemoryShare.m_Target.GetComponentInParent<CActor>();

        Vector3 lTempTargetToMy = m_MyBulletFlyObjMemoryShare.m_Target.position - m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.position;

        float lTempDoTime = 0.5f + lTempTargetToMy.magnitude * 0.2f;
        float lTempDoScaleTime = lTempDoTime * 0.95f;
        m_lmemTween = m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.DOJump(m_MyBulletFlyObjMemoryShare.m_Target.position, m_MyBulletFlyObjMemoryShare.m_Target.position.y + 3.0f, 1, lTempDoTime).SetEase( Ease.Linear);
        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.DOScale(Vector3.one * 3.0f, lTempDoScaleTime).SetEase(Ease.Linear);
        m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.DOLocalRotate(new Vector3(RandomA(), 0.0f, RandomA()), lTempDoScaleTime, RotateMode.LocalAxisAdd);
        //m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj.transform.DOLocalRotate(new Vector3(RandomA(), , 0.0f), lTempDoScaleTime, RotateMode.LocalAxisAdd);

        m_lmemTween.onComplete = EndOK;
    }

    protected override void updataState()
    {
       
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_MyBulletFlyObjMemoryShare.m_TargetTag || other.tag == StaticGlobalDel.TagFloor)
        {
            if (m_lmemTween != null)
                m_lmemTween.Kill();

            EndOK();
        }
    }

    public void EndOK()
    {
        Transform lTempSparkEffect = StaticGlobalDel.NewFxAddParentShow(m_MyBulletFlyObjMemoryShare.m_MyMovable.transform, CGGameSceneData.EAllFXType.eExplosionA);
        lTempSparkEffect.parent = null;
        lTempSparkEffect.localScale = Vector3.one * 5.0f;

        GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable.gameObject);
    }
}
