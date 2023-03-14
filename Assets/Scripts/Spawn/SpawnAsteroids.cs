using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids: MonoBehaviour, IBackToPool
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float spawnRange;

    [SerializeField] private float delayToSpawn = 3f;

    [SerializeField] private List<GameObject> prefabs;

    private Queue<GameObject> pool = new();
    private List<GameObject> spawnedObjs = new();

    private bool enableToSpawn; 

    private void Start()
    {
        CreatePool();
        GameEvents.GetInstance().OnStartMatch += StartSpawn;
        GameEvents.GetInstance().OnEndGame += StopSpawn;
    }

    private void CreatePool()
    {
        int index = 0;
        for (int i = 0; i < prefabs.Count; i++)
        {

            if (index > prefabs.Count - 1) index = 0;

            var obj = Instantiate(prefabs[index], transform);
            obj.SetActive(false);

            if (obj.TryGetComponent<ISetPoolReference>(out var poolReference))
            {
                poolReference.SetPoolReference(this);
            }

            pool.Enqueue(obj);
            index++;
        }
    }

    private void StartSpawn()
    {
        enableToSpawn = true;
        StartCoroutine(SpawnObjs());
    }

    private void StopSpawn()
    {
        enableToSpawn = false;
        StartCoroutine(ReturnToInitialPool());
    }

    IEnumerator SpawnObjs()
    {
        while (enableToSpawn)
        {
            yield return null;

            if ( pool.Count > 0)
            {
                var obj = pool.Dequeue();
                spawnedObjs.Add(obj);

                if (obj.TryGetComponent<IAsteroidLifeControl>(out var lifeControl))
                {
                    var life = GameData.GetInstance().GetAsteroidLife();
                    lifeControl.SetLifeValue(life);
                }

                obj.transform.position = SetObjInitialPosition();

                obj.SetActive(true);
                yield return new WaitForSeconds(delayToSpawn);
            }
        }
    }

    private Vector3 SetObjInitialPosition()
    {
        float x = Random.Range(spawnPosition.position.x - spawnRange, spawnPosition.position.x + spawnRange);
        return new Vector3(x, spawnPosition.position.y, spawnPosition.position.z);
    }

    public void BackToPool(GameObject obj)
    {
        obj.SetActive(false);

        if (spawnedObjs.Contains(obj))
        {
            spawnedObjs.Remove(obj);
        }
        pool.Enqueue(obj);
    }

    IEnumerator ReturnToInitialPool()
    {
        yield return null;
        for (int i = 0; i < spawnedObjs.Count; i++)
        {
            BackToPool(spawnedObjs[i]);
        }
    }
}