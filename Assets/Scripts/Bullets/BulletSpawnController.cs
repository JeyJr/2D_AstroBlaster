using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BulletSpawnController : MonoBehaviour, IBackToPool
{
    [SerializeField] private Transform posLeft, posRight;

    private int maxPoolValue = 30;
    private float delayToSpawn = .1f;
    private int prefabsID = 0;

    [SerializeField] private List<GameObject> prefabs;

    private Queue<GameObject> pool = new();
    private List<GameObject> spawnedObjs= new();

    public bool enableToSpawn;

    private void Start()
    {
        CreatePool();
        StartCoroutine(SpawnObjs());
    }

    private void CreatePool()
    {
        for (int i = 0; i < maxPoolValue; i++)
        {
            var obj = Instantiate(prefabs[prefabsID]);
            obj.SetActive(false);
            
            if(obj.TryGetComponent<ISetPoolReference>(out var poolReference))
            {
                poolReference.SetPoolReference(this);
            }

            pool.Enqueue(obj);
        }
    }


    IEnumerator SpawnObjs()
    {
        while (true)
        {
            yield return null;

            if (enableToSpawn && pool.Count > 0)
            {
                var objLeft = pool.Dequeue();
                var objRight = pool.Dequeue();

                objLeft.SetActive(true);
                objLeft.transform.position = posLeft.position;

                objRight.SetActive(true);
                objRight.transform.position = posRight.position;

                spawnedObjs.Add(objLeft);
                spawnedObjs.Add(objRight);

                yield return new WaitForSeconds(delayToSpawn);
            }
        }
    }

    public void BackToPool(GameObject obj)
    {
        if (spawnedObjs.Contains(obj))
        {
            spawnedObjs.Remove(obj);
        }
        pool.Enqueue(obj);
    }
}
