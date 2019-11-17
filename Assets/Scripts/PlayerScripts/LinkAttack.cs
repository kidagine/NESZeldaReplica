using UnityEngine;

public class LinkAttack : MonoBehaviour
{
    private enum AttackType { Sword, SwordBeam, Arrow, Bomb, Boomerang }
    [SerializeField] private AttackType _attackType = default;
    [SerializeField] private Inventory _invetory = default;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            switch (_attackType)
            {
                case AttackType.Sword:
                    SwordAttack(enemy);
                    break;
                case AttackType.Boomerang:
                    enemy.Stun();
                    break;
                case AttackType.SwordBeam:
                    enemy.Damage(gameObject, 1);
                    break;
                case AttackType.Arrow:
                    enemy.Damage(gameObject, 2);
                    break;
                case AttackType.Bomb:
                    enemy.Damage(gameObject, 4);
                    break;
            }
        }
    }
    
    private void SwordAttack(IEnemy enemy)
    {
        if (_invetory.CheckPassiveItem(ItemType.MagicalSword))
        {
            enemy.Damage(gameObject, 4);
        }
        else if (_invetory.CheckPassiveItem(ItemType.WhiteSword))
        {
            enemy.Damage(gameObject, 2);
        }
        else
        {
            enemy.Damage(gameObject, 1);
        }
    }
}
