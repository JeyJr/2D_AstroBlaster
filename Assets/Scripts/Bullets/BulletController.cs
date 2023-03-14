using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsavel por: 
/// 1. Controlar a movimentação da bullet
/// 2. Retornar para pool quando colidir com algo ou tempo de duração "visivel" acabar
/// </summary>
public class BulletController : MonoBehaviour, ISetPoolReference
{
    [Tooltip("Voltar para pool de origem")]
    private IBackToPool backToPool;

    [Tooltip("Controle de movimentação da bullet")]
    private float moveSpeed = 0;
    private bool enableToMove = false;

    public void SetPoolReference(IBackToPool poolReference)
    {
        backToPool = poolReference;
    }
    private void OnEnable()
    {
        enableToMove = true;
        moveSpeed = GameData.GetInstance().GetBulletMoveSpeed();
    }

    private void Update()
    {
        if (enableToMove)
        {
            transform.Translate(moveSpeed * Time.deltaTime * transform.up); 
        }
    }

    /// <summary>
    /// Vai voltar pra pool quando colidir com qualquer obj
    /// Vai dar dano caso o objeto tenha a inteface ILifeControl
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<IAsteroidLifeControl>(out var lifeControl))
            {
                float damage = GameData.GetInstance().GetBulletDamage();
                lifeControl.LooseLife(damage);
            }

            BackToPool();
        }
        else
        {
            BackToPool();
        }
    }

    private void BackToPool()
    {
        enableToMove = false;
        gameObject.SetActive(false);
        backToPool.BackToPool(gameObject);
    }

}
