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
    private float moveSpeed = 20;
    private float delayToDisable = 1f;
    private bool enableToMove = false;

    public void SetPoolReference(IBackToPool poolReference)
    {
        backToPool = poolReference;
    }

    private void OnEnable()
    {
        enableToMove = true;
        StartCoroutine(DisableBullet());
    }
    IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(delayToDisable);
        BackToPool();
    }

    private void Update()
    {
        if (enableToMove)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            //Tentar dar dano reconhecendo a interface
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
