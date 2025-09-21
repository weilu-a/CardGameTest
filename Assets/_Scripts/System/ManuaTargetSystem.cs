using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuaTargetSystem : Singleton<ManuaTargetSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask targetLayMask;

    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosition);
    }

    public EnemyView EndTargeting(Vector3 endPosition)
    {
        Debug.Log(endPosition);
        Debug.Log("正在找" + targetLayMask);
        Debug.DrawRay(endPosition, Vector3.forward * 10f, Color.red, 10f);
        arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f, targetLayMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        { 
            Debug.Log("碰到了！");
            return enemyView;
        }
        Debug.Log("没碰到");
        return null;
    }
}
