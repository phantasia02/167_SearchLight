using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

public class CGameSceneWindow : CSingletonMonoBehaviour<CGameSceneWindow>
{
    public enum EButtonState
    {
        eNormal     = 0,
        eDisable    = 1,
        eHide       = 2,
    }

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    [SerializeField] GameObject m_ShowObj           = null;
    [SerializeField] Button m_ResetButton           = null;
    [SerializeField] Button m_GoButton              = null;
    [SerializeField] Image m_FragmentSprogressRate  = null;

    [SerializeField] Text m_CurLevelText = null;

    //const float
    protected float m_CurFever      = 0.0f;
    protected float m_TargetFever   = 0.0f;


    private void OnValidate()
    {
    }

    public void SetTemptextPos(Vector3 pos)
    {
    }

    private void Awake()
    {
        m_ResetButton.onClick.AddListener(() => {
            m_ChangeScenes.ResetScene();
        });

        m_GoButton.onClick.AddListener(() => {
            SetGoButton( EButtonState.eHide);
        });

        CSaveManager lTempCSaveManager = CSaveManager.SharedInstance;
        if (lTempCSaveManager != null)
            m_CurLevelText.text = $"{ GlobalData.g_ShowCurLevelNamePrefix + (SceneManager.GetActiveScene().buildIndex).ToString()}";

        CGameManager lTempGameManager = this.GetComponentInParent<CGameManager>();
        if (lTempGameManager == null)
            return;

        CGGameSceneData lTempGameSceneData = CGGameSceneData.SharedInstance;
        if (lTempGameSceneData == null)
            return;

    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateFeverBar()
    {
    }

    public bool GetShow() { return m_ShowObj.activeSelf; }
    public void ShowObj(bool showObj)
    {
        if (showObj)
        {

        }
        m_ShowObj.SetActive(showObj);
        // CGameSceneWindow.SharedInstance.SetCurState(CGameSceneWindow.EState.eEndStop);
    }

    public void SetGoButton(EButtonState settype)
    {
        if (settype == EButtonState.eDisable)
        {
            m_GoButton.gameObject.SetActive(true);
            m_GoButton.interactable = false;
        }
        else if (settype == EButtonState.eHide)
        {
            m_GoButton.gameObject.SetActive(false);
        }
    }


    public void UpdateFragmentSprogressRate(float Val)
    {
        if (Val < 0.0f)
            Val = 0.0f;

        if (Val >= 1.0f)
            Val = 1.0f;

        m_FragmentSprogressRate.fillAmount = Val;
    }
}
