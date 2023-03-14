using UnityEngine;

public class SkillBulletSetup : MonoBehaviour, ISetPoolReference
{
    private IBackToPool myPool;
    [SerializeField] private BulletSetup selectedSetup;

    public void SetPoolReference(IBackToPool poolReference)
    {
        myPool = poolReference;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject;
            if (selectedSetup != BulletSetup.empty)
            {
                player.GetComponentInChildren<ISetBulletSetup>().SetBulletSetup(selectedSetup);
            }

            BackToPool();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            BackToPool();
        }
    }

    private void BackToPool()
    {
        myPool.BackToPool(gameObject);
    }
}
