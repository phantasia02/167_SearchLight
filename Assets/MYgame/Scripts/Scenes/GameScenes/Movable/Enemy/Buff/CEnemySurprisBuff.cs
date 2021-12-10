using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CEnemySurprisBuff : CEnemyBuffBase
{
    public override EMovableBuff BuffType() { return EMovableBuff.eSurpris; }
    public const float m_EndTime = 0.2f;
    public const float m_SizeScale = 1.5f;

    public CEnemySurprisBuff(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void AddBuff()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(true);

        RectTransform lTempRectTransform = m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].rectTransform;
        lTempRectTransform.localScale = new Vector3(m_SizeScale, 0.0f, m_SizeScale);
        lTempRectTransform.DOScale(Vector3.one * m_SizeScale, m_EndTime).SetEase(Ease.OutBounce);
    }

    protected override void updataState()
    {
        if (MomentinTime(m_EndTime + 1.0f))
            m_MyEnemyBaseMemoryShare.m_MyActor.RemoveBuff(this);
    }

    //public override void LateUpdate() { }

    protected override void RemoveBuff()
    {
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(false);
    }
}
