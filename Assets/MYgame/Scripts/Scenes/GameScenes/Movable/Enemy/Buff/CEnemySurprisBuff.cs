using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CEnemySurprisBuff : CEnemyBuffBase
{
    public override EMovableBuff BuffType() { return EMovableBuff.eSurpris; }
    public override float BuffMaxTime() { return 1.5f; }
    public Animator m_SurprisAnimator = null;

    public CEnemySurprisBuff(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void AddBuff()
    {
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(true);

        m_SurprisAnimator = m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.GetComponent<Animator>();
        m_SurprisAnimator.enabled = true;
    }

    protected override void updataState()
    {
        //if (MomentinTime(1.5f))
        //    m_MyEnemyBaseMemoryShare.m_MyActor.RemoveBuff(this);
    }

    protected override void RemoveBuff()
    {
        m_SurprisAnimator.Rebind();
        m_SurprisAnimator.enabled = false;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(false);
    }
}
