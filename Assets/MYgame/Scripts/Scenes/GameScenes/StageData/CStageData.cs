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
    }

    [SerializeField] protected List<DataCreateEnemy> _DataAllCreateEnemyCount = new List<DataCreateEnemy>();


    //[SerializeField]private StaticGlobalDel.EBrickColor[] _brickColors;
    //[SerializeField]private BuildingRecipeData[] _buildings;
    //[SerializeField]private TweenHDRColorEaseCurve _creatarchitecture;
    // [SerializeField] protected GameObject m_GameSceneData;

    public List<DataCreateEnemy> DataAllCreateEnemyCount => _DataAllCreateEnemyCount;
}
