using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CEnemySurprisBuff : CEnemyBuffBase
{
    public override EMovableBuff BuffType() { return EMovableBuff.eSurpris; }

    public CEnemySurprisBuff(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void AddBuff()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(true);

        RectTransform lTempRectTransform = m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].rectTransform;
        lTempRectTransform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        lTempRectTransform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBounce);
    }

    //protected override void updataState() { }

    //public override void LateUpdate() { }

    //protected override void RemoveBuff() { }
}
