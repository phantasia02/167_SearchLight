using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAllEnemyUI : MonoBehaviour
{
    public const float CFMaxX = 1200.0f;
    public const float CFEnemyIconSpacing      = 50.0f;
    public const float CFEnemyIconSpacingHalf  = CFEnemyIconSpacing * 0.5f;
    protected List<COneEnemyIcon> m_AllEnemyUI = new List<COneEnemyIcon>();

    public void AddEnemy()
    {
        RectTransform lTempNewOneEnemyIcon = (RectTransform)StaticGlobalDel.NewOtherObjAddParentShow(this.transform, CGGameSceneData.EOtherObj.eOneEnemyIcon);
        COneEnemyIcon lTempOneEnemyIcon = lTempNewOneEnemyIcon.GetComponent<COneEnemyIcon>();

        m_AllEnemyUI.Add(lTempOneEnemyIcon);
        lTempNewOneEnemyIcon.parent = this.transform;
        lTempNewOneEnemyIcon.anchoredPosition = Vector3.zero;
    }

    public void RemoveEnemy()
    {
        COneEnemyIcon lTempRemoveEnemy = m_AllEnemyUI[m_AllEnemyUI.Count - 1];
        m_AllEnemyUI.Remove(lTempRemoveEnemy);
        lTempRemoveEnemy.ShowSurvive(false);
    }

    public void UpdateLayout()
    {
        if (m_AllEnemyUI.Count > 1)
        {
            float lTempAllXSizeHalf = m_AllEnemyUI.Count * CFEnemyIconSpacing * 0.5f;
            float lTempCurX = -lTempAllXSizeHalf + CFEnemyIconSpacingHalf;
            RectTransform lTempOneEnemyIconRectTransform = null;
            for (int i = 0; i < m_AllEnemyUI.Count; i++)
            {
                lTempOneEnemyIconRectTransform = (RectTransform)m_AllEnemyUI[i].transform;
                lTempOneEnemyIconRectTransform.anchoredPosition = new Vector3(lTempCurX, 0.0f, 0.0f);
                lTempCurX += CFEnemyIconSpacing;
            }
        }
    }

}
