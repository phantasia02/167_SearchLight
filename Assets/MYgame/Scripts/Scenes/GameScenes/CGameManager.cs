using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UniRx;

public class CGameManager : MonoBehaviour
{
    public enum EState
    {
        eReady              = 0,
        ePlay               = 1,
        ePlayHold           = 2,
        ePlayOKPerformance  = 3,
        eReadyEnd           = 4,
        eNextEnd            = 5,
        eGameOver           = 6,
        eWinUI              = 7,
        eMax
    };
    
    bool m_bDown = false;

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    protected ResultUI m_MyResultUI = null;
    public ResultUI MyResultUI { get { return m_MyResultUI; } }

    protected Camera m_Camera = null;
    public Camera MainCamera { get { return m_Camera; } }

    protected CPlayer m_Player = null;
    public CPlayer Player { get { return m_Player; } }
    // ==================== SerializeField ===========================================
    [SerializeField] protected CStageType m_StageTypeData = null;
    public CStageType StageTypeData { get { return m_StageTypeData; } }

    //[SerializeField] GameObject m_WinCamera = null;
    //public GameObject WinCamera { get { return m_WinCamera; } }

    [SerializeField] protected int m_AnswerIndex = 0;
    public int AnswerIndex { get { return m_AnswerIndex; } }
    [Header("Result OBJ")]
    [SerializeField] protected GameObject m_WinObjAnima     = null;
    [SerializeField] protected GameObject m_OverObjAnima    = null;
    // ==================== SerializeField ===========================================

    protected bool isApplicationQuitting = false;
    public bool GetisApplicationQuitting { get { return isApplicationQuitting; } }

    int m_AllFragmentsMax = 100;

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;


    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);

        m_Player = this.GetComponentInChildren<CPlayer>();
        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>();

        if (m_MyResultUI != null)
        {
            m_MyResultUI.NextButton.onClick.AddListener(OnNext);
            m_MyResultUI.OverButton.onClick.AddListener(OnReset);
        }

        GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (lTempCameraObj != null)
            m_Camera = lTempCameraObj.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {


        Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(
        OnNext =>
        {
            if (m_eCurState == EState.eReady)
            {
                SetState(EState.ePlay);

                CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
                if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
                    lTempCReadyGameWindow.CloseShowUI();

                CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                if (lTempGameSceneWindow)
                {

                }
            }
        },
        OnCompleted => { Debug.Log("OK"); }
        ).AddTo(this);


    }

    // Update is called once per frame
    void Update()
    {

        m_StateTime += Time.deltaTime;
        m_StateCount++;
        m_StateUnscaledTime += Time.unscaledDeltaTime;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                  //  UsePlayTick();
                }
                break;
            case EState.ePlay:
                {
                  //  UsePlayTick();
                }
                break;
            case EState.ePlayHold:
                {

                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;

            case EState.eReadyEnd:
                {
                }
                break;


            case EState.eNextEnd:
                {
                }
                break;
            case EState.eGameOver:
                {

                }
                break;
        }
    }

    public void SetState(EState lsetState)
    {
        if (lsetState == m_eCurState)
            return;

        if (m_eCurState == EState.eWinUI || m_eCurState == EState.eGameOver)
            return;

        EState lOldState = m_eCurState;
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
        m_eCurState = lsetState;

        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                }
                break;
            case EState.ePlay:
                {
                    if (lTempGameSceneWindow != null)
                        lTempGameSceneWindow.SetGoButton(CGameSceneWindow.EButtonState.eNormal);
                }
                break;
            case EState.ePlayHold:
                {
                    if (lTempGameSceneWindow != null)
                        lTempGameSceneWindow.SetGoButton(CGameSceneWindow.EButtonState.eDisable);
                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;
            case EState.eReadyEnd:
                {
                }
                break;
            case EState.eNextEnd:
                {

                }
                break;
            case EState.eWinUI:
                {
                    if (lTempGameSceneWindow)
                    {
                       // lTempGameSceneWindow.ShowObj(false);
                    }

                  //  m_AllGroupQuestionHole[0].ChangeQuestionHoleState( CQuestionHole.ECurShowState.eFinish);
                  //  ChangeQuestionHoleState
                    m_MyResultUI.ShowSuccessUI(0.0f);
                }
                break;
            case EState.eGameOver:
                {
                    //if (lTempGameSceneWindow)
                    //    lTempGameSceneWindow.ShowObj(false);
                  
                    m_MyResultUI.ShowFailedUI(1.0f);
                }
                break;
        }
    }

    public void UsePlayTick()
    {
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            m_bDown = true;
            m_OldInput = Input.mousePosition;
            //InputRay();
           // OKAllGroupQuestionHole(0);
        }
        else if (Input.GetMouseButton(0))
        {
            //float moveX = (Input.mousePosition.x - m_OldInput.x) / m_HalfScreenWidth;
            //m_Player.SetXMove(moveX);
            //m_OldInput = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_bDown)
            {
                m_OldInput = Vector3.zero;
                m_bDown = false;
            }
        }

    }
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable
    List<string> testbugtext = new List<string>();
    private void OnGUI()
    {
        string test = "";
        for (int i = testbugtext.Count - 1; i >= 0; i--)
            test += $"{testbugtext[i]}\n";

        guiStyle.fontSize = 60; //change the font size
        GUI.Label(new Rect(10, 10, 1000, 2000), test, guiStyle);
    }

    public void InputRay()
    {
        
    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

    void OnApplicationQuit() { isApplicationQuitting = true; }

    public void SetWinUI()
    {
        SetState(EState.eWinUI);
    }

    public void SetLoseUI()
    {
        SetState(EState.eGameOver);
    }
}
