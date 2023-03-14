using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe responsavel por controlar o sistema de vida e morte, instanciar pedaços e voltar para pool
/// </summary>
public class AsteroidStatus : MonoBehaviour, IAsteroidLifeControl, ISetPoolReference, IUpdateCanvasLife
{
    [SerializeField] private float maxLife;
    [SerializeField] private float life;

    [SerializeField] private TextMeshProUGUI txtLife;


    private IBackToPool myPool;
    public void SetPoolReference(IBackToPool poolReference)
    {
        myPool = poolReference;
    }


    [Space(5)]
    [Tooltip("Evento para controlar ações quando morrer")]
    [SerializeField] private UnityEvent OnTakeDamage;
    
    [Space(5)]
    [Tooltip("Evento para controlar ações quando receber dano!")]
    [SerializeField] private UnityEvent OnHitGround;

    [Space(5)]
    [Tooltip("Evento para controlar ações quando receber dano!")]
    [SerializeField] private UnityEvent OnDestroyed;
    [SerializeField] private GameObject explosionPrefab;


    private void Awake()
    {
        UpdateLife();
    }

    private void OnEnable()
    {
        UpdateLife();
    }


    /// <summary>
    /// Define o valor de vida inicial do Asteroid
    /// </summary>
    /// <param name="value">Valor de vida total</param>
    public void SetLifeValue(float value)
    {
        maxLife = value;
        life = value; 
    }
   
    /// <summary>
    /// Adiciona valor a vida atual do objeto
    /// </summary>
    /// <param name="value">Valor de vida a ser incrementado</param>
    public void GainLife(float value)
    {
        life += value;
    }

    /// <summary>
    /// Subtrair vida do objeto e invoca o evento OnTakeDamage. <br>
    /// Verifica se a vida zerou e invoka o evento Destroyed</br>
    /// </summary>
    /// <param name="damage">Subtrair da vida atual</param>
    public void LooseLife(float damage)
    {
        life -= damage;
        OnTakeDamage.Invoke();

        if (life < 1)
        {
            OnDestroyed.Invoke();
        }
    }

    public float GetLifeValue()
    {
        return maxLife;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Player perde vida, atualiza o sliderLife e asteroid é destruido
            GameData.GetInstance().DecreasePlayerLife();
            collision.gameObject.GetComponent<IUpdateCanvasLife>().UpdateLife();

            LooseLife(life);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            OnHitGround.Invoke();
        }
    }


    //OnTakeDamage
    public void OnTakeDamagePlaySFX() 
    { 

    }
    public void OnTakeDamageSpawnHitVFX()
    {

    }

    public void UpdateLife()
    {
        txtLife.text = life.ToString("F0");
    }

    /// <summary>
    /// Instancia o prefab: explosionPrefab, e o destroi apos 1segundo
    /// </summary>
    public void OnDestroyedSpawnExplosion()
    {
        if(explosionPrefab != null)
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            Instantiate(explosionPrefab, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));

            GameEvents.GetInstance().AddPoint();
        }
    }
   
    /// <summary>
    /// Retorna o objeto para sua pool de origem
    /// </summary>
    public void BackToPool()
    {
        if(myPool != null)
        {
            myPool.BackToPool(gameObject);
        }
    }


    public void CameraStandardShake()
    {
        Camera.main.GetComponent<CameraShake>().Shake();
    }

}
