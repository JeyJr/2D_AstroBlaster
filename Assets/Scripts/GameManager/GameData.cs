using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameData : MonoBehaviour
{
    [Space(5)]
    [Header("Points")]
    [SerializeField] private int points = 0;
    [SerializeField] private UnityEvent<int> OnAddPoint;

    [Space(5)]
    [Header("BulletDamage")]
    [SerializeField] private float damage = 1;


    [Space(5)]
    [Header("BulletDelaySpawn")]
    [SerializeField] private float delayToSpawnBullet = 1;
    [SerializeField] private float minDelayValueToSpawnBullet = .15f;
    [SerializeField] private float subValue = .05f;

    [Space(5)]
    [Header("BulletMoveSpeed")]
    [SerializeField] private float bulletMoveSpeed = 8;
    [SerializeField] private float maxBulletMoveSpeed = 30;


    //Instance
    private static GameData instance;
    public static GameData GetInstance() => instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #region Points: AddPoint, GetPoint
    public void AddPoint()
    {
        points++;
        OnAddPoint.Invoke(points);
    }

    public int GetPoint()
    {
        return points;
    }
    #endregion

    #region BulletDamage: GetDamage, IncreaseDamage
    public float GetDamage() 
    { 
        return damage;
    }
    public void IncreaseDamage()
    {
        damage ++;
    }
    #endregion

    #region BulletDelaySpawn: SubtractDelayTimeToSpawnBullets, GetDelayToSpawnBullet
    public void DecreaseDelayTimeToSpawnBullets()
    {
        if(delayToSpawnBullet > minDelayValueToSpawnBullet)
        {
            delayToSpawnBullet -= subValue;
        }
    }
    public float GetDelayToSpawnBullet()
    {
        return delayToSpawnBullet;
    }
    #endregion

    public void IncreaseMoveSpeed()
    {
        if(bulletMoveSpeed < maxBulletMoveSpeed)
        {
            bulletMoveSpeed++;
        }
    }
    public float GetMoveSpeed()
    {
        return bulletMoveSpeed;
    }

    //Dificuldade do jogo
    [SerializeField] private int dificuldade = 1;



    
}
