using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CWinStatePlayer : CPlayerStateBase
{
    static readonly int EmissionColor = Shader.PropertyToID("_BaseColor");

    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eWin; }

    public CWinStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        EnabledCollisionTag(false);

        //StaticGlobalDel.SetMaterialRenderingMode(m_MyPlayerMemoryShare.m_MyMainMaterial, RenderingMode.Transparent);
        m_MyPlayerMemoryShare.m_MyRigidbody.drag = 1000.0f;
        m_MyPlayerMemoryShare.m_MyMainMaterial.DOFade(0.0f, EmissionColor, 1.0f).onComplete = ()=> 
        {
            m_MyPlayerMemoryShare.m_MyMovable.gameObject.SetActive(false);
        };
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
