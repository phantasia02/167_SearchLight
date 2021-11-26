using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayer : CPlayerStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eMove; }

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

        m_MyPlayerMemoryShare.m_MyTransform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * m_MyPlayerMemoryShare.m_TotleSpeed.Value));

        const float CsMaxX = 4.0f;
        const float CsMaxZ = 11.0f;
        const float CsMinZ = -3.0f;
        Vector3 lTempposition = m_MyPlayerMemoryShare.m_MyTransform.position;
        if (lTempposition.x < -CsMaxX)
            lTempposition.x = -CsMaxX;

        if (lTempposition.x > CsMaxX)
            lTempposition.x = CsMaxX;

        if (lTempposition.z > CsMaxZ)
            lTempposition.z = CsMaxZ;

        if (lTempposition.z < CsMinZ)
            lTempposition.z = CsMinZ;

        m_MyPlayerMemoryShare.m_MyTransform.position = lTempposition;

        m_MyPlayerMemoryShare.m_MyPlayer.UpdateSearchLightDir();
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
        m_MyPlayerMemoryShare.m_MyPlayer.ChangState = StaticGlobalDel.EMovableState.eWait;
    }
}
