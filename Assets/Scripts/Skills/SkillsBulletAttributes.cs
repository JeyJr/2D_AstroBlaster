using UnityEngine.Events;
using UnityEngine;

public class SkillsBulletAttributes : MonoBehaviour, ISetPoolReference
{
    private IBackToPool myPool;

    [Space(5)]
    [SerializeField] private UnityEvent OnHitPlayer;
    [SerializeField] private UnityEvent OnHitGround;



    public void SetPoolReference(IBackToPool poolReference)
    {
        myPool = poolReference;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHitPlayer.Invoke();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            OnHitGround.Invoke();
        }
    }

    public void IncreaseDMG()
    {
        GameData.GetInstance().IncreaseDamage();
    }

    public void SubDelayToSpawnBullet()
    {
        GameData.GetInstance().DecreaseDelayTimeToSpawnBullets();
    }

    public void IncreaseMoveSpeed()
    {
        GameData.GetInstance().IncreaseMoveSpeed();
    }

    public void BackToPool()
    {
        myPool.BackToPool(gameObject);
    }
}
