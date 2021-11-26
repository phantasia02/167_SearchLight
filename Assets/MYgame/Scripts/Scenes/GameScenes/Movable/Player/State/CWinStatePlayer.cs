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
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
