using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CMoveStateBase : CMovableStatePototype
{

    public override EMovableState StateType() { return EMovableState.eMove; }

    public CMoveStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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

    public override void UpdateOriginalAnimation()
    {
      
    }
}
