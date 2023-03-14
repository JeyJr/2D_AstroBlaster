using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Padroniza o tipos de setup para spawnar as bullets
/// </summary>
public enum BulletSetup { 
    setupOne, 
    setupTwo, 
    setupThree,
    setupFour,
    setupFive,
    setupSix,
    setupSeven,
    empty,
}

/// <summary>
/// Padroniza os tipos de posições de spawn 
/// </summary>
public enum PositionNames
{
    pCentral,
    pLeftOne,
    pRightOne,
    pLeftTwo,
    pRightTwo,
}

public class SpawnBullets : MonoBehaviour, IBackToPool, ISetBulletSetup, ISetBulletType
{
    [Space(5)]
    [Header("Controle das posições e spawns")]
    [SerializeField] private List<Transform> positionList; //Posições disponiveis na nave
    private Dictionary<PositionNames, Transform> positions; //Define a biblioteca a ser utilizada
    private List<Transform> spawnPosition = new(); //Define as posições ativas durante o jogo
    private float zRotationValue = 3; //Define as rotações para posições de spawn especificas

    [Space(5)]
    [Header("Controle do Spawn")]
    [SerializeField] private float delayToSpawn = .5f;
    [SerializeField] private int maxPoolValue = 30;

    [Space(5)]
    [Header("Bullet Pool")]
    [SerializeField] private GameObject prefabs; //Tipos de bulets disponiveis para uso
    private List<GameObject> bulletsPool= new(); //Pool

    [Header("Ref: BulletSetupController")]
    [SerializeField] private SpawnSkillBulletSetup bulletSetupController;


    [SerializeField] private SFXControl sfx;

    private bool enableToSpawn; //ENABLE SERA MODIFICADO POR ALGUM EVENTO DO GAMESTATE
    private void Start()
    {
        positions = new Dictionary<PositionNames, Transform>
        {
            { PositionNames.pCentral, positionList[0] },
            { PositionNames.pLeftOne, positionList[1] },
            { PositionNames.pRightOne, positionList[2] },
            { PositionNames.pLeftTwo, positionList[3] },
            { PositionNames.pRightTwo, positionList[4] }
        };

        CreatePool();
        
        GameEvents.GetInstance().OnStartMatch += StartSpawn;
        GameEvents.GetInstance().OnEndGame += StopSpawn;
    }

    /// <summary>
    /// Spawn inicia com um evento OnStartMatch
    /// </summary>
    private void StartSpawn()
    {
        enableToSpawn = true;
        SetBulletSetup(BulletSetup.setupOne);
        StartCoroutine(SpawnObjs());
    }

    private void StopSpawn()
    {
        enableToSpawn = false;
    }

    private void CreatePool()
    {
        for (int i = 0; i < maxPoolValue; i++)
        {
            GameObject obj = Instantiate(prefabs);
            obj.SetActive(false);
            
            if(obj.TryGetComponent<ISetPoolReference>(out var poolReference))
            {
                poolReference.SetPoolReference(this);
            }

            bulletsPool.Add(obj);
        }
    }

    /// <summary>
    /// Spawn das bullets é organizado da seguinte forma:  <br></br>
    /// 1. A lista de posições para spawn precisa ter algum elemento <br></br>
    /// 2. A pool de bullets precisa ter bullets instanciadas  <br></br>
    /// 3. As bullets so podem ser habilitadas se houver alguma(s) bullets na pool desabilitadas e com numero suficiente para spawn  
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnObjs()
    {
        while (enableToSpawn)
        {
            yield return null;

            if (bulletsPool.Count > positionList.Count)
            {
                delayToSpawn = GameData.GetInstance().GetDelayToSpawnBullet();

                yield return new WaitForSeconds(delayToSpawn);

                for (int i = 0; i < spawnPosition.Count; i++)
                {
                    GameObject obj = bulletsPool.Find(obj => !obj.activeSelf);

                    if(obj!= null)
                    {
                        obj.SetActive(true);
                        obj.transform.SetPositionAndRotation(spawnPosition[i].position, spawnPosition[i].localRotation);
                    }
                }

                sfx.PlayClip();
            }
        }
    }

    public void BackToPool(GameObject obj)
    {
        GameObject bullet = bulletsPool.Find(gameObject => gameObject == obj);

        if(bullet != null)
        {
            bullet.SetActive(false);
        }
    }



    /// <summary>
    /// Defina o tipo de setup para spawnar as bullets
    /// </summary>
    /// <param name="setup">Defina o valor do setup</param>
    public void SetBulletSetup(BulletSetup setup)
    {
        spawnPosition.Clear();

        switch (setup)
        {
            case BulletSetup.empty:
                break;
            case BulletSetup.setupOne:
                SetupOne();
                break;
            case BulletSetup.setupTwo:
                SetupTwo();
                break;
            case BulletSetup.setupThree:
                SetupThree();
                break;
            case BulletSetup.setupFour:
                SetupFour();
                break;
            case BulletSetup.setupFive:
                SetupFive();
                break;
            case BulletSetup.setupSix:
                SetupSix();
                break;
            case BulletSetup.setupSeven:
                SetupSeven();
                break;
            default:
                throw new System.ArgumentException("Setup invalido", nameof(setup));
        }
    }

    //BuletType----------
    //Modificar as sprites da pool para o novo type coletado pelo player
    //Os bonus serão implementados de forma natural quando o elementos sao utilizados na cena
    public void SetNewBulletSprite(Sprite sprite)
    {
        StartCoroutine(NewBulletType(sprite));
    }

    IEnumerator NewBulletType(Sprite sprite)
    {
        yield return null;
        for (int i = 0; i < bulletsPool.Count; i++)
        {
            bulletsPool[i].GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }
    }

    //BulletSetup----------

    /// <summary>
    /// Spawna bullets de uma unica posição: pCentral
    /// </summary>
    public void SetupOne()
    {
        zRotationValue = 0f;

        var pCentral = positions.GetValueOrDefault(PositionNames.pCentral);

        pCentral.rotation = Quaternion.Euler(0f, 0f, zRotationValue);
        spawnPosition.Add(pCentral);
    }

    /// <summary>
    /// Spawna bullets de duas posições laterais e sem rotação nas posições: pLeftOne e pRightOne
    /// </summary>
    public void SetupTwo()
    {
        zRotationValue = 0f;

        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);

        pLeftOne.rotation = Quaternion.Euler(0f, 0f, zRotationValue);
        pRightOne.rotation = Quaternion.Euler(0f, 0f, zRotationValue);

        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne);

        RemoveFromPool();
    }

    /// <summary>
    /// Spawna bullets de tres posições sem rotação: pCentral, pLeftOne e pRightOne;
    /// </summary>
    public void SetupThree()
    {
        zRotationValue = 0f;

        var pCentral = positions.GetValueOrDefault(PositionNames.pCentral);
        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);
        
        pCentral.rotation = Quaternion.Euler(0f, 0f, zRotationValue);
        pLeftOne.rotation = Quaternion.Euler(0f, 0f, zRotationValue);
        pRightOne.rotation = Quaternion.Euler(0f, 0f, zRotationValue);

        spawnPosition.Add(pCentral);
        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne);

        RemoveFromPool();
    }

    /// <summary>
    /// Spawna bullets de tres posições com rotação: pCentral, pLeftOne e pRightOne;
    /// </summary>
    public void SetupFour()
    {
        zRotationValue = 3f;

        var pCentral = positions.GetValueOrDefault(PositionNames.pCentral);
        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);

        pCentral.rotation = Quaternion.Euler(0, 0, 0);
        pLeftOne.rotation = Quaternion.Euler(0, 0, zRotationValue);
        pRightOne.rotation = Quaternion.Euler(0, 0, -zRotationValue);

        spawnPosition.Add(pCentral);
        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne);

        RemoveFromPool();
    }

    /// <summary>
    /// Spawna bullets de quatro posições sem rotação: pLeftOne, pRightOne, pLeftTwo e pRightTwo;
    /// </summary>
    public void SetupFive()
    {
        zRotationValue = 3;

        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);

        var pLeftTwo = positions.GetValueOrDefault(PositionNames.pLeftTwo);
        var pRightTwo = positions.GetValueOrDefault(PositionNames.pRightTwo);

        pLeftOne.rotation = Quaternion.Euler(0, 0, 0);
        pRightOne.rotation = Quaternion.Euler(0, 0, 0);

        pLeftTwo.rotation = Quaternion.Euler(0, 0, zRotationValue);
        pRightTwo.rotation = Quaternion.Euler(0, 0, -zRotationValue);

        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne); 

        spawnPosition.Add(pLeftTwo);
        spawnPosition.Add(pRightTwo); 
        
        RemoveFromPool();
    }

    /// <summary>
    /// Spawna bullets de quatro posições com rotação: pLeftOne, pRightOne, pLeftTwo e pRightTwo;
    /// </summary>
    public void SetupSix()
    {
        zRotationValue = 3;


        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);

        var pLeftTwo = positions.GetValueOrDefault(PositionNames.pLeftTwo);
        var pRightTwo = positions.GetValueOrDefault(PositionNames.pRightTwo);

        pLeftOne.rotation = Quaternion.Euler(0, 0, zRotationValue);
        pRightOne.rotation = Quaternion.Euler(0, 0, -zRotationValue);

        pLeftTwo.rotation = Quaternion.Euler(0, 0, zRotationValue * 2);
        pRightTwo.rotation = Quaternion.Euler(0, 0, -zRotationValue * 2);

        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne);

        spawnPosition.Add(pLeftTwo);
        spawnPosition.Add(pRightTwo);

        RemoveFromPool();
    }

    /// <summary>
    /// Spawna bullets de cinco posições com rotação: pCentral, pLeftOne, pRightOne, pLeftTwo e pRightTwo;
    /// </summary>
    public void SetupSeven()
    {
        zRotationValue = 3;

        var pCentral = positions.GetValueOrDefault(PositionNames.pCentral);

        var pLeftOne = positions.GetValueOrDefault(PositionNames.pLeftOne);
        var pRightOne = positions.GetValueOrDefault(PositionNames.pRightOne);

        var pLeftTwo = positions.GetValueOrDefault(PositionNames.pLeftTwo);
        var pRightTwo = positions.GetValueOrDefault(PositionNames.pRightTwo);

        pCentral.rotation = Quaternion.Euler(0, 0, 0);

        pLeftOne.rotation = Quaternion.Euler(0, 0, zRotationValue);
        pRightOne.rotation = Quaternion.Euler(0, 0, -zRotationValue);

        pLeftTwo.rotation = Quaternion.Euler(0, 0, zRotationValue * 2);
        pRightTwo.rotation = Quaternion.Euler(0, 0, -zRotationValue * 2);


        spawnPosition.Add(pCentral);

        spawnPosition.Add(pLeftOne);
        spawnPosition.Add(pRightOne);

        spawnPosition.Add(pLeftTwo);
        spawnPosition.Add(pRightTwo);

        RemoveFromPool();
    }


    private void RemoveFromPool() 
    {
        bulletSetupController.RemoveFromPool();
    }
}
