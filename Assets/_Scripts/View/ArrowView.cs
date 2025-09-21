using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;
    [SerializeField] private LineRenderer lineRenderer;

    private Vector3 startPosition;

    private void Update()
    {
        Vector3 endPosition = MoustUtil.getMousePositionInWorld();
        Vector3 direction = -(startPosition - arrowHead.transform.position).normalized;
        lineRenderer.SetPosition(1, endPosition - direction * 0.1f);
        arrowHead.transform.position = endPosition;
        arrowHead.transform.right = Quaternion.Euler(0, 0, -90) * direction;
    }

    public void SetupArrow(Vector3 startPosition)
    {
        this.startPosition = startPosition;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, MoustUtil.getMousePositionInWorld());
    }

}
