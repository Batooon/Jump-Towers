using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Platform : MonoBehaviour
{
    [field: SerializeField]
    public Transform JumpPoint
    {
        get;
        private set;
    }
    
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
