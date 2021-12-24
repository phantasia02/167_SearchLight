using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;


public class CtestNpcMemoryShare : CActorMemoryShare
{
    public const float CDefCorrectionDir = 2.0f;

    public CtestNpc m_MytestNpc = null;
    public Path path = null;
    public int m_CurNodeIndex = 0;
    public Seeker m_seeker = null;
    public IAstarAI m_AiMove = null;
    public float m_CorrectionDir = CDefCorrectionDir;
    public Vector3 m_EndPos = Vector3.zero;

};

public class CtestNpc : CActor
{
    protected CtestNpcMemoryShare m_MyCtestNpcMemoryShare = null;

    public override EActorType MyActorType() { return EActorType.eEnemy; }

    protected override void AddInitState()
    {
       
        //m_AllState[(int)CMovableStatePototype.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemyBase(this));
    }


    protected override void CreateMemoryShare()
    {

        m_MyCtestNpcMemoryShare = new CtestNpcMemoryShare();
        m_MyMemoryShare = m_MyCtestNpcMemoryShare;

        m_MyCtestNpcMemoryShare.m_MytestNpc = this;
        m_MyCtestNpcMemoryShare.m_seeker = this.GetComponent<Seeker>();
        m_MyCtestNpcMemoryShare.m_AiMove = this.GetComponent<IAstarAI>();
        //m_MyEnemyBaseMemoryShare.m_AllEmoticons = m_AllEmoticons;
        //m_MyEnemyBaseMemoryShare.m_AllChangRendererMat = m_AllChangRendererMat;
        //m_MyEnemyBaseMemoryShare.m_StateShowObj = m_StateShowObj;

        base.CreateMemoryShare();

        //ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
    }

    protected override void Start()
    {
        Initpos();
    }

    public void OnPathComplete(Path p)
    {

        if (!p.error)
        {
            m_MyCtestNpcMemoryShare.path = p;
            m_MyCtestNpcMemoryShare.m_CurNodeIndex = 0;
        }
        else
            Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

    }

    protected override void Update()
    {
     

        if (m_MyCtestNpcMemoryShare.m_EndPos != null && m_MyCtestNpcMemoryShare.m_AiMove != null)
        {
            Vector3 lTempV3ok = m_MyCtestNpcMemoryShare.m_EndPos - this.transform.position;
            lTempV3ok.y = 0.0f;

            
            if (m_MyCtestNpcMemoryShare.m_AiMove.reachedEndOfPath)
                Initpos();
            else
                m_MyCtestNpcMemoryShare.m_AiMove.destination = m_MyCtestNpcMemoryShare.m_EndPos;

        }
    }

    public void Initpos()
    {
        m_MyCtestNpcMemoryShare.m_EndPos = Random.insideUnitSphere;
        m_MyCtestNpcMemoryShare.m_EndPos.y = 0.0f;
        m_MyCtestNpcMemoryShare.m_EndPos.Normalize();
        m_MyCtestNpcMemoryShare.m_EndPos = m_MyCtestNpcMemoryShare.m_EndPos * Random.Range(2.0f, 10.0f) + this.transform.position;

        if (m_MyCtestNpcMemoryShare.m_EndPos != null && m_MyCtestNpcMemoryShare.m_AiMove != null)
            m_MyCtestNpcMemoryShare.m_AiMove.destination = m_MyCtestNpcMemoryShare.m_EndPos;

    }
}
