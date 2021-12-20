using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COneEnemyIcon : MonoBehaviour
{
    protected Image m_SurviveImage = null;
    protected Image m_DeathImage    = null;

    private void Awake()
    {
        Transform lTempTransform = null;
        lTempTransform = this.transform.Find("SurviveImage");
        m_SurviveImage = lTempTransform.GetComponent<Image>();
        lTempTransform = this.transform.Find("DeathImage");
        m_DeathImage = lTempTransform.GetComponent<Image>();

        ShowSurvive(true);
    }

    public void ShowSurvive(bool Survive)
    {
        m_SurviveImage.gameObject.SetActive(Survive);
        m_DeathImage.gameObject.SetActive(!Survive);
    }
}
