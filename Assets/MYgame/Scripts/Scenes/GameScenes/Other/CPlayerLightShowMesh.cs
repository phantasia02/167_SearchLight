using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerLightShowMesh : MonoBehaviour
{
    // ==================== SerializeField ===========================================

    //[SerializeField] protected Transform m_Player = null;
    

    // ==================== SerializeField ===========================================
    static readonly int PlayerPos = Shader.PropertyToID("_PlayerLightPos");
    static readonly int PlayerPosfloatID = Shader.PropertyToID("_PlayerLight2");

    public Transform m_PlayerLight = null;
    public Transform PlayerLight { set { m_PlayerLight = value; } }

    protected Renderer m_MyMeshRenderer = null;
    protected CGameManager m_MyGameManager = null;

    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb
    {
        get
        {
            if (mpb == null)
                mpb = new MaterialPropertyBlock();
            return mpb;
        }
    }

    private void Awake()
    {
        m_MyGameManager = GetComponentInParent<CGameManager>();

        if (m_MyGameManager == null)
            m_MyGameManager = GameObject.FindObjectOfType<CGameManager>();

        m_MyMeshRenderer = this.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    CPlayer lTempPlayer = GameObject.FindObjectOfType<CPlayer>();
    //    PlayerLight = lTempPlayer.PlayCtrlLight;
    //}

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerLight == null)
            return;

        Mpb.SetVector(PlayerPos, m_PlayerLight.position);
        m_MyMeshRenderer.SetPropertyBlock(Mpb);

        if (m_MyGameManager.CurState == CGameManager.EState.eGameOver)
            Destroy(this);
    }
}
