using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EAllFXType
    {
        eExplodePos         = 0,

        eMax,
    };

    public enum EOtherObj
    {
        eBulletFlyObj       = 0,
        eSpark              = 1,
        eEnemyATKEffect     = 2,
        eOneEnemyIcon       = 3,
        eMax,
    };

    [SerializeField]  public GameObject[]    m_AllFX                 = null;
    [SerializeField]  public GameObject[]    m_AllOtherObj           = null;
    [SerializeField]  public GameObject[]    m_AllCEnemyTypeObj      = null;

    private void Awake()
    {
        //for (int i = 0; i < (int)EArmsType.eMax; i++)
        //{
        //    m_CurNewArmsCount = i;
        //    m_AllArmsPool[i] = new CObjPool<GameObject>();
        //    m_AllArmsPool[i].NewObjFunc = NewArms;
        //    m_AllArmsPool[i].RemoveObjFunc = RemoveArms;
        //    m_AllArmsPool[i].InitDefPool(10);
        //}
    }
}
