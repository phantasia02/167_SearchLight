using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSurprisBuffBase : CMovableBuffPototype
{
    public override EMovableBuff BuffType() { return EMovableBuff.eSurpris; }

    public CSurprisBuffBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    //protected override void AddBuff() { }

    //protected override void updataState() { }

    //public override void LateUpdate() { }

    //protected override void RemoveBuff() { }
}
