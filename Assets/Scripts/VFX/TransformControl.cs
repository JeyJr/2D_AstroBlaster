using System.Collections;
using UnityEngine;

public class TransformControl : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private bool rotateClockWise = true;

    private void OnEnable()
    {
        StartCoroutine(RotateGameObject());
        rotateClockWise = Random.value > .5f;
    }

    IEnumerator RotateGameObject()
    {
        while (true)
        {
            if (rotateClockWise)
            {
                transform.Rotate(0, 0, -rotationSpeed, Space.World);
            }
            else
            {
                transform.Rotate(0, 0, rotationSpeed, Space.World);
            }

            yield return null;
        }
    }
}
