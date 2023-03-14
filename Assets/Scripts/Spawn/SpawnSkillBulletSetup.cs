using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A cada X tempo sera instanciado uma skill de BulletSetup em ordem 2,3,4,5,6 e 7
/// Enquanto o player nao coletar o objeto spawnado, o mesmo ira ficar aparecendo de tempo em tempo até ser coletado
/// Dessa forma o update do setup de spawn será gradualmente aprimorado durante a gameplay
/// </summary>
public class SpawnSkillBulletSetup : MonoBehaviour, IBackToPool
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<GameObject> pool = new();
    [SerializeField] private float delayTime = 30;
    [SerializeField] private bool enableToSpawn;

    private void Start()
    {
        GameEvents.GetInstance().OnStartMatch += StartSpawn;
        GameEvents.GetInstance().OnEndGame += EndSpawn;
    }

    /// <summary>
    /// Spawn inicia com um evento OnStartMatch
    /// </summary>
    private void StartSpawn()
    {
        enableToSpawn = true;
        StartCoroutine(InstantiatePoolObjects());
    }

    private void EndSpawn()
    {
        enableToSpawn = false;
        for (int i = 0; i < pool.Count; i++)
        {
            Destroy(pool[i]);
        }

        pool.Clear();
    }

    IEnumerator InstantiatePoolObjects()
    {
        yield return null;

        pool.Clear();
        foreach (var item in prefabs)
        {
            GameObject obj = Instantiate(item, transform);
            pool.Add(obj);
            obj.SetActive(false);
            obj.GetComponent<ISetPoolReference>().SetPoolReference(this);
        }

        StartCoroutine(SpawnSkills());
    }

    IEnumerator SpawnSkills()
    {
        if (enableToSpawn)
        {
            yield return new WaitForSeconds(delayTime);

            if (pool.Count == 0)
            {
                yield break;
            }

            if (enableToSpawn)
            {
                GameObject obj = pool.First();
                obj.SetActive(true);
                obj.transform.position = transform.position;
            }
        }
    }

    /// <summary>
    /// Retorna a skill para pool e iniciar o spawn de skills  
    /// </summary>
    /// <param name="obj"></param>
    public void BackToPool(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            obj.SetActive(false);
            StartCoroutine(SpawnSkills());
        }
    }

    /// <summary>
    /// Remove as skills da pool que foram coletadas pelo player, caso a ultima tenha sido coletada, é encerrado esse ciclo de spawns
    /// </summary>
    public void RemoveFromPool()
    {
        GameObject obj = pool.First();
        pool.Remove(obj);
        Destroy(obj);

        if(pool.Count > 0)
        {
            StartCoroutine(SpawnSkills());
        }
    }
}
