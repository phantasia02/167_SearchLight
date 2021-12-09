using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyRifle : CEnemyBase
{
    public override EEnemyType MyEnemyType() { return EEnemyType.eEnemyRifle; }

    protected override void AddInitState()
    {
        base.AddInitState();
    }

}
