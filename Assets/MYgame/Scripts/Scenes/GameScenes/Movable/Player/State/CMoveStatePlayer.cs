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
        if (lTempposition.x < -CPlayer.CsLightDisMaxX)
            lTempposition.x = -CPlayer.CsLightDisMaxX;

        if (lTempposition.x > CPlayer.CsLightDisMaxX)
            lTempposition.x = CPlayer.CsLightDisMaxX;

        if (lTempposition.z > CPlayer.CsLightDisMaxZ)
            lTempposition.z = CPlayer.CsLightDisMaxZ;

        if (lTempposition.z < CPlayer.CsLightDisMinZ)
            lTempposition.z = CPlayer.CsLightDisMinZ;

        m_MyPlayerMemoryShare.m_PlayCtrlLight.position = lTempposition;

        m_MyPlayerMemoryShare.m_MyPlayer.UpdateSearchLightDir();
        CheckIrradiateEnemy();
    }

    protected override void OutState()
    {

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
