using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSetActorOtherTransform : MonoBehaviour
{
    [SerializeField] protected int m_SetOtherIndex = 0;

    private void Start()
    {
        CActor lTempActor = this.GetComponentInParent<CActor>();
        if (lTempActor == null)
            return;

        lTempActor.SetOtherTransform(this.transform, m_SetOtherIndex);
    }
}
