using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformControl : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;

    void Start()
    {
        StartCoroutine(RotateGameObject());
        
    }

    IEnumerator RotateGameObject()
    {
        while (true)
        {
            transform.Rotate(0, 0, rotationSpeed, Space.World);
            yield return null;
        }
    }
}
