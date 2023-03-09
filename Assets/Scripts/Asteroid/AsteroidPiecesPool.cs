
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidPiecesPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    [SerializeField] private float x = 2.5f;
    [SerializeField] private float y = 1f;

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
            var obj = Instantiate(prefabs[i], transform.position, Quaternion.identity);
            obj.transform.position = Position();

            obj.GetComponent<ILifeControl>().SetLifeValue(life);
            obj.GetComponent<IUpdateTextLife>().UpdateTextLife();
            obj.SetActive(true);
        }

        AfterSpawnPieces.Invoke();
    }


    private Vector3 Position()
    {
        float x = Random.Range(transform.position.x - this.x, transform.position.x + this.x);
        float y = Random.Range(transform.position.y - this.y, transform.position.y + this.y);

        return new Vector3(x, y, 2);
    }

}
