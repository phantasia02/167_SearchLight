using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ELoseResultType
{
    eHideQuestionHole   = 0,
    eHideFragments      = 1,
    eMax
};

[CreateAssetMenu]
public class CStageType : ScriptableObject
{
    public ELoseResultType m_LoseResultType = ELoseResultType.eHideQuestionHole;
}
