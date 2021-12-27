using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAIStateEnemyBase : CEnemyStateBase
{
    public override EMovableState StateType() { return EMovableState.eMove; }
    protected Vector3 m_EndPoint = Vector3.zero;

    public CAIStateEnemyBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyEnemyBaseMemoryShare.m_MyEnemyBase.ATKShowObj(CEnemyBase.EATKShowObj.eNotATKShow);
        SetAnimationState(CAnimatorStateCtl.EState.eRun);
        m_MyEnemyBaseMemoryShare.m_AiMove.canMove = true;
        Initpos();
    }

    protected override void updataState()
    {
        if (m_MyEnemyBaseMemoryShare.m_AiMove.reachedEndOfPath || m_MyEnemyBaseMemoryShare.m_AiMove.reachedDestination)
        {
            m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Clear();
            m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eWait);

            if (!m_MyEnemyBaseMemoryShare.m_IsShow && Random.Range(0, 10) < 3)
                m_MyEnemyBaseMemoryShare.m_MyRandomStateList.Add(EMovableState.eAtk);


            m_MyEnemyBaseMemoryShare.m_MyActor.SetChangState(RandomState(m_MyEnemyBaseMemoryShare.m_MyRandomStateList));
           
        }
        else
            m_MyEnemyBaseMemoryShare.m_AiMove.destination = m_EndPoint;
    }

    protected override void OutState()
    {
        m_MyEnemyBaseMemoryShare.m_AiMove.canMove = false;
    }

    public void Initpos()
    {

        Vector3 lTempEndPos = Random.insideUnitSphere;
        lTempEndPos.y = 0.0f;
        lTempEndPos.Normalize();
        lTempEndPos = lTempEndPos * Random.Range(3.0f, 10.0f) + m_MyEnemyBaseMemoryShare.m_MyActor.transform.position;

        if (m_MyEnemyBaseMemoryShare.m_AiMove != null)
        {
            m_EndPoint = lTempEndPos;
            m_MyEnemyBaseMemoryShare.m_AiMove.destination = m_EndPoint;
        }
    }

}
