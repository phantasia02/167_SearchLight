using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActorChildCollider : MonoBehaviour
{
    protected CActor m_MyActor = null;


    private void Awake()
    {
        m_MyActor = this.GetComponentInParent<CActor>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (m_MyActor == null)
            return;

        m_MyActor.OnTriggerEnter(other);
    }

    public void OnTriggerStay(Collider other)
    {
        if (m_MyActor == null)
            return;

        m_MyActor.OnTriggerStay(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (m_MyActor == null)
            return;

        m_MyActor.OnTriggerExit(other);
    }


    public void OnCollisionEnter(Collision other)
    {
        if (m_MyActor == null)
            return;

        m_MyActor.OnCollisionEnter(other);
    }
}
