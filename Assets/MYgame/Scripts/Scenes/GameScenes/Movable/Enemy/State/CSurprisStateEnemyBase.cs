using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CSurprisStateEnemyBase : CEnemyStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eHit; }

    public CSurprisStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_WasFound = true;
        m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].gameObject.SetActive(true);

        RectTransform lTempRectTransform = m_MyEnemyBaseMemoryShare.m_AllEmoticons[0].rectTransform;
        lTempRectTransform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        lTempRectTransform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBounce);
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
