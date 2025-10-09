using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;

    void Start()
    {
        
        Destroy(gameObject, destroyDelay);
    }
}
