using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Classe responsavel por controlar a movimentação e todas as caracteristicas e controles da mesma
/// </summary>
public class SpaceShipController : MonoBehaviour, IUpdateCanvasLife
{
    public float MoveSpeed { get; set; }

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float screenLimitX = 10;
    [SerializeField] private float screenLimity = 10;

    [Header("Suavização dos movimentos")]
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity;

    [Header("LifeControl")]
    [SerializeField] private Slider lifeBar;

    [SerializeField] private Sprite standardBulletSprite;

    private void Start()
    {
        GameEvents.GetInstance().OnStartMatch += SetSetupOnStartMatch;
    }

    private void SetSetupOnStartMatch()
    {
        playerInput = GetComponent<PlayerInput>();
        MoveSpeed = GameData.GetInstance().GetPlayerMoveSpeed();

        lifeBar.maxValue = GameData.GetInstance().GetPlayerMaxLife();
        lifeBar.value = lifeBar.maxValue;

        transform.position = Vector2.zero;

        GetComponentInChildren<ISetBulletType>().SetNewBulletSprite(standardBulletSprite);
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
        Vector3 targetPosition = transform.position + new Vector3(input.x * MoveSpeed, input.y * MoveSpeed, 0f);

        targetPosition.x = Mathf.Clamp(targetPosition.x, -screenLimitX, screenLimitX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -screenLimity, screenLimity);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

    public void UpdateLife()
    {
        lifeBar.maxValue = GameData.GetInstance().GetPlayerMaxLife();
        lifeBar.value = GameData.GetInstance().GetPlayerLife();
    }

}
