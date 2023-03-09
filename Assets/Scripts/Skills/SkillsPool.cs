using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPool : MonoBehaviour, IBackToPool
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<GameObject> pool = new();
    [SerializeField] private float delayTime = 1;

    public bool enabledToSpawn;

    private void Start()
    {
        if(prefabs.Count > 0)
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
        while (true)
        {
            yield return null;

            if(enabledToSpawn && prefabs.Count > 0)
            {
                yield return new WaitForSeconds(delayTime);
                int index = Random.Range(0, pool.Count);

                if (!pool[index].activeSelf)
                {
                    pool[index].SetActive(true);
                    pool[index].transform.position = transform.position;
                }
            }
        }
    }

    public void BackToPool(GameObject obj)
    {
        GameObject bullet = pool.Find(gameObject => gameObject == obj);

        if (bullet != null)
        {
            bullet.SetActive(false);
        }
    }

}
