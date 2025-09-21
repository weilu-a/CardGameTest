using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f; // 特效持续时间

    void Start()
    {
        // 特效播放完成后自动销毁
        Destroy(gameObject, destroyDelay);
    }
}
