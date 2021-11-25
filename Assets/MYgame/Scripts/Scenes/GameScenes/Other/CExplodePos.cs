using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CExplodePos : MonoBehaviour
{
    [SerializeField] protected LayerMask m_AllExplodeMask;
    public LayerMask AllExplodeMask { get { return m_AllExplodeMask; } }

    [SerializeField] protected float m_ExplosionForce = 1.0f;
    public float ExplosionForce
    {
        set { m_ExplosionForce = value; }
        get { return m_ExplosionForce; }
    }

    [SerializeField] protected float m_ExplosionRadius = 1.0f;
    public float ExplosionRadius
    {
        set { m_ExplosionRadius = value; }
        get { return m_ExplosionRadius; }
    }

    [SerializeField] protected float m_DelayDeletTime = 1.0f;
    public float DelayDeletTime
    {
        set { m_DelayDeletTime = value; }
        get { return m_DelayDeletTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDelet(m_DelayDeletTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        Collider[] lTempAllCollider = Physics.OverlapSphere(this.transform.position, 5.0f, m_AllExplodeMask);

        Rigidbody lTempRigidbody = null;

        foreach (Collider lTColliderObj in lTempAllCollider)
        {
            lTempRigidbody = lTColliderObj.GetComponentInParent<Rigidbody>();
            if (lTempRigidbody != null)
                lTempRigidbody.AddExplosionForce(m_ExplosionForce, this.transform.position, m_ExplosionRadius);
        }
    }

    public IEnumerator DelayDelet(float delayTime = 0.0f)
    {
        //  Debug.Log("[1] WaitForEndOfFrame Frame Count: " + Time.frameCount + "Render Frame Count: " + Time.renderedFrameCount);
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
    }
}
