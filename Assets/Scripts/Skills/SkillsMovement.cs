using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2;
    private bool enableToMove;

    private void OnEnable()
    {
        enableToMove = true;
    }

    private void OnDisable()
    {
        enableToMove=false;
    }

    void Update()
    {
        if (enableToMove)
        {
            transform.Translate(moveSpeed * Time.deltaTime * -transform.up);
        }
    }
}
