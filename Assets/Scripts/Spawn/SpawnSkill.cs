using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : MonoBehaviour, IBackToPool
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<GameObject> pool = new();
    [SerializeField] private float delayTime = 1;

    private bool enabledToSpawn;

    private void Start()
    {
        GameEvents.GetInstance().OnStartMatch += StartSpawn;
        GameEvents.GetInstance().OnEndGame += StopSpawn;
    }

    /// <summary>
    /// Spawn inicia com um evento OnStartMatch
    /// </summary>
    private void StartSpawn()
    {
        enabledToSpawn = true;
        InitializePool();
    }

    private void StopSpawn()
    {
        enabledToSpawn = false;
    }


    private void InitializePool()
    {
        if (prefabs.Count > 0)
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                var obj = Instantiate(prefabs[i], transform);

                obj.GetComponent<ISetPoolReference>().SetPoolReference(this);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        StartCoroutine(SpawnSkills());
    }


    IEnumerator SpawnSkills()
    {
        while (enabledToSpawn)
        {
            yield return new WaitForSeconds(delayTime);

            if (prefabs.Count > 0)
            {
                int index = Random.Range(0, pool.Count);

                if (!pool[index].activeSelf)
                {
                    pool[index].SetActive(true);
                    pool[index].transform.position = transform.position;
                }
            }
        }

        for (int i = 0; i < pool.Count; i++)
        {
            Destroy(pool[i]);
        }
        pool.Clear();
    }

    public void BackToPool(GameObject obj)
    {
        GameObject item = pool.Find(gameObject => gameObject == obj);

        if (item != null)
        {
            item.SetActive(false);
        }
    }


}
