using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EAllFXType
    {
        eExplosionA         = 0,

        eMax,
    };

    public enum EOtherObj
    {
        eBulletFlyObj       = 0,
        eSpark              = 1,
        eEnemyATKEffect     = 2,
        eOneEnemyIcon       = 3,
        eEnemyGrenade       = 4,
        eMax,
    };

    [SerializeField]  public GameObject[]    m_AllFX                 = null;
    [SerializeField]  public GameObject[]    m_AllOtherObj           = null;
    [SerializeField]  public GameObject[]    m_AllCEnemyTypeObj      = null;
    [SerializeField]  public Material        m_DeathMat              = null;

    private void Awake()
    {
    
    }
}
