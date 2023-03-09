using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controla a movimentação do objeto quando habilitado na cena
/// </summary>
public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool enableToMove;

    private void OnEnable()
    {
        enableToMove = true;
        
    }

    private void OnDisable()
    {
        enableToMove = false;
    }

    private void Update()
    {
        if (enableToMove)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }



}
