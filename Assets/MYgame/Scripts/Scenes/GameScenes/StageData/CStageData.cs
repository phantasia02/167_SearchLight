using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(
    menuName = "Data/Stage Data",
    fileName = "StageData")]
public class CStageData : ScriptableObject
{
    [System.Serializable]
    public class DataCreateEnemy
    {
        public CEnemyBase.EEnemyType m_EnemyType = CEnemyBase.EEnemyType.eEnemyRifle;
        public int Count = 0;
        public CBulletFlyObj.EBulletArms m_ARMS = CBulletFlyObj.EBulletArms.eNormalBullet;
    }

    [SerializeField] protected List<DataCreateEnemy> _DataAllCreateEnemyCount = new List<DataCreateEnemy>();


    public List<DataCreateEnemy> DataAllCreateEnemyCount => _DataAllCreateEnemyCount;
}
