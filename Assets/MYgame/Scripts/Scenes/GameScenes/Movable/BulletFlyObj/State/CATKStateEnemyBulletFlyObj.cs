using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CATKStateEnemyBulletFlyObj : CStateBulletFlyObjBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }

    protected Renderer              m_RendererMesh = null;
    protected MaterialPropertyBlock m_MaterialProperty;
    protected readonly int          _EmissionColor = Shader.PropertyToID("_EmissionColor");
    protected readonly int          _BaseColor = Shader.PropertyToID("_BaseColor");
    protected Color                 m_color;
    
    public CATKStateEnemyBulletFlyObj(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MaterialProperty = new MaterialPropertyBlock();

        Vector3 LaunchtoPlayCtrlLight = m_MyGameManager.Player.SearchlightTDObj.transform.position - m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.position;
        Vector3 LaunchtoPlayCtrlLightDir = LaunchtoPlayCtrlLight.normalized;

        m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.parent = m_MyGameManager.AllBulletParent;
        m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.forward = LaunchtoPlayCtrlLightDir;
        m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.localScale = new Vector3(1.0f, 1.0f, LaunchtoPlayCtrlLight.magnitude);

        Ray lTempRay = new Ray(m_MyBulletFlyObjMemoryShare.m_MyMovable.transform.position, LaunchtoPlayCtrlLightDir);
        if (Physics.Raycast(lTempRay, out RaycastHit hit, StaticGlobalDel.g_ePlayerMask))
        {
            Transform lTempSparkEffect = StaticGlobalDel.NewOtherObjAddParentShow(m_MyBulletFlyObjMemoryShare.m_MyMovable.transform, CGGameSceneData.EOtherObj.eSpark);
            lTempSparkEffect.parent = null;
            lTempSparkEffect.up = -hit.normal;
            lTempSparkEffect.position = hit.point;
            lTempSparkEffect.localScale = Vector3.one * 5.0f;
        }


        // ====================== Renderer ======================
        m_RendererMesh = m_MyBulletFlyObjMemoryShare.m_MyMovable.GetComponentInChildren<Renderer>();
        if (m_RendererMesh == null)
            return;

        m_RendererMesh.material.EnableKeyword("_EMISSION");
        m_color = m_RendererMesh.material.GetColor(_BaseColor);
        Color lTempEndCo = new Color();
        lTempEndCo.r = m_color.r;
        lTempEndCo.g = m_color.g;
        lTempEndCo.b = m_color.b;
        lTempEndCo.a = 0.0f;

        CEnemyBulletFlyObj lTempPlayerBulletFlyObj = (CEnemyBulletFlyObj)m_MyBulletFlyObjMemoryShare.m_MyBulletFlyObj;
        AnimationCurve lTempAnimationCurve = lTempPlayerBulletFlyObj.curve;

        Tween lTempTween = null;
        lTempTween = DOTween.To(
         () => m_color, x => m_color = x,
         lTempEndCo, 0.5f);

        lTempTween.SetEase(lTempAnimationCurve);
        lTempTween.OnUpdate(
            ()=> {
                m_MaterialProperty.SetColor(_BaseColor, m_color);
                m_RendererMesh.SetPropertyBlock(m_MaterialProperty);
            });

      //  lTempTween.onComplete = () => {m_MyBulletFlyObjMemoryShare.m_MyMovable.DestroyObj();};
    }

    protected override void updataState()
    {
 
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        //if (other.tag == StaticGlobalDel.TagEnemy || other.tag == StaticGlobalDel.TagFloor)
        //{
        //    GameObject.Destroy(m_MyBulletFlyObjMemoryShare.m_MyMovable.gameObject);
        //}
    }
}
