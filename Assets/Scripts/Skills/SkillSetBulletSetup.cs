using UnityEngine;
using UnityEngine.Events;

public class SkillSetBulletSetup : MonoBehaviour, ISetPoolReference
{
    public BulletSetup selectedSetup;
    private IBackToPool myPool;

    [Space(5)]
    [SerializeField] private UnityEvent OnHitPlayer;
    [SerializeField] private UnityEvent OnHitGround;

    private ISetBulletSetup setBulletSetup;

    public void SetPoolReference(IBackToPool poolReference)
    {
        myPool = poolReference;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(setBulletSetup == null)
            {
                var player = collision.gameObject;
                setBulletSetup = player.GetComponentInChildren<ISetBulletSetup>();
            }

            OnHitPlayer.Invoke();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            OnHitGround.Invoke();
        }
    }

    public void SetBulletSetupState()
    {
        setBulletSetup.SetBulletSetup(selectedSetup);
    }

    public void BackToPool()
    {
        myPool.BackToPool(gameObject);
    }

}
