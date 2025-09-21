using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    public List<EnemyView> EnemyViews { get; private set; } = new();

    public void AddEnemy(EnemyData enemyData)
    {
        Transform solt = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreater.Instance.CreatEnemyView(enemyData, solt.position, solt.rotation);
        enemyView.transform.parent = solt;
        EnemyViews.Add(enemyView);
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        EnemyViews.Remove(enemyView);
        //À¿Õˆ∂Øª≠
        yield return null;
        Destroy(enemyView.gameObject);
    }
}
