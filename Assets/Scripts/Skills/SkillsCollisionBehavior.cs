using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SkillsCollisionBehavior : MonoBehaviour, ISetPoolReference
{
    private IBackToPool myPool;


    [Space(5)]
    [Header("Bullet Type")]
    [SerializeField] private Sprite bulletSprite;

    [Header("Bonus")]
    [SerializeField] private float bonusPlayerLife = 0;
    [SerializeField] private float bonusBulletDamage = 0;
    [SerializeField] private float bonusBulletDelayTime = 0;
    [SerializeField] private float bonusBulletMoveSpeed = 0;


    [Space(5)]
    [SerializeField] private UnityEvent OnHitPlayer;
    [SerializeField] private GameObject player;


    public void SetPoolReference(IBackToPool poolReference)
    {
        myPool = poolReference;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(player == null)
            {
                player = collision.gameObject;
            }

            OnHitPlayer.Invoke();
            BackToPool();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            BackToPool();
        }
    }

    //Definir o tipo de bullet
    public void SkillSetNewBulletType()
    {
        if (bulletSprite != null)
        {
            player.GetComponentInChildren<ISetBulletType>().SetNewBulletSprite(bulletSprite);
        }
    }

    #region Bullet: Controle de dano
    public void SkillIncreaseDMG()
    {
        GameData.GetInstance().IncreaseBulletDamage();
    }

    public void SkillBonusBulletDMG()
    {
        GameData.GetInstance().SetBulletBonusDamage(bonusBulletDamage);
    }
    #endregion

    #region Bullet: Controle de velocidade de movimento e spawn
    public void SkillIncreaseBulletMoveSpeed()
    {
        GameData.GetInstance().IncreaseBulletMoveSpeed();
    }

    public void SkillBonusMoveSpeed()
    {
        GameData.GetInstance().SetBonusBulletMoveSpeed(bonusBulletMoveSpeed);
    }

    public void SkillSubDelayToSpawnBullet()
    {
        GameData.GetInstance().DecreaseDelayTimeToSpawnBullets();
    }
    public void SkillBonusDelayToSpawnBullet()
    {
        GameData.GetInstance().SetBonusDelayTimeToSpawnBullet(bonusBulletDelayTime);
    }

    #endregion

    #region Player: Movimentação
    public void SkillIncreasePlayerMoveSpeed()
    {
        GameData.GetInstance().IncreasePlayerMoveSpeed();
        player.GetComponent<SpaceShipController>().MoveSpeed = GameData.GetInstance().GetPlayerMoveSpeed();
    }

    #endregion

    #region Player: Controle da vida
    public void SkillIncreaseMaxLife()
    {
        GameData.GetInstance().IncreasePlayerMaxLife();
        player.GetComponent<IUpdateCanvasLife>().UpdateLife();
    }

    public void SkillRecoveryLife()
    {
        GameData.GetInstance().RecoveryPlayerLife();
        player.GetComponent<IUpdateCanvasLife>().UpdateLife();
    }

    public void SkillBonusLife()
    {
        GameData.GetInstance().SetBonusLife(bonusPlayerLife);
    }
    #endregion

    //Standard-Voltar para pool nas colisoes 
    private void BackToPool()
    {
        myPool.BackToPool(gameObject);
    }


}
