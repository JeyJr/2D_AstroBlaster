using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private float shakeDuration = 0.1f;

    /// <summary>
    /// Treme a tela durante um intervalo de tempo
    /// </summary>
    public void Shake()
    {
        shakeMagnitude = 0.5f;
        shakeDuration = 0.1f;

        StartCoroutine(DoShake());
    }

    /// <summary>
    /// Treme a tela durante um intervalo de tempo
    /// </summary>
    /// <param name="magnitude">Intensidade do tremor</param>
    /// <param name="duration">Duração do tremor</param>

    private IEnumerator DoShake()
    {
        float elapsed = 0.0f;
        Vector3 originalCamPos = transform.localPosition;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalCamPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalCamPos;
    }
}
