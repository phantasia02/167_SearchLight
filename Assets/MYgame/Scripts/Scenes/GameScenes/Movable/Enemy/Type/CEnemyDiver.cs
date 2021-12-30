using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CEnemyDiver : CEnemyBase
{
    public const float CsYDifMove = -2.2f;
    public const float CsYDifMoveBuff = CsYDifMove + 0.1f;

    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyDiver; }
    public override float DefSpeed { get { return 1.0f; } }


    // ==================== SerializeField ===========================================

    [SerializeField] protected CDataRendererMat m_WaterShadow = new CDataRendererMat();

    // ==================== SerializeField ===========================================
    Tween m_ShadowTweenBuff = null;

    public override bool AddCurDiscoveryTime(float addTime){return false;}

    protected override void AddInitState()
    {
        m_AllState[(int)CMovableStatePototype.EMovableState.eWait].AllThisState.Add(new CDiverWaitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eHit].AllThisState.Add(new CHitStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemyBase(this));

        m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CDiverATKStateEnemyBase(this));

        //m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CMoveStateEnemyBase(this));
        m_AllState[(int)CMovableStatePototype.EMovableState.eMove].AllThisState.Add(new CDiverMoveStateEnemyBase(this));


        m_AllCreateList[(int)CMovableBuffPototype.EMovableBuff.eSurpris] = () => { return new CEnemySurprisBuff(this); };
    }

    protected override void CreateMemoryShare()
    {
        base.CreateMemoryShare();

        Color lTempColor = m_WaterShadow.m_Renderer.material.color;
        lTempColor.a = 1.0f;
        m_WaterShadow.Mpb.SetColor(StaticGlobalDel._baseColor, lTempColor);
        m_WaterShadow.m_Renderer.SetPropertyBlock(m_WaterShadow.Mpb);
        m_WaterShadow.m_Renderer.gameObject.SetActive(true);
    }

    public override void ShowOther1(bool show)
    {
        if (m_ShadowTweenBuff != null)
        {
            m_ShadowTweenBuff.Kill();
            m_ShadowTweenBuff = null;
        }

        float lTempEndAlpha = show ? 1.0f : 0.0f;
        float lTempRatio = 0.0f;
        Color lTempColor = m_WaterShadow.m_Renderer.material.color;

        if (show)
        {
            m_WaterShadow.m_Renderer.gameObject.SetActive(true);

            if (lTempColor.a > 0.8f)
            {
                lTempColor.a = 1.0f;
                m_WaterShadow.Mpb.SetColor(StaticGlobalDel._baseColor, lTempColor);
                m_WaterShadow.m_Renderer.SetPropertyBlock(m_WaterShadow.Mpb);
            }
        }
        else
        {
            if (lTempColor.a < 0.2f)
                m_WaterShadow.m_Renderer.gameObject.SetActive(false);
        }
       

        m_ShadowTweenBuff = DOTween.To(() => lTempRatio, x => lTempRatio = x, lTempEndAlpha, 1.0f).SetEase(Ease.Linear);
        m_ShadowTweenBuff.onUpdate = () =>
        {
            lTempColor.a = lTempRatio;
            m_WaterShadow.Mpb.SetColor(StaticGlobalDel._baseColor, lTempColor);
            m_WaterShadow.m_Renderer.SetPropertyBlock(m_WaterShadow.Mpb);
        };

        if (!show)
        {
            m_ShadowTweenBuff.onComplete = () => {m_WaterShadow.m_Renderer.gameObject.SetActive(false);};
            m_ShadowTweenBuff.onKill = () => 
            {
                m_WaterShadow.m_Renderer.gameObject.SetActive(false);
                m_ShadowTweenBuff = null;
            };
        }
    }
}
