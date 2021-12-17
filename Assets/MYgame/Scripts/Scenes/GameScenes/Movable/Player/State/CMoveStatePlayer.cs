using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }

    public CMoveStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_MyMovable.ResetMoveBuff();
    }

    protected override void updataState()
    {
        UpdateSpeed();

        m_MyPlayerMemoryShare.m_PlayCtrlLight.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * m_MyPlayerMemoryShare.m_TotleSpeed.Value));


        Vector3 lTempposition = m_MyPlayerMemoryShare.m_PlayCtrlLight.position;
        if (lTempposition.x < -StaticGlobalDel.g_CsLightDisMaxX)
            lTempposition.x = -StaticGlobalDel.g_CsLightDisMaxX;

        if (lTempposition.x > StaticGlobalDel.g_CsLightDisMaxX)
            lTempposition.x = StaticGlobalDel.g_CsLightDisMaxX;
        
        if (lTempposition.z > StaticGlobalDel.g_CsLightDisMaxZ)
            lTempposition.z = StaticGlobalDel.g_CsLightDisMaxZ;

        if (lTempposition.z < StaticGlobalDel.g_CsLightDisMinZ)
            lTempposition.z = StaticGlobalDel.g_CsLightDisMinZ;

        m_MyPlayerMemoryShare.m_PlayCtrlLight.position = lTempposition;

        m_MyPlayerMemoryShare.m_MyPlayer.UpdateSearchLightDir();
        CheckIrradiateEnemy();
    }

    protected override void OutState()
    {
        //m_MyPlayerMemoryShare.m_MyMovable.

       // StaticGlobalDel.NewOtherObjAddParentShow
    }

    public override void MouseDown() { }
    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.UpdateDrag();
    }
    public override void MouseUp()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.SetChangState(EMovableState.eWait);
    }
}
