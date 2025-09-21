using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewCreater : Singleton<EnemyViewCreater>
{
    [SerializeField] private EnemyView enemyViewPrefab;

    public EnemyView CreatEnemyView(EnemyData enemyData,Vector3 position,Quaternion quaternion)
    {
        EnemyView enemyView = Instantiate(enemyViewPrefab, position, quaternion);
        enemyView.Setup(enemyData);
        return enemyView;
    }
}
