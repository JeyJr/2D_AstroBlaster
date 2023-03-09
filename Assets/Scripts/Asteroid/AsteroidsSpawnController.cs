using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawnController: MonoBehaviour, IBackToPool
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float spawnRange;

    [SerializeField] private int maxPoolValue = 5;
    [SerializeField] private float delayToSpawn = 3f;

    [SerializeField] private List<GameObject> prefabs;

    private Queue<GameObject> pool = new();
    private List<GameObject> spawnedObjs = new();

    public bool enableToSpawn; //ENABLE SERA MODIFICADO POR ALGUM EVENTO DO GAMESTATE
    public float lifeMultiplier = 10;

    private void Start()
    {
        CreatePool();
        StartCoroutine(SpawnObjs());
    }

    private void CreatePool()
    {
        int index = 0;
        for (int i = 0; i < maxPoolValue; i++)
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

    IEnumerator SpawnObjs()
    {
        while (true)
        {
            yield return null;

            if (enableToSpawn && pool.Count > 0)
            {
                var obj = pool.Dequeue();
                spawnedObjs.Add(obj);

                if (obj.TryGetComponent<ILifeControl>(out var lifeControl))
                {
                    lifeControl.SetLifeValue(lifeMultiplier);
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
}