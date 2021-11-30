using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCode : MonoBehaviour
{
    // ==================== SerializeField ===========================================

    [SerializeField] protected Transform m_Player = null;
    [SerializeField] protected Renderer m_MyMeshRenderer = null;

    // ==================== SerializeField ===========================================
    static readonly int PlayerPos = Shader.PropertyToID("_PlayerLightPos");
    static readonly int PlayerPosfloatID = Shader.PropertyToID("_PlayerLight2");

    Material m_MyMain = null;

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
        m_MyMain = m_MyMeshRenderer.material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Mpb.SetVector(PlayerPos, m_Player.position);
     
        m_MyMeshRenderer.SetPropertyBlock(Mpb);
        // m_MyMain.
    }
}
