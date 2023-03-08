using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidPiecesPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Transform spawnPosition;
    private float force = 1f;

    [SerializeField] private UnityEvent AfterSpawnPieces;

    private float life;

    public void SpawnPieces()
    {
        if (TryGetComponent<ILifeControl>(out var lifeControl))
        {
            Debug.Log("Passando vida pros fi!");
            float value = lifeControl.GetLifeValue() / prefabs.Count;
            life = value >= 1 ? value : 1;
        }

        if (prefabs.Count > 0)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        yield return null;

        for (int i = 0; i < prefabs.Count; i++)
        {
            var obj = Instantiate(prefabs[i], spawnPosition.transform.position, Quaternion.identity);
            obj.transform.position = NewPosition();
            obj.SetActive(true);

            if (obj.TryGetComponent<ILifeControl>(out var lifeControl))
            {
                lifeControl.SetLifeValue(life);
            }

            if (obj.TryGetComponent<IUpdateTextLife>(out var updateTextLife))
            {
                updateTextLife.UpdateTextLife();
            }
        }

        AfterSpawnPieces.Invoke();
    }


    private Vector3 NewPosition()
    {
        float x = Random.Range(spawnPosition.transform.position.x, spawnPosition.transform.position.x) * force;
        float y = Random.Range(spawnPosition.transform.position.x, spawnPosition.transform.position.x) * force;

        return new Vector3(x, y, 2);
    }

}
