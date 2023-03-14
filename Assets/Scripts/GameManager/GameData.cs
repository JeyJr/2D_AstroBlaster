using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameData : MonoBehaviour
{
    [Space(5)]
    [Header("Points")]
    [SerializeField] private int points = 0;

    //BulletDamage
    private float bonusDamage = 0;
    private float damage = 1;

    //BulletDelaySpawn
    private float bonusDelayToSpawnBullet = 0; 
    private float delayToSpawnBullet = 1;
    private readonly float minDelayValueToSpawnBullet = .15f;
    private readonly float subValue = .05f;

    //BulletMoveSpeed
    private float bonusBulletMoveSpeed = 0;
    private float bulletMoveSpeed = 8;
    private readonly float maxBulletMoveSpeed = 30;

    //PlayerMoveSpeed
    private float playerMoveSpeed = 3;
    private readonly float maxPlayerMoveSpeed = 15;


    //PlayerLife
    private float bonusLife = 0;
    private float maxLife = 10;
    private float currentLife = 10;

    //Instance
    private static GameData instance;
    public static GameData GetInstance() => instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        InitialRecord();
    }

    private void Start()
    {
        GameEvents.GetInstance().OnMainMenu += InitialSetup;
        GameEvents.GetInstance().OnAddPoint += AddPoint;
    }

    private void InitialSetup()
    {
        points = 0;

        bonusDamage = 0;
        damage = 1;

        bonusDelayToSpawnBullet = 0;
        delayToSpawnBullet = 1;

        bonusBulletMoveSpeed = 0;
        bulletMoveSpeed = 8;

        playerMoveSpeed = 3;

        bonusLife = 0;
        maxLife = 10;

        currentLife = maxLife;
    }

    #region Points: AddPoint, GetPoint
    public void AddPoint()
    {
        points++;
    }

    public int GetPoint()
    {
        return points;
    }

    private void InitialRecord()
    {
        if (!PlayerPrefs.HasKey("record"))
        {
            PlayerPrefs.SetInt("record", 0);
        }
    }

    public int GetRecord()
    {
        return PlayerPrefs.GetInt("record");
    }

    public void SetRecorde(int value)
    {
        PlayerPrefs.SetInt("record", value);
    }

    #endregion

    #region BulletDamage: GetDamage, IncreaseDamage, SetBulletBonusDamage
    //Controle do dano que as bullets causam nos alvos
    public float GetBulletDamage()
    {
        return damage + bonusDamage;
    }
    public void IncreaseBulletDamage()
    {
        damage++;
    }

    public void SetBulletBonusDamage(float value)
    {
        bonusDamage = value;
    }
    #endregion

    #region BulletDelaySpawn: SubtractDelayTimeToSpawnBullets, GetDelayToSpawnBullet
    //Controlar todo o comportamento do TEMPO de spawn dos tiros

    public void DecreaseDelayTimeToSpawnBullets()
    {
        if (delayToSpawnBullet > minDelayValueToSpawnBullet)
        {
            delayToSpawnBullet -= subValue;
        }
    }
    public float GetDelayToSpawnBullet()
    {
        if(delayToSpawnBullet - bonusDelayToSpawnBullet < minDelayValueToSpawnBullet)
        {
            return minDelayValueToSpawnBullet;
        }

        return delayToSpawnBullet - bonusDelayToSpawnBullet;
    }

    public void SetBonusDelayTimeToSpawnBullet(float value)
    {
        bonusDelayToSpawnBullet = value;
    }
    #endregion

    #region BulletMoveSpeed: IncreaseBulletMoveSpeed, GetMoveSpeed
    //Controle da velocidade de movimentação das bullets
    public void IncreaseBulletMoveSpeed()
    {
        if (bulletMoveSpeed < maxBulletMoveSpeed)
        {
            bulletMoveSpeed++;
        }
    }
    public float GetBulletMoveSpeed()
    {
        if(bulletMoveSpeed + bonusBulletMoveSpeed > maxBulletMoveSpeed)
        {
            return maxBulletMoveSpeed;
        }

        return bulletMoveSpeed  + bonusBulletMoveSpeed;
    }

    public void SetBonusBulletMoveSpeed(float value)
    {
        bonusBulletMoveSpeed = value;
    }

    #endregion

    #region PlayerMoveSpeed: IncreasePlayerMoveSpeed, GetPlayerMoveSpeed
    //Controle de movimentação do personagem
    public void IncreasePlayerMoveSpeed()
    {
        if (playerMoveSpeed < maxPlayerMoveSpeed)
        {
            playerMoveSpeed++;
        }
    }

    public float GetPlayerMoveSpeed()
    {
        return playerMoveSpeed;
    }

    #endregion

    #region PlayerLifeControl: SetBonusLife, GetLife, DecreaseLife, IncreaseMaxLife, RecoveryLife
    //Controle da vida do personagem: Quando colidir com targets ou quando ganhar algum buff com skills
    public void SetBonusLife(float value)
    {
        bonusLife = value;
    }
    
    public float GetPlayerLife()
    {
        return currentLife;
    }

    public float GetPlayerMaxLife()
    {
        return maxLife + bonusLife;
    }

    public void DecreasePlayerLife()
    {
        currentLife --;

        if(currentLife < 1)
        {
            GameController.GetInstance().SetGameState(GameState.EndGame);
        }
    }

    public void IncreasePlayerMaxLife()
    {
        maxLife++;
        currentLife = maxLife;
    }

    public void RecoveryPlayerLife()
    {
        currentLife = maxLife;
    }
    #endregion

    #region AsteroidLife:

    public float GetAsteroidLife()
    {
        float lifeValue = damage + bonusDamage;
        return Random.Range(lifeValue * 5, lifeValue * 15);
    }

    #endregion

}
