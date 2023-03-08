using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


/// <summary>
/// Classe responsavel por controlar a movimentação e todas as caracteristicas e controles da mesma
/// </summary>
public class SpaceShipMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float screenLimitX = 10;
    [SerializeField] private float screenLimity = 10;

    [Header("Suavização dos movimentos")]
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Controle da movimentação, limite de tela e suavização dos movimentos da nave
    /// </summary>
    private void Movement()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 targetPosition = transform.position + new Vector3(input.x * moveSpeed, input.y * moveSpeed, 0f);

        targetPosition.x = Mathf.Clamp(targetPosition.x, -screenLimitX, screenLimitX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -screenLimity, screenLimity);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

}
