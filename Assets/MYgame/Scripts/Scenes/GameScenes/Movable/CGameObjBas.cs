using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameObjBasListData
{
    public List<CGameObjBas> m_GameObjBasListData = new List<CGameObjBas>();
    public Hashtable m_GameObjBasHashtable = new Hashtable();
}

public abstract class CGameObjBas : MonoBehaviour
{
    public enum EObjType
    {
        eMovable            = 0,
        ePlayer             = 1,
        eEnemy              = 2,
        eMax
    }

    abstract public EObjType ObjType();
    protected Transform m_OriginalParent = null;
    protected CGameManager m_MyGameManager = null;

    protected int m_GameObjBasIndex = -1;
    public int GameObjBasIndex
    {
        set { m_GameObjBasIndex = value; }
        get { return m_GameObjBasIndex; }
    }

    protected virtual void Awake()
    {
        m_MyGameManager = GetComponentInParent<CGameManager>();

        if (m_MyGameManager == null)
            m_MyGameManager = GameObject.FindObjectOfType<CGameManager>();

        m_OriginalParent = gameObject.transform.parent;
        m_MyGameManager.AddGameObjBasListData(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public virtual void Init()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnDestroy()
    {
        m_MyGameManager.RemoveGameObjBasListData(this);
    }
}
